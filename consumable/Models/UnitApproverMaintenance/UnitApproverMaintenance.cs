using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.UnitApproverMaintenance
{
    public class UnitApproverMaintenance
    {
        public String PLANT_CD { get; set; }
        public String SLOC_CD { get; set; }
        public String PIC { get; set; }
        public String DELETION_FLAG { get; set; }
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }
        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }
        public String KEY_UPDATE_DATA { get; set; }
    }
}