using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

using Cadenza;

namespace Awareness.Agnostic
{
	
    public abstract class AbstractPlatform
    {
        public abstract IClock Clock { get; }
        public abstract DateTime LastActivity { get; }

        protected abstract String ResourcesDirectory { get; }

        public readonly AbstractPreferences Preferences;

        public abstract event EventHandler ApplicationLaunched, ApplicationWillQuit, SystemResumed;

        public AbstractPlatform(AbstractPreferences preferences)
        {
            Preferences = preferences;
        }

        public abstract void ThreadSleep(TimeSpan duration);
        public abstract void RunOnMainThread(Action act);

        public string ResourceNamed(string name)
        {
            var path = Path.Combine(ResourcesDirectory, name);

            if (!File.Exists(path))
                throw new FileNotFoundException("Resource not found: " + path);

            return path;
        }
		
		public abstract Thread CreateWorkerThread (Action act);

        public abstract IThreadEvent GetThreadEvent();

        public abstract AbstractBowlSound CreateBowlSound(string soundPath);

		public abstract void OpenUrl (string url);
    }
}
