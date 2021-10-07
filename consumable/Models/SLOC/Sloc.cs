using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.SLOC
{
    public class Sloc
    {
        public String PLANT_CD { get; set; }
        public String SLOC_CD { get; set; }
        public String SLOC_NAME { get; set; }
        public String WAREHOUSE_CD { get; set; }
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }
        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }
    }
}