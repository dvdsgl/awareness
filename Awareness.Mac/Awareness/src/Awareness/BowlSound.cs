using System;
using System.Linq;
using System.Diagnostics;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using Cadenza;

using Awareness.Agnostic;

namespace Awareness
{
    public class BowlSound : AbstractBowlSound
	{
		NSSound Sound;
		
		public BowlSound (string path) : base (path)
		{
			Sound = new NSSound (SoundPath, false);
		}
        
        public override void Play (float volume = 100)
        {
            Sound.Volume = volume / 100;
			Sound.Play ();
        }     
    }
}