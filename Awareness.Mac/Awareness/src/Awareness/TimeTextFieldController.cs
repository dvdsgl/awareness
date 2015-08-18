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
    abstract class TimeTextFieldController
    {
        public static IDictionary<NSTextField, TimeTextFieldController> BuildControllers (AbstractPlatform platform, NSTextField work, NSTextField break_)
        {
          var breakController = new BreakTimeTextFieldController (platform, break_);
          var workController = new WorkTimeTextFieldController (platform, work);

          breakController.Other = workController;
          workController.Other = breakController;

          return new Dictionary<NSTextField,TimeTextFieldController> { {work, workController}, {break_, breakController} };
        }

        protected readonly AbstractPlatform Platform;
        public readonly NSTextField Field;
        public readonly string Name;

        public TimeTextFieldController (AbstractPlatform platform, NSTextField field, string name)
        {
            Platform = platform;
            Field = field;
            Name = name;
        }

        public TimeTextFieldController Other { get; private set; }
        
        public abstract TimeSpan Setting { get; set; }
        public abstract TimeSpan MinBound { get; }
        public abstract TimeSpan MaxBound { get; }
        public abstract string ConstraintDescription { get; }
        
        public TimeSpan ValidTime {
            get {
                return Time.Select (t => t.Min (MaxBound).Max (MinBound)).GetValueOrDefault (Setting);
            }
        }
        
        public Maybe<TimeSpan> Time {
            get {
                int minutes;
                return int.TryParse (Field.StringValue, out minutes) ? minutes.Minutes ().ToMaybe () : Maybe<TimeSpan>.Nothing;
                //return Maybe.TryParse<int> (Field.StringValue).Select (TimeSpan.FromMinutes);
            }
        }

        public bool IsValid {
            get {
                return Time == ValidTime.ToMaybe ();
            }
        }
        
        public bool Validate ()
        {
            if (IsValid) {
              return true;
            } else {
              Field.IntValue = (int) ValidTime.TotalMinutes;
              return false;
            }
        }
        
        public TimeSpan ReadTimeFromSetting ()
        {
            var time = Setting;
            Field.IntValue = (int) time.TotalMinutes;
            return time;
        }
        
        public TimeSpan WriteTimeToSetting ()
        {
            return Setting = ValidTime;
        }
    }
    
    class WorkTimeTextFieldController : TimeTextFieldController
    {
        public WorkTimeTextFieldController (AbstractPlatform platform, NSTextField field) : base (platform, field, "Work time")
        {
        }
        
        public override TimeSpan Setting {
            get {
                return Platform.Preferences.MaxActivity;
            }
            set {
                Platform.Preferences.MaxActivity = value;
            }
        }
        
        public override TimeSpan MinBound { get { return 15.Minutes ().Max (Other.Setting); } }
        public override TimeSpan MaxBound { get { return 120.Minutes (); } }
        
        
        public override string ConstraintDescription {
            get {
                return string.Format ("{0} must be less than {1} minutes and greater than {2}.",
                    Name, (int)MaxBound.TotalMinutes, Other.Name.ToLower ());
            }
        }
    }
    
    class BreakTimeTextFieldController : TimeTextFieldController
    {
        public BreakTimeTextFieldController (AbstractPlatform platform, NSTextField field) : base (platform, field, "Break time")
        {
        }
        
        public override TimeSpan Setting {
            get {
                return Platform.Preferences.MinBreak;
            }
            set {
                Platform.Preferences.MinBreak = value;
            }
        }
        
        public override TimeSpan MinBound { get { return 2.Minutes (); } }
        public override TimeSpan MaxBound { get { return Other.Setting; } }
        
        public override string ConstraintDescription {
            get {
                return string.Format ("{0} must be greater than {1} minutes and less than {2}.",
                    Name, (int)MinBound.TotalMinutes, Other.Name.ToLower ());
            }
        }
    }
    
}

