using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cadenza;

namespace Awareness.Agnostic
{
    public abstract class AbstractPreferences
    {
        public const string BowlVolumePercentageKey = "BowlVolumePercentage";
        public const string PlayBowlOnStartKey = "PlayBowlOnStart";
        public const string StartAtLoginKey = "StartAtLogin";
        public const string IsFirstRunKey = "IsFirstRun";
        public const string MaxActivitySecondsKey = "MaxActivitySeconds";
        public const string MinBreakSecondsKey = "MinBreakSeconds";
        
        const int MinBreakSecondsDefault = 5 * 60;
        const int MaxActivitySecondsDefault = 60 * 60;

        protected abstract int GetInt (string key, int dfault);
        protected abstract void SetInt (string key, int value);

        protected abstract bool GetBool (string key, bool dfault);
        protected abstract void SetBool (string key, bool value);
        
        public event EventHandler<PreferencesEventArgs> Changed;
        
        protected virtual IEnumerable<string> Keys {
            get {
                return new [] {
                    BowlVolumePercentageKey,
                    PlayBowlOnStartKey,
                    IsFirstRunKey,
                    MaxActivitySecondsKey,
                    MinBreakSecondsKey,
                    StartAtLoginKey
                };
            }
        }

        public bool IsFirstRun {
            get {
                return GetBool (IsFirstRunKey, true);
            }
            set {
                SetBool (IsFirstRunKey, value);
            }
        }
        
        public bool StartAtLogin {
            get {
                return GetBool (StartAtLoginKey, true);
            }
            set {
                SetBool (StartAtLoginKey, value);
            }
        }
        
        public bool PlayBowlOnStart {
            get {
                return GetBool (PlayBowlOnStartKey, true);
            }
            set {
                SetBool (PlayBowlOnStartKey, value);
            }
        }
        
        public int BowlVolumePercentage {
            get {
                return GetInt (BowlVolumePercentageKey, 80);
            }
            set {
                SetInt (BowlVolumePercentageKey, value);
            }
        }

        public TimeSpan MaxActivity {
            get {
                return TimeSpan.FromSeconds (GetInt (MaxActivitySecondsKey, MaxActivitySecondsDefault));
            }
            set {
                SetInt (MaxActivitySecondsKey, (int)value.TotalSeconds);
            }
        }

        public TimeSpan MinBreak {
            get {
                return TimeSpan.FromSeconds (GetInt (MinBreakSecondsKey, MinBreakSecondsDefault));
            }
            set {
                SetInt (MinBreakSecondsKey, (int)value.TotalSeconds);
            }
        }
        
        protected void RaiseChange (string key)
        {
           Changed.Raise (this, new PreferencesEventArgs (key));
        }
    }
}
