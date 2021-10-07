﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.InvoiceInquiry
{
    public class InvoiceInquiryDetail
    {
        public Int32 ROW_NUM { get; set; }
        public String PO_NUMBER { get; set; }
        public String PO_ITEM { get; set; }
        public String PO_TEXT { get; set; }
        public String MATDOC_NUMBER { get; set; }
        public String MATDOC_ITEM { get; set; }
        public String MATDOC_YEAR { get; set; }
        public String HEADER_TEXT { get; set; }
        public DateTime? MATDOC_DATE { get; set; }
        public String MATERIAL_DESCRIPTION { get; set; }
        public String MATDOC_CURRENCY { get; set; }

        public String MATDOC_UNIT { get; set; }
        public String MAT_NUMBER { get; set; }
        public String MATDOC_QTY { get; set; }
        public Double MATDOC_AMOUNT { get; set; }


        //For Posting Only
        public Double PO_TOTAL_AMOUNT { get; set; }
        public Double CALCULATE_TAX { get; set; }
        public Double STAMP_AMOUNT { get; set; }
        public Double TAX_INVOICE_AMOUNT { get; set; }

    }

    public class InvoiceErrorDetail {
        public String INVOICE_ID { get; set; }
        public String INVOICE_NO { get; set; }
        public String POSTED_DT_STR { get; set; }
        public String MESSAGE { get; set; }
    
    }
}