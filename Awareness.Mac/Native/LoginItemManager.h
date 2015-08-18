//
//  LoginItemManager.h
//  Native
//
//  Created by David Siegel on 4/23/11.
//  Copyright 2011 Futureproof LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>

@interface LoginItemManager : NSObject {

}

+ (NSArray *) loginItems;
+ (void) addLoginItem:(NSString *)path;
+ (void) removeLoginItem:(NSString *)path;

@end
