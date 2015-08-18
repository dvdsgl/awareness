using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

namespace Awareness.Agnostic
{
    public class MockPlatform : AbstractPlatform
    {
        DateTime lastActivity;

        readonly Action<Action> Invoke;
        readonly MockClock MockClock = new MockClock ();

        public override event EventHandler ApplicationLaunched, ApplicationWillQuit, SystemResumed;

        public override IClock Clock {
            get { return MockClock; }
        }

        public override DateTime LastActivity {
            get {
                return lastActivity;
            }
        }

        protected override string ResourcesDirectory
        {
            get { throw new NotImplementedException(); }
        }

        public MockPlatform() : this (act => act())
        {
        }

        public MockPlatform(Action<Action> invoke) : base (new DictionaryPreferences ())
        {
            Invoke = invoke;
        }

        public override void RunOnMainThread(Action act)
        {
            Invoke (act);
        }

        public override void ThreadSleep(TimeSpan duration)
        {
            // Actually sleep for a bit, just for manual testing.
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(duration.TotalMinutes));

            MockClock.Elapse (duration);
        }

        public void ObserveActivity ()
        {
            lastActivity = MockClock.Now;
        }

        public void ObserveIdleness (TimeSpan duration)
        {
            MockClock.Elapse(duration);
        }

        public override IThreadEvent GetThreadEvent()
        {
            return new MockThreadEvent(MockClock);
        }
		
		public override Thread CreateWorkerThread (Action act)
		{
			return new Thread (() => act ());
		}

        public override AbstractBowlSound CreateBowlSound(string soundPath)
        {
            throw new NotImplementedException();
        }

		public override void OpenUrl (string url)
		{
			throw new NotImplementedException ();
		}
    }
}
