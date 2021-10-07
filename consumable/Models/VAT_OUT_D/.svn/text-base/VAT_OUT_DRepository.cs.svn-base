using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class VAT_OUT_DRepository
    {
        private VAT_OUT_DRepository() { }
        private static VAT_OUT_DRepository instance = null;

        public static VAT_OUT_DRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_OUT_DRepository();
                }
                return instance;
            }
        }

        public IEnumerable<VAT_OUT_D> GetListDataByID(string TaxInvNo, string DebitAdvNo, string ReplacementFG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo,
                DebitAdvNo = DebitAdvNo,
                ReplacementFG = ReplacementFG
            };

            IEnumerable<VAT_OUT_D> result = db.Fetch<VAT_OUT_D>("VAT_OUT_D_GetListDataByID", args);
            db.Close();
            return result;
        }
    }
}