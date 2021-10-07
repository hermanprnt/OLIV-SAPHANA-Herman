using System;
using System.Collections.Generic;

namespace consumable.Models.RequistionForm
{
    public class RequistionFormDtl
    {
        public Int32 ROW_NUM { get; set; }
        public String RF_REGISTER_DTL_ID { get; set; }
        public String RF_REGISTER_ID { get; set; }
        public String RF_DTL_NO { get; set; }
        public String DESCRIPTION { get; set; }
        public Double AMOUNT { get; set; }
        public String PR_NO { get; set; }
        public String PR_CREATOR { get; set; }
        public DateTime? PR_CREATED_DT { get; set; }
        public String PO_NO { get; set; }
        public DateTime? PO_CREATED_DT { get; set; }

        public String CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public String UPDATED_BY { get; set; }
        public DateTime? UPDATED_DT { get; set; }

        // Transient
        public String S_AMOUNT { get; set; }
    }
}