using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class VAT_OUT_CSVRepository
    {
        private VAT_OUT_CSVRepository() { }
        private static VAT_OUT_CSVRepository instance = null;

        public static VAT_OUT_CSVRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_OUT_CSVRepository();
                }
                return instance;
            }
        }

        public VAT_OUT_CSV GetListData(String TaxInvNo, String ReplacementFG, String DebitAdvNo)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo,
                ReplacementFG = ReplacementFG,
                DebitAdvNo = DebitAdvNo
            };

            VAT_OUT_CSV result = db.SingleOrDefault<VAT_OUT_CSV>("VAT_OUT_CSVGetList", args);
            db.Close();
            return result;
        }
    }
}