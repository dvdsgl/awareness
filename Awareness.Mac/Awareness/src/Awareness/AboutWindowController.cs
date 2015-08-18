using System;
using System.Drawing;

using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

using Awareness.Agnostic;

namespace Awareness
{
    public partial class AboutWindowController
    {
        MacPlatform Platform;
        NSImage SplashImage;

        public AboutWindowController (MacPlatform platform) : base ("AboutWindow")
        {
            Platform = platform;
            Initialize ();
        }

        void Initialize ()
        {
            SplashImage = new NSImage (Platform.ResourceNamed ("splash.jpg"));
        }

        public override void AwakeFromNib ()
        {
            ImageView.Image = SplashImage;
            ImageView.Frame = new RectangleF (PointF.Empty, SplashImage.Size);
            Window.SetContentSize (SplashImage.Size);
            Window.WillClose += HandleWindowWillClose;
        }

        public override void ShowWindow (NSObject sender)
        {
            if (!Window.IsVisible)
                Window.Center ();
            base.ShowWindow (sender);
        }

        void HandleWindowWillClose (object sender, EventArgs e)
        {
            if (!Platform.MacPreferences.ShouldAskToStartAtLogin) return;
            
            var alert = new NSAlert {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = "Would you like Awareness to start at login?",
                InformativeText = "Unless Awareness starts automatically, you will likely forget to use it. What good would that do you?"
            };
            alert.AddButton ("Start Awareness at Login");
            alert.AddButton ("No Thanks");
            
            // TODO: Figure out why this freezes Awareness!
            // alert.BeginSheet (Window, this, new Selector ("alertDidEnd:returnCode:contextInfo:"), IntPtr.Zero);
            
            var pressed = (NSAlertButtonReturn) alert.RunModal ();
            if (pressed == NSAlertButtonReturn.First) {
                Log.Info ("User consented to starting Awareness automatically at login.");
                Platform.Preferences.StartAtLogin = true;
            }
            
            Platform.MacPreferences.HasAskedToStartAtLogin = true;
        }
    }
}

