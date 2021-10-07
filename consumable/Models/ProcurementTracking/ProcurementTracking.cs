using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.ProcurementTracking
{
    public class ProcurementTracking
    {
        public Int32 ROW_NUM { get; set; }
      
        public String VERIFICATION_STATUS { get; set; }
        public String TRANS_TYPE { get; set; }

        public String PR_NO { get; set; }
        public DateTime? PR_CREATED_DT { get; set; }
        public String PR_CREATED_BY { get; set; }        

        public String PO_NO { get; set; }       
        public DateTime? PO_CREATED_DT { get; set; }
        public String PO_CREATED_BY { get; set; }

        public String GR_NO { get; set; }
        public DateTime? GR_CREATED_DT { get; set; }
        public String GR_CREATED_BY { get; set; }        

        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }

        public String INVOICE_NO { get; set; }
        public String VENDOR_CODE { get; set; }
        public String VENDOR_NAME { get; set; }
        public String AP_DOC_NO { get; set; }  //alias AP_DOC_NO alias IR_DOC_NO 
        public Double INVOICE_AMOUNT { get; set; }
        public String TAX_INVOICE_NO { get; set; }
        public DateTime? TAX_INVOICE_DT { get; set; }
        public DateTime? INV_DOC_DATE { get; set; }
        public DateTime? RECEIVED_DATE { get; set; }
        public DateTime? INV_VERIFICATION_PLAN_DATE { get; set; }
        public DateTime? INV_VERIFICATION_ACTUAL_DATE { get; set; }
        public String INV_VERIFICATION_BY { get; set; }
        public DateTime? INV_PAYMENT_ACTUAL_DATE { get; set; }
        public DateTime? INV_PAYMENT_PLAN_DATE { get; set; }
        //public String INV_PAYMENT_PLAN_DATE { get; set; }
        public String PAYMENT_STATUS { get; set; }
        public String CLEARING_NO { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public DateTime? NOTICE_DATE { get; set; }
        public String NOTICE_BY { get; set; }
        public String REMARKS { get; set; }

        public String SOURCE_DATA { get; set; }

    }
}