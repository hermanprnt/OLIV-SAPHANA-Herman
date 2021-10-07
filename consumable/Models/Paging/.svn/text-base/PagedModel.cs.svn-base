using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class PagedModel
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public int Count { get; set; }

        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get; set; }
        public List<int> IndexList { get; set; }

        public void Validate()
        {
            if (Page < 1)
                Page = 1;
            if (Rows < 1 || Rows > 100)
               Rows = 10;
        }
    }
}