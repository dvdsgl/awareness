using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

using Cadenza;
using Cadenza.Collections;

using Awareness.Agnostic;

namespace Awareness
{
    public partial class PreferencesWindowController
    {
        readonly MacPlatform Platform;
        readonly BowlPlayer BowlPlayer;
        
        PreferencesWindowDelegate WindowDelegate;
        
        IDictionary<NSTextField, TimeTextFieldController> TimeControllers;

        public PreferencesWindowController (MacPlatform platform, BowlPlayer player) : base ("PreferencesWindow")
        {
            Platform = platform;
            BowlPlayer = player;
            Initialize ();
        }

        void Initialize ()
        {
            WindowDelegate = new PreferencesWindowDelegate (this);
            Window.Level = NSWindowLevel.Floating;
        }
        
        #region PreferencesWindowDelegate
        // TODO: Replace this class with Window.ShouldClose = delegate { ... } when the bug is fixed (http://bugzilla.xamarin.com/show_bug.cgi?id=159)
        class PreferencesWindowDelegate : NSWindowDelegate
        {
            readonly PreferencesWindowController Controller;
            
            public PreferencesWindowDelegate (PreferencesWindowController controller)
            {
                Controller = controller;
            }

            public override bool WindowShouldClose (NSObject sender)
            {
                return Controller.WindowShouldClose (sender);
            }
        }
        #endregion

        public bool WindowShouldClose (NSObject sender)
        {
            TimeTextFieldController invalid;
            if (TimeControllers.Values.TryGetFirst (c => !c.IsValid, out invalid)) {
                var alert = NSAlert.WithMessage (string.Format ("{0} is invalid", invalid.Name), "Revert Changes", null, null, invalid.ConstraintDescription);
                alert.BeginSheet (Window, this, new Selector ("alertDidEnd:returnCode:contextInfo:"), IntPtr.Zero);
                alert.RunModal ();
                return false;
            } else {
                TimeControllers.Values.ForEach (c => c.WriteTimeToSetting ());
                return true;
            }
        }
        
        [Export("alertDidEnd:returnCode:contextInfo:")]
        void AlertDidEnd (NSAlert alert, int returnCode, IntPtr contextInfo)
        {
            NSApplication.SharedApplication.AbortModal ();
            TimeControllers.Values.ForEach (c => c.ReadTimeFromSetting ());
        }

        public override void AwakeFromNib ()
        {
            Window.Center ();
            DisplayBowlVolumePercentage (Platform.Preferences.BowlVolumePercentage);
            
            TimeControllers = TimeTextFieldController.BuildControllers (Platform, WorkTime, BreakTime);
            TimeControllers.Values.ForEach (c => c.ReadTimeFromSetting ());
            
            // TODO: What is reseting the Window.Delegate?!
            NSTimer.CreateScheduledTimer (1.Second (), () => Window.Delegate = WindowDelegate);
        }
        
        partial void TimeChanged (NSTextField sender)
        {
            var controller = TimeControllers[sender];
            controller.Validate ();
            controller.WriteTimeToSetting ();
        }
        
        string VolumeLevelToDescription (int level)
        {
            switch (level / 25) {
            case 0: return "grasshopper";
            case 1: return "frog";
            case 2: return "owl";
            case 3: return "monkey";
            case 4: return "elephant";
            default: return "?";
            }
        }
        
        partial void ObserveBowlVolumeChange (NSSlider sender)
        {
            // TODO Why is sender sometimes null?
            if (sender == null) return;
            
            DisplayBowlVolumePercentage (sender.IntValue);
        }
        
        partial void TestBowlSound (NSButton sender)
        {
            BowlPlayer.Play ();
        }
        
        void DisplayBowlVolumePercentage (int percent)
        {
            BowlVolumePercentage.StringValue = VolumeLevelToDescription (percent);
        }
    } 
}

