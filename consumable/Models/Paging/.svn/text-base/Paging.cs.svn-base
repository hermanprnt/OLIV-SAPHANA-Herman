using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.Paging
{
    public class Paging
    {
        int mxpg = 3;
        public int CountData { get; set; }
        public int StartData { get; set; }
        public int EndData { get; set; }
        public int PositionPage { get; set; }
        public int SizePage { get; set; }
        public double CountPage { get; set; }
        public int First { get; set; }
        public int Last { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }
        public int MaxPage { get; set; }
        public int MinPage { get; set; }
        public Paging(int count, int page, int size)
        {
            EndData = page * size;
            CountData = count;
            PositionPage = page;
            SizePage = size;
            StartData = (page - 1) * size + 1;
            CountPage = Math.Ceiling((double)count / size);
            First = 1;
            Last = (int)CountPage;
            Next = page < (int)CountPage ? page + 1 : (int)CountPage;
            Prev = page == 1 ? 1 : page - 1;
            MaxPage = page + mxpg;
            MinPage = page - mxpg;
        }
    }
}