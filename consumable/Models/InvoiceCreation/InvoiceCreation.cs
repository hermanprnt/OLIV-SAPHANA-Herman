using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.InvoiceCreation
{
    public class InvoiceCreation
    {
        public string PO_NUMBER { set; get; }
        public string PO_ITEM { set; get; }
        public DateTime? PO_DATE { set; get; }
        public String PO_TEXT { get; set; }

        public int TOTAL_QTY { set; get; }
        public double TOTAL_AMOUNT { set; get; }

        //public DateTime? MATDOC_DATE { set; get; }
        public string VEND_CODE { set; get; }
        public string SUPPLIER_NAME { set; get; }
        public string EDIT_AMOUNT_FLAG { set; get; }
        public string MATDOC_UNIT { set; get; }
        public string MATDOC_CURRENCY { set; get; }

        public string GR_STATUS { set; get; }

        public String UPLOAD_BY { get; set; }
        public DateTime UPLOAD_DATE { get; set; }
        public String ATTACH_FILE { get; set; }
        
        public String INVOICE_NO { get; set; }
        public String INVOICE_ID { get; set; }
        
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }
        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }
    }
}