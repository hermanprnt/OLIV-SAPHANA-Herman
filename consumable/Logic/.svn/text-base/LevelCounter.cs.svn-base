using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Logic
{
    public class LevelCounter
    {
        public List<int> d = new List<int>();
        private int _level = 0;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;


            }
        }

        public void Up()
        {
            ++_level;
            if (_level > d.Count)
            {
                for (int i = 0; i < (_level - d.Count); i++)
                    d.Add(0);
            }
        }

        public void Down()
        {

            //if (_level < d.Count && _level > 0)
            //{
            //    for (int i = _level; i < d.Count; i++)
            //    {
            //        d[i] = 0;
            //    }
            //}
            --_level;
        }



        public void Inc(int value = 1)
        {
            int v = d[Level - 1] + value;
            d[Level - 1] = v;

            if (d.Count > Level)
            {
                for (int l = Level; l < d.Count; l++) { d[l] = 0; }
            }

        }



        public void Dec(int value = 1)
        {
            Inc(-1 * value);
        }

        public int Val
        {
            get
            {
                if (d.Count > 0)
                    return (d[d.Count - 1]);
                else
                    return -1;
            }
        }
    }
}
