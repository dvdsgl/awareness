using System;
using System.Runtime.InteropServices;

namespace Awareness
{
	public static class Runtime
	{
		[DllImport("/usr/lib/libc.dylib")]
		extern static IntPtr dlopen (string path, int mode);
		
		public static bool IsInitialized { get; private set; }
		
		public static MacPlatform Initialize (MacPlatform platform)
		{
			if (IsInitialized)
				throw new InvalidOperationException ("Runtime cannot be Initialized twice.");
			
			// Initialize Native library
			dlopen (platform.ResourceNamed ("Native.dylib"), 0x1);
			
			IsInitialized = true;
			
			return platform;
		}
	}
}

