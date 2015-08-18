using System;

using MonoMac.Foundation;

namespace Awareness
{
	public static class NSObjectExtensions
	{
		public static void AddObserver (this NSObject self, NSObject observer, string keypath)
		{
			self.AddObserver (observer, (NSString) keypath, NSKeyValueObservingOptions.New, IntPtr.Zero);
		}
	}
}

