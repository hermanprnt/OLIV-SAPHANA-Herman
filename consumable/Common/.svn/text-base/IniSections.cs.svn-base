using System;
using System.Collections.Generic;

namespace consumable.Common
{
    [Serializable]
    public class IniSections
    {
        public Dictionary<string, string> kv = null;

        public IniSections(Dictionary<string, string> dict)
        {
            kv = dict;
        }

        public string this[string Index]
        {
            get
            {
                if (kv.ContainsKey(Index))
                    return kv[Index];
                else
                    return null;
            }

            set
            {
                if (kv.ContainsKey(Index))
                {
                    kv[Index] = value;
                }
                else
                {
                    kv.Add(Index, value);
                }
            }
        }
    }

}