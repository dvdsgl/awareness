
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Awareness
{
    public partial class AboutWindowController : MonoMac.AppKit.NSWindowController
    {
        #region Constructors

        // Called when created from unmanaged code
        public AboutWindowController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public AboutWindowController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        // Call to load from the XIB/NIB file
        public AboutWindowController () : base("AboutWindow")
        {
            Initialize ();
        }

        #endregion

        //strongly typed window accessor
        public new AboutWindow Window {
            get { return (AboutWindow)base.Window; }
        }
    }
}

