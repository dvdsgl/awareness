using System;

using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace Awareness
{
	[BaseType (typeof (NSObject))]
	interface LoginItemManager {
		[Static]
		[Export ("loginItems")]
		NSString[] GetLoginItems ();

		[Static]
		[Export ("addLoginItem:")]
		void AddLoginItem (string path);

	}

	[BaseType (typeof (NSObject))]
	interface ActivityMonitor {
		[Static]
		[Export ("systemIdleTime")]
		int SystemIdleTime ();
	}
}
