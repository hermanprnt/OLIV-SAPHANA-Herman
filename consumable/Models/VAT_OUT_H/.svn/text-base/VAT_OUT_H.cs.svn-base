using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_OUT_H
    {
        public string TAX_INVOICE_NO { get; set; }
        public string REPLACEMENT_FG { get; set; }
        public string DEBIT_ADVICE_NO { get; set; }
        public string TRANS_TYPE_CODE { get; set; }
        public string TAX_INVOICE_MONTH { get; set; }
        public string TAX_INVOICE_YEAR { get; set; }
        public DateTime TAX_INVOICE_DATE { get; set; }
        public decimal DPP_PRICE { get; set; }
        public decimal VAT_PRICE { get; set; }
        public decimal LUXURY_TAX_PRICE { get; set; }
        public decimal DOWN_PAYMENT_FG { get; set; }
        public decimal DOWN_PAYMENT_DPP { get; set; }
        public decimal DOWN_PAYMENT_VAT { get; set; }
        public decimal DOWN_PAYMENT_LT { get; set; }
        public string REFERENCE { get; set; }
        public int ADD_REMARK_ID { get; set; }
        public string BUSINESS_UNIT { get; set; }
        public string STATUS { get; set; }
        public DateTime? INTERFACE_DT { get; set; }
        public DateTime? DOWNLOAD_DT { get; set; }
        public DateTime? COMPLETE_DT { get; set; }
        public DateTime CREATED_DT { get; set; }
        public DateTime? CHANGED_DT { get; set; }
        public string CREATED_BY { get; set; }
        public string CHANGED_BY { get; set; }
        public string SAP_DOC_NO { get; set; }
        public string NAME { get; set; }
        public string NPWP { get; set; }
        //public CUSTOMER CUSTOMER { get; set; }
        public SystemProperty STATUS_NAME
        {
            get
            {
                SystemProperty result = SystemRepository.Instance.GetVAT_OUT_STS(STATUS);
                return result;
            }
        }
        public CUSTOMER CUSTOMER
        {
            get
            {
                CUSTOMER result = CUSTOMERRepository.Instance.GetDataByID(TAX_INVOICE_NO, REPLACEMENT_FG, DEBIT_ADVICE_NO);
                return result;
            }
        }
    }
}