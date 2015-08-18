using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awareness.Agnostic
{
    public interface IThreadEvent
    {
        bool WaitOne(TimeSpan timeout);
        bool Set();
    }
}
