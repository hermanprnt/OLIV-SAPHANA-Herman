﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.ReceivingList
{
    public class ReceivingList
    {
        public string DN_NO { set; get; }
        public string DN_NO_ITEM { set; get; }
        public string PO_NUMBER { set; get; }
        public string PO_ITEM { set; get; }
        public DateTime? PO_DATE { set; get; }
        public String PO_TEXT { get; set; }

        public int TOTAL_QTY { set; get; }
        public double TOTAL_AMOUNT { set; get; }
        
        //public DateTime? MATDOC_DATE { set; get; }
        public string VEND_CODE { set; get; }
        public string SUPPLIER_NAME { set; get; }
        //public string MATDOC_NUMBER { set; get; }
        public string MATDOC_CURRENCY { set; get; }
        public string MATDOC_UNIT { set; get; }
       
        public string GR_STATUS { set; get; }
        public string INVOICE_ID { set; get; }
    }
}