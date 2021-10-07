using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class CUSTOMER
    {
        public string TAX_INVOICE_NO { get; set; }
        public string REPLACEMENT_FG { get; set; }
        public string DEBIT_ADVICE_NO { get; set; }
        public string NAME { get; set; }
        public string NPWP { get; set; }
        public string ADDRESS { get; set; }
        public DateTime CREATED_DT { get; set; }
        public DateTime? CHANGED_DT { get; set; }
        public string CREATED_BY { get; set; }
        public string CHANGED_BY { get; set; }
    }
}