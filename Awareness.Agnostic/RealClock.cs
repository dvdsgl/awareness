using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awareness.Agnostic
{
    public class RealClock : IClock
    {
        public DateTime Now {
            get { return DateTime.Now; }
        }

        public void ThreadSleep(TimeSpan duration)
        {
            Thread.Sleep(duration);
        }
    }
}
