using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class VAT_IN_DRepository
    {
        private VAT_IN_DRepository() { }
        private static VAT_IN_DRepository instance = null;

        public static VAT_IN_DRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_IN_DRepository();
                }
                return instance;
            }
        }

        public IEnumerable<VAT_IN_D> GetListDetail(string TaxInvoiceNo, string ReplacementFG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvoiceNo,
                REPLACEMENT_FG = ReplacementFG
            };
            IEnumerable<VAT_IN_D> result = db.Fetch<VAT_IN_D>("VAT_IN_D_GetListDetail", args);
            db.Close();
            return result;
        }

        public string GetLastID()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };
            string result = db.SingleOrDefault<string>("VAT_IN_D_GetLastID", args);
            db.Close();
            if(result==null)
            {
                result = "0000000000";
            }
            return result;
        }

        public bool InsertData(VAT_IN_ScanBarcode data)
        {
            bool status = false;
            IDBContext db = DatabaseManager.Instance.GetContext();
            foreach (var item in data.DetailTransaksi)
            {
                string ID = GetLastID();

                dynamic args = new
                {
                    TRANS_DETAIL_TYPE = HelperRepository.Instance.AutoIncNumID(ID),
                    REPLACEMENT_FG = item.fgPengganti,
                    TAX_INVOICE_NO = item.noFaktur,
                    //PRODUCT_CODE = null,
                    PRODUCT_NM = item.Nama,
                    UNIT_PRICE = item.HargaSatuan,
                    PRODUCT_QTY = item.JmlBarang,
                    TOTAL_PRICE = item.HargaTotal,
                    DISC_PRICE = item.Diskon,
                    DPP_PRICE = item.DPP,
                    VAT_PRICE = item.PPN,
                    LUXURY_TAX_PERC = item.TarifPPnBM,
                    LUXURY_TAX_PRICE = item.PPnBM,
                    CREATED_DT = DateTime.Now,
                    CHANGED_DT = DateTime.Now,
                    CREATED_BY = "TMMIN - CASHIER",
                    CHANGED_BY = "TMMIN - CASHIER"
                };
                try
                {
                    int result = db.Execute("VAT_IN_D_InsertData", args);
                }
                catch (Exception)
                {
                    db.Close();
                    status = false;
                    return status;
                }
                

            }

            status = true;
            db.Close();
            return status;
        }
    }
}