using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace Awareness.Agnostic
{
    public class RealThreadEvent : IThreadEvent
    {
        readonly AutoResetEvent Event = new AutoResetEvent(false);

        public bool WaitOne(TimeSpan timeout)
        {
            return Event.WaitOne(timeout);
        }

        public bool Set()
        {
            return Event.Set();
        }
    }
}
