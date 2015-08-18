using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace Awareness.Agnostic
{
    public class MockThreadEvent : IThreadEvent
    {
        bool IsSet;
        MockClock Clock;

        public MockThreadEvent(MockClock clock)
        {
            Clock = clock;
        }

        public bool WaitOne(TimeSpan timeout)
        {
            IsSet = false;
            var waitUntil = Clock.Now + timeout;

			// TODO: Find alternative to Thread.Yield on .NET 3.5
            //while (Clock.Now < waitUntil)
            //    Thread.Yield();

            if (IsSet)
            {
                return true;
            }
            else
            {
                Clock.Elapse(timeout);
                return false;
            }
        }

        public bool Set()
        {
            IsSet = true;
            return true;
        }
    }
}
