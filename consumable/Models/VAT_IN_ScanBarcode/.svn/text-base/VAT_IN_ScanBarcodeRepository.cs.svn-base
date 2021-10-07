using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class VAT_IN_ScanBarcodeRepository
    {

        private VAT_IN_ScanBarcodeRepository() { }
        
        #region Singleton
        private static VAT_IN_ScanBarcodeRepository instance = null;
        public static VAT_IN_ScanBarcodeRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_IN_ScanBarcodeRepository();
                }
                return instance;
            }
        }
        #endregion

        public bool SaveData(VAT_IN_ScanBarcode data, string suplierCode, string uploadBy)
        {
            bool status = false;
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic argsTbRVatInH = new
            {
                TRANS_TYPE_CODE = data.kdJenisTransaksi,
                REPLACEMENT_FG = data.fgPengganti,
                SAP_TAX_INVOICE_NO = data.noFaktur.Substring(data.noFaktur.Length - 8, 8),
                TAX_INVOICE_NO = data.noFaktur,
                TAX_INVOICE_MONTH = data.TanggalFaktur.Month.ToString(),
                TAX_INVOICE_YEAR = data.TanggalFaktur.Year.ToString(),
                TAX_INVOICE_DT = data.TanggalFaktur,
                SUPPLIER_CODE = suplierCode,
                DPP_PRICE = data.JmlDPP,
                VAT_PRICE = data.JmlPPN,
                LUXURY_TAX_PRICE = data.JmlPPnBM,
                STATUS = "1",
                INV_STATUS = "1",
                IS_CREDITABLE = "1",
                //SCAN_DT = DateTime.Now,
                //DOWNLOAD_DT = DateTime.Now,
                SCAN_BY = uploadBy,
                QR_CODE_PATH = data.QRPath,
                //CREATED_DT = DateTime.Now,
                //CHANGED_DT = DateTime.Now,
                IS_USED = "0",
                CREATED_BY = uploadBy
            };

            try
            {
                int result = db.Execute("InsertTbRVatInH", argsTbRVatInH);

                int i = 0;
                foreach (detailTransaksi vatInBarcodeDtlTransaksi in data.DetailTransaksi)
                {
                    dynamic argsTbRVatInD = new
                    {
                        REPLACEMENT_FG = data.fgPengganti,
                        TAX_INVOICE_NO = data.noFaktur,
                        PRODUCT_NM = vatInBarcodeDtlTransaksi.Nama,
                        UNIT_PRICE = vatInBarcodeDtlTransaksi.HargaSatuan,
                        PRODUCT_QTY = vatInBarcodeDtlTransaksi.JmlBarang,
                        TOTAL_PRICE = vatInBarcodeDtlTransaksi.HargaTotal,
                        DISC_PRICE = vatInBarcodeDtlTransaksi.Diskon,
                        DPP_PRICE = vatInBarcodeDtlTransaksi.DPP,
                        VAT_PRICE = vatInBarcodeDtlTransaksi.PPN,
                        LUXURY_TAX_PERC = vatInBarcodeDtlTransaksi.TarifPPnBM,
                        LUXURY_TAX_PRICE = vatInBarcodeDtlTransaksi.PPnBM,
                        CREATED_BY = uploadBy
                    };

                    db.Execute("InsertTbRVatInD", argsTbRVatInD);

                    i++;
                }

                db.Close();
                status = true;
                return status;
            }
            catch (Exception)
            {
                db.Close();
                status = false;
                return status;
            }

        }

        
    }
}