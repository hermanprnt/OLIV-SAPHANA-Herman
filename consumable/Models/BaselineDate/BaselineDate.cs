using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.BaselineDate
{
    public class BaselineDate
    {        
        public DateTime? HOLIDAY_DATE { get; set; }
        public String HOLIDAY_DATE_OLD_STR { get; set; }
        public String HOLIDAY_DATE_STR { get; set; }
        public DateTime? BASELINE_DATE { get; set; }
        public String BASELINE_DATE_STR { get; set; }

        public String CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public String CREATED_DT_STR { get; set; }

        public String UPDATED_BY { get; set; }
        public DateTime? UPDATED_DT { get; set; }
        public String UPDATED_DT_STR { get; set; }
    }
}