using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Awareness
{
    public partial class PreferencesWindowController : MonoMac.AppKit.NSWindowController
    {
		#region Constructors
		
        // Called when created from unmanaged code
        public PreferencesWindowController (IntPtr handle) : base (handle)
        {
            Initialize ();
        }
		
        // Called when created directly from a XIB file
		[Export ("initWithCoder:")]
        public PreferencesWindowController (NSCoder coder) : base (coder)
        {
            Initialize ();
        }
		
        // Call to load from the XIB/NIB file
        public PreferencesWindowController () : base ("PreferencesWindow")
        {
            Initialize ();
        }
		
		#endregion
		
        //strongly typed window accessor
        public new PreferencesWindow Window {
            get {
                return (PreferencesWindow)base.Window;
            }
        }
    }
}

