using System;

namespace Awareness.Agnostic
{
    public class PreferencesEventArgs : EventArgs
    {
        public readonly string Key;
        
        public PreferencesEventArgs (string key)
        {
            Key = key;
        }
    }
}

