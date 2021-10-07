using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_OUT_SAP_Model
    {
        public string TAX_INVOICE_NO { get; set; }
        public string REPLACEMENT_FG { get; set; }
        public string DEBIT_ADVICE_NO { get; set; }
        public string TRANS_TYPE_CODE { get; set; }
        public string INVOICE_MONTH { get; set; }
        public string INVOICE_YEAR { get; set; }
        public DateTime? INVOICE_DATE { get; set; }
        public Decimal DPP_PRICE { get; set; }
        public Decimal VAT_PRICE { get; set; }
        public Decimal LUXURY_TAX_PRICE { get; set; }
        public string REFERENCE { get; set; }
        public decimal ADD_REMARK_ID { get; set; }
        public string BUSINESS_UNIT { get; set; }
        public string SAP_DOC_NO { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NM { get; set; }
        public Decimal UNIT_PRICE { get; set; }
        public Int64 PRODUCT_QTY { get; set; }
        public Decimal TOTAL_PRICE { get; set; }
        public Decimal DISC_PRICE { get; set; }
        public Decimal DPP_PRICE_D { get; set; }
        public Decimal VAT_PRICE_D { get; set; }
        public Decimal LUXURY_TAX_PERC_D { get; set; }
        public Decimal LUXURY_TAX_PRICE_D { get; set; }        
        public string NPWP { get; set; }
        public string NAME { get; set; }
        public string ADDRESS { get; set; }
        
    }
}