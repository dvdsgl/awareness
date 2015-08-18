using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using Awareness.Agnostic;

namespace Awareness
{
    class WindowsPreferences : AbstractPreferences
    {
        T Get<T>(string key, T dfault)
        {
            var value = Settings.Default[key];
			return value == null ? dfault : (T) value;
        }

        void Set<T>(string key, T val)
		{
			Settings.Default [key] = val;
			Settings.Default.Save ();
        }

        protected override int GetInt(string key, int dfault)
        {
            return Get(key, dfault);
        }

        protected override void SetInt(string key, int value)
        {
            Set(key, value);
        }

        protected override bool GetBool(string key, bool dfault)
        {
			return Get(key, dfault);
        }

        protected override void SetBool(string key, bool value)
        {
            Set(key, value);
        }
    }
}
