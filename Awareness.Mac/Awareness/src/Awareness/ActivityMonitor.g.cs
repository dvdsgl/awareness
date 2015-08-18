//
// Auto-generated from generator.cs, do not edit
//
// We keep references to objects, so warning 414 is expected

#pragma warning disable 414

using System;

using System.Runtime.InteropServices;

using MonoMac.CoreFoundation;

using MonoMac.Foundation;

using MonoMac.ObjCRuntime;

using MonoMac.CoreGraphics;

using MonoMac.CoreAnimation;

using MonoMac.CoreLocation;

using MonoMac.QTKit;

using MonoMac.CoreVideo;

using MonoMac.OpenGL;

namespace Awareness {
	//[Register("ActivityMonitor")]
	public partial class ActivityMonitor : NSObject {
		static IntPtr selSystemIdleTime = Selector.GetHandle ("systemIdleTime");

		static IntPtr class_ptr = Class.GetHandle ("ActivityMonitor");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public  ActivityMonitor () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public ActivityMonitor (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public ActivityMonitor (NSObjectFlag t) : base (t) {}

		public ActivityMonitor (IntPtr handle) : base (handle) {}

		[Export ("systemIdleTime")]
		public static int GetSystemIdleTime ()
		{
			return (int) MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selSystemIdleTime);
		}

	
	} /* class ActivityMonitor */
}
