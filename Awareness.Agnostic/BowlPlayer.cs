using System;
using System.IO;
using System.Media;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cadenza;
using Cadenza.Collections;

namespace Awareness.Agnostic
{
	public class BowlPlayer
	{
		public static readonly TimeSpan TimeBetweenBowls = 3.Seconds ();

		public event EventHandler<BowlPlayerEventArgs> WillPlay, StartedPlaying, StoppedPlaying;

		readonly AbstractPlatform Platform;
		readonly string BowlSoundPath;

		volatile Thread PlayThread;

		public bool IsPlaying {
			get {
				return PlayThread != null && PlayThread.IsAlive;
			}
		}

		public BowlPlayer (AbstractPlatform platform)
		{
			Platform = platform;
			BowlSoundPath = Platform.ResourceNamed ("bowl.wav");
		}

		public void StopPlaying ()
		{
			// Kill the bowls if they are sounding.
			if (PlayThread != null && PlayThread.IsAlive) {
				PlayThread.Abort ();
				StoppedPlaying.Raise (this, new BowlPlayerEventArgs (0));
			}
		}

		public void Play (int times = 1)
		{
			WillPlay.Raise (this, new BowlPlayerEventArgs (times));

			PlayThread = Platform.CreateWorkerThread (() => ThreadPlay (times));
			PlayThread.Start ();
		}

		void ThreadPlay (int times = 1)
		{
			var args = new BowlPlayerEventArgs (times);
			var bowls = times.Times ().Select (i => Platform.CreateBowlSound (BowlSoundPath)).ToArray ();

			Platform.RunOnMainThread (() => StartedPlaying.Raise (this, args));

			foreach (var bowl in bowls) {
				bowl.Play (Platform.Preferences.BowlVolumePercentage);
				Platform.ThreadSleep (TimeBetweenBowls);
			}

			Platform.ThreadSleep (AbstractBowlSound.SoundDuration);

			foreach (var bowl in bowls)
				bowl.Dispose ();

			Platform.RunOnMainThread (() => StoppedPlaying.Raise (this, args));
		}
	}

}
