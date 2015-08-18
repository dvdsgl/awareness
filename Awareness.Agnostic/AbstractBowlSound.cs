using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cadenza;

namespace Awareness.Agnostic
{
	public abstract class AbstractBowlSound : IDisposable
	{
		static int Bowls;
		
		public static readonly TimeSpan SoundDuration = 14.Seconds ();
		
		protected readonly int Id;
		protected readonly string SoundPath;

		public AbstractBowlSound (string soundPath)
		{
			Id = Bowls++;
			SoundPath = soundPath;	
		}

		public abstract void Play (float volume = 100);

		public virtual void Dispose ()
		{
		}
	}
}
