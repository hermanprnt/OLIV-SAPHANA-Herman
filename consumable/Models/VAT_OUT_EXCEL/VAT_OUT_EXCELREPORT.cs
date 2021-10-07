using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_OUT_EXCELREPORT
    {
        public IEnumerable<VAT_OUT_H> VAT_OUT { get; set; }
        public decimal TotalDPP { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal TotalPPnBM { get; set; }
    }
}