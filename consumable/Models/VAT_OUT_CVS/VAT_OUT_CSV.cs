using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_OUT_CSV
    {
        public string TRANS_TYPE_CODE { get; set; }
        public string REPLACEMENT_FG { get; set; }
        public string TAX_INVOICE_NO { get; set; }
        public string DEBIT_ADVICE_NO { get; set; }
        public string TAX_INVOICE_MONTH { get; set; }
        public string TAX_INVOICE_YEAR { get; set; }
        public DateTime TAX_INVOICE_DATE { get; set; }
        public string NPWP { get; set; }
        public string NAME { get; set; }
        public string ADDRESS { get; set; }
        public decimal DPP_PRICE { get; set; }
        public decimal VAT_PRICE { get; set; }
        public decimal LUXURY_TAX_PRICE { get; set; }
        public decimal ADD_REMARK_ID { get; set; }
        public decimal DOWN_PAYMENT_FG { get; set; }
        public decimal DOWN_PAYMENT_DPP { get; set; }
        public decimal DOWN_PAYMENT_VAT { get; set; }
        public decimal DOWN_PAYMENT_LT { get; set; }
        public string REFERENCE { get; set; }
        public IEnumerable<VAT_OUT_D> VAT_OUT_DETAIL
        {
            get
            {
                var result = VAT_OUT_DRepository.Instance.GetListDataByID(TAX_INVOICE_NO, DEBIT_ADVICE_NO, REPLACEMENT_FG);
                return result;
            }
        }
    }
}