using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

using Cadenza;
using Cadenza.Collections;

namespace Awareness.Agnostic
{
    public class BreakTimer
    {
        public event EventHandler BreakChecked;
        public event EventHandler BreakTaken;
        public event EventHandler BreakSuggested;

        public DateTime LastBreak { get; protected set; }

        public DateTime LastBreakSuggested { get; protected set; }

        Thread UpdateThread;
        IThreadEvent UpdateThreadWaiter;
        TimeSpan IdleTimeAccumulatedSinceLastBreak, UpdateThreadIdleTime;
        AbstractPlatform Platform;

        public BreakTimer (AbstractPlatform platform)
        {
            Platform = platform;
            Platform.SystemResumed += delegate {
                Log.Info ("System resumed, waking UpdateThread...");
                CheckNowAsync ();
            };

            LastBreak = LastBreakSuggested = platform.Clock.Now;
            IdleTimeAccumulatedSinceLastBreak = TimeSpan.Zero;
        }

        /// <summary>
        /// Causes the break checker thread to check.
        /// 
        /// Any events raised as a result of the check will be called via AbstractPlatform.RunOnMainThread, so
        /// your handlers will not fire synchronously.
        /// </summary>
        public void CheckNowAsync ()
        {
            UpdateThreadWaiter.Set ();
        }

        public void Start ()
        {
            UpdateThreadWaiter = Platform.GetThreadEvent ();
            UpdateThread = Platform.CreateWorkerThread (TimerLoop);
            UpdateThread.IsBackground = true;
            UpdateThread.Start ();
        }

        public void Stop ()
        {
            UpdateThread.Abort ();
        }

        TimeSpan TimeUntilNextCheck {
            get {
                // Time until next check is the minimum of...
                var until = new[] {
                 1.Minute (),
                 // The time until the next break suggestion is expected
                 NextBreakSuggestionExpected - Platform.Clock.Now,
                 // And the shortest possible time until a break is taken
                 // TODO: Sometimes this is longer than MinBreak.
                 Platform.Preferences.MinBreak - PlatformIdleTime + 1.Second ()
             }.Where (t => TimeSpan.Zero < t).MinBy (t => t.Ticks);
             
                // Always wait at least one second.
                return until.Max (1.Second ());
            }
        }

        TimeSpan PlatformIdleTime {
            get { return Platform.Clock.Now - Platform.LastActivity; }
        }
        
        TimeSpan MaxIdleTime {
            get { return PlatformIdleTime.Max (UpdateThreadIdleTime); }
        }

        public TimeSpan ActivityTime {
            get { return (Platform.Clock.Now - LastBreak) - IdleTimeAccumulatedSinceLastBreak; }
        }

        public int ElapsedMaxActivityIntervals {
            get { return (int)(ActivityTime.Ticks / Platform.Preferences.MaxActivity.Ticks); }
        }

        DateTime NextBreakSuggestionExpected {
            get { return LastBreakSuggested + Platform.Preferences.MaxActivity + IdleTimeAccumulatedSinceLastBreak; }
        }

        void Raise (EventHandler e)
        {
            if (e == null)
                return;
            Platform.RunOnMainThread (() => e (this, EventArgs.Empty));
        }

        void TimerLoop ()
        {
            int currentActivityInterval = 0;
         
            DateTime lastCheck = Platform.Clock.Now,
                     lastActivity = Platform.LastActivity;
         
            while (true) {
                Log.Debug ("Checking if break needed:\n\t- {0} since last break (excluding accumulated idle time)\n\t- {1} since last platform activity\n\t- {2} since last check",
                        ActivityTime.ToShortTimeString (), PlatformIdleTime.ToShortTimeString (), UpdateThreadIdleTime.ToShortTimeString ());
             
                if (Platform.Preferences.MinBreak < MaxIdleTime) {
                    // User took a break
                    Log.Info ("User took a {0} break.", MaxIdleTime.ToShortTimeString ());
                    
                    LastBreak = Platform.Clock.Now;
                    currentActivityInterval = 0;
                    IdleTimeAccumulatedSinceLastBreak = TimeSpan.Zero;
                    
                    Raise (BreakTaken);
                } else {
                    // User hasn't taken a break yet...
                 
                    // If there has been no activity since the last check, we make sure we
                    // don't count the elapsed idle time against the clock.
                    if (SecondFloor (lastActivity) == SecondFloor (Platform.LastActivity)) {
                        Log.Info ("User has been idle since last check.");

                        // Activity has not happened since the last check, count the time since
                        // the last check as accumulated idle time.
                        IdleTimeAccumulatedSinceLastBreak += UpdateThreadIdleTime;
                    } else if (Platform.Preferences.MaxActivity < ActivityTime) {
                        Log.Debug ("User needs a break.");
                     
                        // User needs a break! However, we only suggest a break every MaxActivity interval.
                        if (currentActivityInterval < ElapsedMaxActivityIntervals) {
                            Log.Info ("A full activity interval has elapsed, time to suggest a break.");
                         
                            Raise (BreakSuggested);
                         
                            currentActivityInterval = ElapsedMaxActivityIntervals;
                            LastBreakSuggested = Platform.Clock.Now;
                        } else {
                            Log.Debug ("A full activity interval has not elapsed, not suggesting a break.");
                        }
                     
                    }
                }
             
                Raise (BreakChecked);
             
                lastCheck = Platform.Clock.Now;
                lastActivity = Platform.LastActivity;
             
                if (UpdateThreadWaiter.WaitOne (TimeUntilNextCheck)) {
                    Log.Info ("UpdateThread woken by signal.");
                } else {
                    Log.Debug ("UpdateThread woke for scheduled check.");
                }
                UpdateThreadIdleTime = Platform.Clock.Now - lastCheck;
            }
        }

        DateTime SecondFloor (DateTime t)
        {
            // Round time down to the second. 
            return new DateTime ((t.Ticks / 1.Second ().Ticks) * 1.Second ().Ticks);
        }
    }
}
