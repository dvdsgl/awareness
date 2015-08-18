//
//  LoginItemManager.m
//  Native
//
//  Created by David Siegel on 4/23/11.
//  Copyright 2011 Futureproof LLC. All rights reserved.
//

#import "LoginItemManager.h"
#import <CoreServices/CoreServices.h>

NSString *GetSharedListFileItemPath (LSSharedFileListItemRef itemRef)
{	
	CFURLRef url;
	if (LSSharedFileListItemResolve (itemRef, 0, (CFURLRef *)&url, NULL) == noErr) {
		return [(NSURL *)url path];
	}
	return nil;
}

LSSharedFileListRef GetLoginItems ()
{
	return LSSharedFileListCreate (NULL, kLSSharedFileListSessionLoginItems, NULL);
}

NSArray *GetLoginItemsArray (LSSharedFileListRef loginItems)
{
	UInt32 seed;
	NSArray* items = (NSArray *) LSSharedFileListCopySnapshot (loginItems, &seed);
	return [items autorelease];
}

BOOL LoginItemsAppend (LSSharedFileListRef itemsRef, NSString *path)
{
	BOOL success = NO;
	LSSharedFileListItemRef item;
	CFURLRef url = (CFURLRef)[NSURL fileURLWithPath:path];
	
	if ((item = LSSharedFileListInsertItemURL (itemsRef, kLSSharedFileListItemLast, NULL, NULL, url, NULL, NULL))) {
		success = YES;
		CFRelease (item);
	}
	return success;
}

BOOL LoginItemsRemove (LSSharedFileListRef itemsRef, NSString *path)
{
	BOOL success = NO;
	NSArray *items = GetLoginItemsArray (itemsRef);
	for (int i = 0; i < [items count]; i++){
		LSSharedFileListItemRef itemRef = (LSSharedFileListItemRef)[items objectAtIndex:i];
		if ([path compare:GetSharedListFileItemPath(itemRef)] == NSOrderedSame) {
			LSSharedFileListItemRemove (itemsRef, itemRef);
			success = YES;
			break;
		}
	}
	return success;
}

@implementation LoginItemManager

+ (NSArray *)loginItems
{
    LSSharedFileListRef itemsRef;
	NSMutableArray *paths = [NSMutableArray array];
	
	if ((itemsRef = GetLoginItems ())) {
		NSArray *items = GetLoginItemsArray (itemsRef);	
		for (int i = 0; i < [items count]; i++) {
			LSSharedFileListItemRef itemRef = (LSSharedFileListItemRef)[items objectAtIndex:i];
			NSString *path = GetSharedListFileItemPath (itemRef);
			if (path != nil) {
				[paths addObject:path];
			}
		}
        CFRelease (itemsRef);
	}
	return paths;
}

+ (void) addLoginItem:(NSString *)path
{
	LSSharedFileListRef itemsRef;
	
	if ([[self loginItems] containsObject:path]) {
	} else if ((itemsRef = GetLoginItems ())) {
		LoginItemsAppend (itemsRef, path);
		CFRelease (itemsRef);
	}
}

+ (void) removeLoginItem:(NSString *)path
{
	LSSharedFileListRef itemsRef;
	
	if (![[self loginItems] containsObject:path]) {
	} else if ((itemsRef = GetLoginItems ())) {
		LoginItemsRemove (itemsRef, path);
		CFRelease (itemsRef);
	}
}

@end
