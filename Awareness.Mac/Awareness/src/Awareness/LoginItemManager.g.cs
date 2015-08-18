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
	//[Register("LoginItemManager")]
	public partial class LoginItemManager : NSObject {
		static IntPtr selLoginItems = Selector.GetHandle ("loginItems");
		static IntPtr selAddLoginItem = Selector.GetHandle ("addLoginItem:");
        static IntPtr selRemoveLoginItem = Selector.GetHandle ("removeLoginItem:");

		static IntPtr class_ptr = Class.GetHandle ("LoginItemManager");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public  LoginItemManager () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public LoginItemManager (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public LoginItemManager (NSObjectFlag t) : base (t) {}

		public LoginItemManager (IntPtr handle) : base (handle) {}

		[Export ("loginItems")]
		public static NSString[] GetLoginItems ()
		{
			return NSArray.ArrayFromHandle<MonoMac.Foundation.NSString>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selLoginItems));
		}

		[Export ("addLoginItem:")]
		public static void AddLoginItem (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			var nspath = new NSString (path);

			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (class_ptr, selAddLoginItem, nspath.Handle);
			nspath.Dispose ();

		}
        
        [Export ("removeLoginItem:")]
        public static void RemoveLoginItem (string path)
        {
            if (path == null)
                throw new ArgumentNullException ("path");
            var nspath = new NSString (path);
            
            MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (class_ptr, selRemoveLoginItem, nspath.Handle);
            nspath.Dispose ();
        }

	
	} /* class LoginItemManager */
}
