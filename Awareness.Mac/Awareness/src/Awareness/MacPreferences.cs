using System;
using System.Linq;
using System.Collections.Generic;

using MonoMac.Foundation;
using MonoMac.AppKit;

using Cadenza;
using Cadenza.Collections;

using Awareness.Agnostic;

namespace Awareness
{
    public class MacPreferences : AbstractPreferences
    {
        public const string ShowInDockKey = "ShowInDock";
        public const string ShowInMenuBarKey = "ShowInMenuBar";
        public const string HasAskedToStartAtLoginKey = "HasAskedToStartAtLogin";
        
        readonly DefaultsObserver Observer;
  
        protected override IEnumerable<string> Keys {
            get {
                return base.Keys.Concat (new [] {
                    ShowInMenuBarKey,
                    ShowInDockKey,
                    HasAskedToStartAtLoginKey
                });
            }
        }
        
        protected NSUserDefaults Defaults {
            get { return NSUserDefaults.StandardUserDefaults; }
        }

        public bool ShowInMenuBar {
            get {
                return GetBool (ShowInMenuBarKey, true);
            }
        }

        public bool HasAskedToStartAtLogin {
            get {
                return GetBool (HasAskedToStartAtLoginKey, false);
            }
            set {
                SetBool (HasAskedToStartAtLoginKey, value);
            }
        }
        
        public bool ShowInDock {
            get {
                return GetBool (ShowInDockKey, true);
            }
            set {
                SetBool (ShowInDockKey, value);
            }
        }
        
        public bool ShouldAskToStartAtLogin {
            get {
                return !StartAtLogin && !HasAskedToStartAtLogin;
            }
        }
        
        public MacPreferences ()
        {
            Observer = new DefaultsObserver (RaiseChange);
            Keys.ForEach (key => Defaults.AddObserver (Observer, key));
        }
        
     #region implemented abstract members of AbstractPreferences
        
        protected override int GetInt (string key, int dfault)
        {
            return Defaults [key] == null ? dfault : Defaults.IntForKey (key);
        }

        protected override void SetInt (string key, int value)
        {
            Defaults.SetInt (value, key);
            RaiseChange (key);
        }

        protected override bool GetBool (string key, bool dfault)
        {
            return Defaults [key] == null ? dfault : Defaults.BoolForKey (key);
        }

        protected override void SetBool (string key, bool value)
        {
            Defaults.SetBool (value, key);
            RaiseChange (key);
        }
     
     #endregion
        
        class DefaultsObserver : NSObject
        {
            readonly Action<string> Observe;
            
            public DefaultsObserver (Action<string> observe)
            {
                Observe = observe;
            }
            
            public override void ObserveValue (NSString key, NSObject of, NSDictionary change, IntPtr context)
            {
                // TODO why is key sometimes null?
                if (key == null) return;
                
                Observe (key.ToString ());
            }
        }
    }
}

