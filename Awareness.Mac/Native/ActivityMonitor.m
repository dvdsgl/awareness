//
//  ActivityMonitor.m
//  Native
//
//  Created by David Siegel on 5/1/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "ActivityMonitor.h"
#import <IOKit/IOKitLib.h>

int GetSystemIdleTime ()
{
    int idlesecs = -1;
    io_iterator_t iter = 0;
    if (IOServiceGetMatchingServices(kIOMasterPortDefault, IOServiceMatching("IOHIDSystem"), &iter) == KERN_SUCCESS) {
        io_registry_entry_t entry = IOIteratorNext(iter);
        if (entry) {
            CFMutableDictionaryRef dict = NULL;
            if (IORegistryEntryCreateCFProperties(entry, &dict, kCFAllocatorDefault, 0) == KERN_SUCCESS) {
                CFNumberRef obj = CFDictionaryGetValue(dict, CFSTR("HIDIdleTime"));
                if (obj) {
                    int64_t nanoseconds = 0;
                    if (CFNumberGetValue(obj, kCFNumberSInt64Type, &nanoseconds)) {
                        idlesecs = (nanoseconds / 1000000000); // Divide by 10^9 to convert from nanoseconds to seconds.
                    }
                }
                CFRelease(dict);
            }
            IOObjectRelease(entry);
        }
        IOObjectRelease(iter);
    }
    return idlesecs;
} 


@implementation ActivityMonitor

+ (int) systemIdleTime
{
    return GetSystemIdleTime();
}

@end
