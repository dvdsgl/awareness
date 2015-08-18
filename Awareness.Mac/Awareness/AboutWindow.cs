
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Awareness
{
    public partial class AboutWindow : MonoMac.AppKit.NSWindow
    {
        #region Constructors

        // Called when created from unmanaged code
        public AboutWindow (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public AboutWindow (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        // Shared initialization code
        void Initialize ()
        {
        }
        
        #endregion
    }
}

