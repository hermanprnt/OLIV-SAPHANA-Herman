using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.WithholdingTax
{
    public class WithholdingTaxRepository
    {
        private WithholdingTaxRepository() { }
        private static WithholdingTaxRepository instance = null;

        public static WithholdingTaxRepository Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new WithholdingTaxRepository();
                }
                return instance;
            }
        }

        public List<WithholdingTax> GetWitholdingTax(string name, int fromnumber, int tonumber)
        {
            List<WithholdingTax> result = new List<WithholdingTax>();
            WithholdingTax item = null;

            for (int count = 1; count <= 5; count++)
            {
                item = new WithholdingTax();
                item.WitholdingTaxCode = "F"+count;
                item.WitholdingTaxName = "Payable PPh Final " + count + "%";                
               
                result.Add(item);
            }

            return result;
        }
    }
}