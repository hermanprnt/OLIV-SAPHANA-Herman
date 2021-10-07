using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.InvoiceReceiving
{
    public class InvoiceReceiving
    {
        public string SUPPLIER_NAME { set; get; }

      
        public DateTime? SUBMIT_DT { set; get; }
        public string SUBMIT_BY { set; get; }
        public string RECEIVED_STATUS { set; get; }
        public string NOTICE { set; get; }

        public string INVOICE_NO { set; get; }
        public DateTime? INVOICE_DATE { set; get; }
        public string S_INVOICE_DATE { get; set; }
        public string TAX_INVOICE_NO { get; set; }
        public Double TAX_INVOICE_AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public Double TURN_OVER { set; get; }
        public Double TAX_BASE { set; get; }
        public Double CALCULATE_TAX { set; get; }
        public string CHECKBOX_STAMP { set; get; }
        public Double TOTAL_AMOUNT { set; get; }
        public string STATUS { set; get; }
        public string SUPPLIER_CD { set; get; }
        public DateTime? PAYMENT_PLAN_DATE { set; get; }
        public DateTime? PAYMENT_ACTUAL_DATE { set; get; }
        public string PROGRESS_STATUS { set; get; }
        public string NOTICE_BY { set; get; }
        public string NOTICE_REMARK { set; get; }
        public DateTime? NOTICE_DATE { set; get; }
        public string CERTIFICATE_ID { set; get; }
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }
        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }

        //Helper
        public String MSG_UPDATED { get; set; }
    }
}