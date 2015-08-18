using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Awareness.Agnostic
{
    public class MockClock : IClock
    {
        DateTime now;

        public DateTime Now
        {
            get
            {
                return now;
            }
        }

        public void Elapse(TimeSpan duration)
        {
            now += duration;
        }
    }
}
