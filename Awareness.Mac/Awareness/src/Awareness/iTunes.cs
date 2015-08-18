using System;
using System.Diagnostics;

using Awareness.Agnostic;

namespace Awareness
{
	public class iTunes
	{
		readonly string iTunesGoQuietScript;
		readonly AbstractPlatform Platform;
		
		public iTunes (AbstractPlatform platform)
		{
			Platform = platform;
			iTunesGoQuietScript = Platform.ResourceNamed ("iTunesGoQuiet.as");
		}
		
		/// <summary>
        /// If iTunes is running, this will use an AppleScript to reduce iTunes playback volume for
        /// a short period so the user can hear the bowls.
        /// </summary>
        /// <param name="seconds">
        /// A <see cref="System.Int32"/> with the number of seconds iTunes should lower volume for.
        /// </param>
        public void GoQuiet (int seconds)
        {
            var args = string.Format ("{0} {1}", iTunesGoQuietScript, seconds);
            Process.Start ("osascript", args);
        }
		
		public void GoQuiet (TimeSpan duration)
		{
			GoQuiet ((int) duration.TotalSeconds);
		}
	}
}

