using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.SUPPLIER
{
    public class Supplier
    {
        public string TAX_INVOICE_NO { get; set; }
        public string REPLACEMENT_FG { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string NAME { get; set; }
        public string NPWP { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public string CREATED_BY { get; set; }

        public DateTime? UPDATED_DT { get; set; }        
        public string UPDATED_BY { get; set; }

        public string SUPPLIER_ID { get; set; }
        public string SUPPLIER_CD { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string PKP_FLAG { get; set; }
        public Double PPN_RATE { get; set; }
        public string S_PPN_RATE { get; set; }
        public string EDIT_AMOUNT_FLAG { get; set; }

        public string PAY_METHOD { get; set; }
        public string TERM_PAY { get; set; }
        public string PARTNER_BANK { get; set; }
        public string EMAIL { get; set; }
        //public string TAX_CD { get; set; }

        public string PARTNER_BANK_TYPE { get; set; }
        public string PARTNER_BANK_NAME { get; set; }
        public string PARTNER_BANK_CODE { get; set; }
        public string CURRENCY { get; set; }



        //temp
        public string PAY_METHOD_NAME { get; set; }
        public string TERM_PAY_NAME { get; set; }

        //helper
        public string WITHOLDING_CODE { get; set; }
        public string AUTO_POSTING_FLAG { get; set; }
        public string GL_ACCOUNT_ID { get; set; }
        public string CODE { get; set; }

        //20200518
        public string SKB_FLAG { get; set; }
        public string PLAT_KUNING_FLAG { get; set; }
        public DateTime? SKB_VALID_FROM { get; set; }
        public DateTime? SKB_VALID_TO { get; set; }

        //20200706
        public string SKB_VALID_FROM_STR { get; set; }
        public string SKB_VALID_TO_STR { get; set; }
    }
}