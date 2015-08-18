using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using MonoMac.AppKit;
using MonoMac.Foundation;

using Cadenza;
using Cadenza.Collections;

using Awareness.Agnostic;

namespace Awareness
{
    partial class AppDelegate
    {
        MacPlatform Platform;
        iTunes iTunes;
        Maybe<NSStatusItem> Status;
        AwarenessController Controller;
        Maybe<AboutWindowController> AboutWindowController;
        Maybe<PreferencesWindowController> PreferencesWindowController;

        /// <summary>
        /// Convenient way to set the titles of BreakInfoMenuItem and BreakInfoMenuItemDock to the same string. 
        /// </summary>
        string BreakInfoMenuItemTitle {
            get { return BreakInfoMenuItem.Title; }
            set { BreakInfoMenuItem.Title = BreakInfoMenuItemDock.Title = value; }
        }
        
        bool AppStartAtLogin {
            get {
                return LoginItemManager.GetLoginItems ().Any (path =>
                 path.ToString ().EndsWith ("Awareness.app")
             );
            }
            set {
                if (value) {
                    LoginItemManager.AddLoginItem (NSBundle.MainBundle.BundlePath);  
                } else {
                    LoginItemManager.RemoveLoginItem (NSBundle.MainBundle.BundlePath);
                }
            }
        }

        public override void AwakeFromNib ()
        {
            Platform = Runtime.Initialize (new MacPlatform ());
            // Do this before we start listening for notifications.
            Platform.Preferences.StartAtLogin = AppStartAtLogin;
            Platform.Preferences.Changed += PreferencesChanged;
         
            Controller = new AwarenessController (Platform);
            Controller.FirstRun += FirstRun;
            Controller.BowlPlayer.WillPlay += BowlPlayerWillPlay;
            Controller.BowlPlayer.StartedPlaying += BowlPlayerStartedPlaying;
            Controller.BreakTimer.BreakChecked += HandleBreakChecked;

            iTunes = new iTunes (Platform);
         
            if (Platform.MacPreferences.ShowInMenuBar)
                CreateAndDisplayStatusItem ();
            
            base.AwakeFromNib ();
        }

        void PreferencesChanged (object sender, PreferencesEventArgs e)
        {
            switch (e.Key) {
            case MacPreferences.ShowInMenuBarKey:
                if (Platform.MacPreferences.ShowInMenuBar) {
                    CreateAndDisplayStatusItem ();
                } else {
                    HideStatusItem ();
                }
                break;
            case MacPreferences.StartAtLoginKey:
                AppStartAtLogin = Platform.Preferences.StartAtLogin;
                break;
            case MacPreferences.ShowInDockKey:
                NSApplication.SharedApplication.SetIsShownInDock (Platform.MacPreferences.ShowInDock);
                break;
            }
        }

        void FirstRun (object sender, EventArgs e)
        {
            DisplayAboutWindow ();
        }

        void BowlPlayerWillPlay (object sender, BowlPlayerEventArgs e)
        {
            if (Controller.BowlPlayer.IsPlaying) return;
            
            var duration = BowlPlayer.TimeBetweenBowls.Seconds * (e.TimesPlayed - 1) + BowlSound.SoundDuration.Seconds;
            iTunes.GoQuiet (duration.Seconds ());
         
            // We sleep for a moment to let iTunes go quiet.
            Platform.ThreadSleep (1.Second ());
        }

        void BowlPlayerStartedPlaying (object sender, BowlPlayerEventArgs e)
        {
            if (Controller.BowlPlayer.IsPlaying) return;
            
            NSApplication.SharedApplication.RequestUserAttention (NSRequestUserAttentionType.InformationalRequest);
        }

        void HandleBreakChecked (object sender, EventArgs e)
        {
            UpdateViews ();
        }

        void CreateAndDisplayStatusItem ()
        {
            // TASK: Find and use NSVariableStatusItemLength constant instead of literal
            Status = NSStatusBar.SystemStatusBar.CreateStatusItem (/*NSVariableStatusItemLength*/ -1).ToMaybe ();
            foreach (var status in Status) {
                status.Menu = StatusMenu;
                status.HighlightMode = true;
            }
            UpdateViews ();
        }

        void HideStatusItem ()
        {
            foreach (var status in Status) {
                NSStatusBar.SystemStatusBar.RemoveStatusItem (status);
            }
            Status = Maybe<NSStatusItem>.Nothing;
        }

        void UpdateViews ()
        {
            if (Platform.MacPreferences.ShowInMenuBar) {
                foreach (var status in Status) {
                    status.Title = Controller.BreakTimer.ActivityTime.ToHoursAndMinutes ();
                }
            }
            BreakInfoMenuItemTitle = string.Format ("{0} since last break", Controller.BreakTimer.ActivityTime.ToHoursAndMinutesLong ());
        }

        partial void OpenAwarenessWebsite (NSMenuItem sender)
        {
            Platform.OpenUrl (Constants.AwarenessWebsiteUrl);
        }

        partial void OpenBreakIdeas (NSMenuItem sender)
        {
            Platform.OpenUrl (Constants.BreakIdeasUrl);
        }

        partial void DisplayAboutWindow (NSMenuItem sender)
        {
            DisplayAboutWindow ();
        }

        void DisplayAboutWindow ()
        {
            if (!AboutWindowController.HasValue) {
                AboutWindowController = new AboutWindowController (Platform).ToMaybe ();
                AboutWindowController.Value.Window.WillClose += delegate {
                    AboutWindowController = Maybe<AboutWindowController>.Nothing;
                };
            }
            foreach (var controller in AboutWindowController)
                controller.ShowWindow (this);
        }
  
        partial void DisplayPreferences (NSMenuItem sender)
        {   
            if (!PreferencesWindowController.HasValue) {
                PreferencesWindowController = new PreferencesWindowController (Platform, Controller.BowlPlayer).ToMaybe ();
                PreferencesWindowController.Value.Window.WillClose += delegate {
                    PreferencesWindowController = Maybe<PreferencesWindowController>.Nothing;
                };
            }
            
            foreach (var controller in PreferencesWindowController)
                controller.ShowWindow (this);
        }
                
        partial void OpenHelpWebsite (NSMenuItem sender)
        {
            Platform.OpenUrl (Constants.AwarenessSupportUrl);
        }
    }
}

