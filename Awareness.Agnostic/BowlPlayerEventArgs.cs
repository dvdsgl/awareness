using System;

namespace Awareness.Agnostic
{
	public class BowlPlayerEventArgs : EventArgs
	{
		public readonly int TimesPlayed;
		
		public BowlPlayerEventArgs (int timesPlayed)
		{
			TimesPlayed = timesPlayed;
		}
	}
}

