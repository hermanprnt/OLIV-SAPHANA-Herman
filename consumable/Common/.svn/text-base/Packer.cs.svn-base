using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common
{
    public class Packer : IPut
    {
        public List<IPut> putters = new List<IPut>();

        public void Add(IPut o)
        {
            putters.Add(o);
        }

        public int Put(string _what, string _user = "", string _where = "", int ID = 0, string _func = "", string _remarks = null, int _tag = 0, string _id = "", string _sts = null)
        {
            int r = 0;
            for (int i = 0; i < putters.Count; i++)
            {
                r = putters[i].Put(_what, _user, _where, ID, _func, _remarks, _tag, _id, _sts);
            }
            return r;
        }

        public void Dispose()
        {
            for (int i = putters.Count -1; i > 0; i--)
                putters[i].Dispose();
            putters.Clear();            
        }
    }


    
}
