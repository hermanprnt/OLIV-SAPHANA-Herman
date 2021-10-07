using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.RequistionForm
{
    public class RequistionForm
    {
        public Int32 ROW_NUM { get; set; }
        public String RF_REGISTER_ID { get; set; }
        public String RF_NO { get; set; }
        public DateTime? RF_DT { get; set; }
        public String PIC_USER { get; set; }
        public String DIVISION { get; set; }
        public Double TOTAL_AMOUNT { get; set; }
        public String WBS_NO { get; set; }

        public String CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public String UPDATED_BY { get; set; }
        public DateTime? UPDATED_DT { get; set; }

        // List
        public IList<RequistionFormDtl> listRequistionFormDtl { get; set; }

        // Transient
        public String S_RF_DT { get; set; }
        public String RF_DTL_NO { get; set; }
        public String S_TOTAL_AMOUNT { get; set; }
    }
}