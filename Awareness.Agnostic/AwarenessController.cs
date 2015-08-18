using System;

using Cadenza;

namespace Awareness.Agnostic
{
	public class AwarenessController
	{
		readonly AbstractPlatform Platform;

		public event EventHandler FirstRun;

		public BowlPlayer BowlPlayer { get; protected set; }
		public BreakTimer BreakTimer { get; protected set; }

		public AwarenessController (AbstractPlatform platform)
		{
			Platform = platform;
			Platform.ApplicationLaunched += ApplicationLaunched;
			Platform.ApplicationWillQuit += ApplicationWillQuit;

			BreakTimer = new BreakTimer (Platform);
			BreakTimer.BreakSuggested += BreakSuggested;

			BowlPlayer = new BowlPlayer (Platform);
		}

		void BreakSuggested (object sender, EventArgs e)
		{
			BowlPlayer.Play (BreakTimer.ElapsedMaxActivityIntervals);
		}

		void ApplicationLaunched (object sender, EventArgs e)
		{
			if (Platform.Preferences.IsFirstRun) {
				Log.Info ("Awareness launched for the first time.");
				FirstRun.Raise (this);
				Platform.Preferences.IsFirstRun = false;
			} else {
				Log.Info ("Awareness launched.");
			}

            if (Platform.Preferences.PlayBowlOnStart)
			    BowlPlayer.Play (1);
			BreakTimer.Start ();
		}

		void ApplicationWillQuit (object sender, EventArgs e)
		{
			Log.Info ("Awareness will quit.");
			BowlPlayer.StopPlaying ();
			BreakTimer.Stop ();
		}
	}
}
