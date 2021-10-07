using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace consumable.Common.Data
{
    [Description("TOT : ZHRTPIST00009 for ZHRTFM_UPD_OT_DATA, MIOS_UPD_OVERTIME")]
    public class TOvertime
    {
        [Description("Personnell number")]
        public string PERNR { get; set; } 

        [Description("Complete Name ")]
        public string CNAME { get; set; } 

        [Description("Category")]
        public string CAT { get; set; } 

        [Description("Attendace Quota type")]
        public string KTART { get; set; } 

        [Description("Attendace Quota type text")]
        public string KTEXT { get; set; } 

        [Description("Date Start")]
        public string BEGDA { get; set; } 
        [Description("Date End")]
        public string ENDDA { get; set; } 

        [Description("Time Start")]
        public string BEGUZ { get; set; } 
        [Description("Time End")]
        public string ENDUZ { get; set; } 
        
        [Description("Deletion Flag")]
        public string DELET { get; set; }
        [Description("Message")]
        public string MESSAGE { get; set; } 
    }
}
