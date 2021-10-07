using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class CUSTOMERRepository
    {
        private CUSTOMERRepository() { }
        private static CUSTOMERRepository instance = null;

        public static CUSTOMERRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CUSTOMERRepository();
                }
                return instance;
            }
        }

        public CUSTOMER GetDataByID(string TAX_INVOICE_NO, string REPLACEMENT_FG, string DEBIT_ADVICE_NO)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TAX_INVOICE_NO,
                REPLACEMENT_FG = REPLACEMENT_FG,
                DEBIT_ADVICE_NO = DEBIT_ADVICE_NO,
                //CustomerName = CustomerName,
                //CustomerNPWP = CustomerNPWP
            };

            CUSTOMER result = db.SingleOrDefault<CUSTOMER>("CUSTOMER_GetDataByID",args);
            db.Close();
            return result;
        }
    }
}