using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.InvoiceCreation
{
    public class Invoice
    {
        public string INVOICE_NO { set; get; }
        public DateTime? INVOICE_DATE { set; get; }
        public string S_INVOICE_DATE { get; set; }
        public string TAX_INVOICE_NO { get; set; }
        public string CURRENCY { get; set; }
        public Double TURN_OVER { set; get; }
        public Double TAX_BASE { set; get; }
        public Double CALCULATE_TAX { set; get; }
        public string CHECKBOX_STAMP { set; get; }
        public Double STAMP_AMOUNT { set; get; }
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
        public Double TAX_INVOICE_AMOUNT { set; get; }
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }
        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }
        public string PO_CATEGORY { get; set; }//20200518
        public string WITHOLDING_TAX_CD { get; set; }//20200605
        public string NOTICE_FLAG { set; get; }
        public string NUMBER { set; get; }
        public string SUPPLIER_NAME { set; get; }
        public string INVOICE_ID { set; get; }

        public IList<InvoiceAttachment> InvoiceAttachmentList { get; set; }
    }
}