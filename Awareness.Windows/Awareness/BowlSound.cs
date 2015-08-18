using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Awareness.Agnostic;

namespace Awareness
{
    class BowlSound : AbstractBowlSound
    {
        [DllImport("winmm.dll")]
        extern static int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        readonly string Alias;

        public BowlSound(string soundPath) : base (soundPath)
        {
            Alias = "Bowl" + Id;
            mciSendString("Open \"" + soundPath + "\" Alias " + Alias, "", 0, 0);
        }

        public override void Play()
        {
            mciSendString("Play " + Alias, "", 0, 0);
        }

        public override void Dispose()
        {
            mciSendString("Close " + Alias, "", 0, 0);
        }
    }
}
