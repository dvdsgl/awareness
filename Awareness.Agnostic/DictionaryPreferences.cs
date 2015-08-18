using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awareness.Agnostic
{
    public class DictionaryPreferences : AbstractPreferences
    {
        Dictionary<string, bool> Bools = new Dictionary<string, bool>();
        Dictionary<string, int> Ints = new Dictionary<string, int>();

        protected override int GetInt(string key, int dfault)
        {
            return Ints.ContainsKey(key) ? Ints[key] : dfault;
        }

        protected override void SetInt(string key, int value)
        {
            Ints[key] = value;
            RaiseChange (key);
        }

        protected override bool GetBool(string key, bool dfault)
        {
            return Bools.ContainsKey(key) ? Bools[key] : dfault;
        }

        protected override void SetBool(string key, bool value)
        {
            Bools[key] = value;
            RaiseChange (key);
        }
    }
}
