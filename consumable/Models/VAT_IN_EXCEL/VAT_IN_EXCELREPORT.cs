using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_IN_EXCELREPORT
    {
        public IEnumerable<consumable.Models.VAT_IN_H.VAT_IN_H> VAT_IN { get; set; }
        public decimal TotalDPP { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal TotalPPnBM { get; set; }
    }
}