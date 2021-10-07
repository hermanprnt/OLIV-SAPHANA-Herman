using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.VAT_IN_H
{
    public class VAT_IN_HRepository
    {

        private VAT_IN_HRepository() { }
        
        #region Singleton
        private static VAT_IN_HRepository instance = null;
        public static VAT_IN_HRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_IN_HRepository();
                }
                return instance;
            }
        }
        #endregion

        public IEnumerable<VAT_IN_H> GetVatIn()
        {
            List<VAT_IN_H> result = new List<VAT_IN_H>();
            VAT_IN_H item = null;
            Random numGen = new Random();

            for (int count = 1; count <= 5; count++)
            {
                item = new VAT_IN_H();
                item.ROW_NUM = count;
                item.TAX_INVOICE_NO = "41501491" + numGen.Next(10, 99);
                item.DPP_PRICE = 100000000;
                item.VAT_PRICE = 200000000;
                item.LUXURY_TAX_PRICE = 300000000;
                item.TAX_INVOICE_DT = DateTime.Now;
                item.SAP_DOC_NO = "DOC-NO-" + numGen.Next(1, 99);
                item.SAP_POST_DT = DateTime.Now;
                item.SCAN_DT = DateTime.Now;
                item.DOWNLOAD_DT = DateTime.Now;
                item.SCAN_BY = "Dummy User";
                item.CREATED_BY = "Dummy User";
                item.CREATED_DT = DateTime.Now;

                if (count == 5)
                {
                    item.CHANGED_BY = "Dummy User";
                    item.CHANGED_DT = DateTime.Now;
                }

                result.Add(item);
            }

            return result;
        }

        public IEnumerable<VAT_IN_H> GetAllData(DateTime? TaxInvoiceDtFrm, DateTime? TaxInvoiceDtTo, string TaxInvoiceNoFrm,
            string TaxInvoiceNoTo, DateTime? SapPostDtFrm, DateTime? SapPostDtTo, string SapDocNoFrm, string SapDocNoTo,
            DateTime? ScanDtFrm, DateTime? ScanDtTo, string Name, DateTime? DLDtFrm, DateTime? DLDtTo, string NPWP,
            string Status)
        {
            if (TaxInvoiceNoFrm == "")
            {
                TaxInvoiceNoFrm = null;
            }
            if (TaxInvoiceNoTo == "")
            {
                TaxInvoiceNoTo = null;
            }
            if (SapDocNoFrm == "")
            {
                SapDocNoFrm = null;
            }
            if (SapDocNoTo == "")
            {
                SapDocNoTo = null;
            }
            if(Status == "")
            {
                Status = null;
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_DT_FROM = TaxInvoiceDtFrm,
                TAX_INVOICE_DT_TO = TaxInvoiceDtTo,
                TAX_INVOICE_NO_FROM = TaxInvoiceNoFrm,
                TAX_INVOICE_NO_TO = TaxInvoiceNoTo,
                SAP_POST_DT_FROM = SapPostDtFrm,
                SAP_POST_DT_TO = SapPostDtTo,
                SAP_DOC_NO_FROM = SapDocNoFrm,
                SAP_DOC_NO_TO = SapDocNoTo,
                SCAN_DT_FROM = ScanDtFrm,
                SCAN_DT_TO = ScanDtTo,
                DOWNLOAD_DT_FROM = DLDtFrm,
                DOWNLOAD_DT_TO = DLDtTo,
                SUPPLIER_NAME = Name,
                SUPPLIER_NPWP = NPWP,
                VAT_IN_STATUS = Status
            };

            IEnumerable<VAT_IN_H> result = db.Fetch<VAT_IN_H>("GetAllData_VAT_In_H", args);
            db.Close();
            return result;
        }

        public int CountAllData()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
            };

            int result = db.SingleOrDefault<int>("VAT_IN_H_CountAllData", args);
            db.Close();
            return result;
        }

        public IEnumerable<VAT_IN_H> GetListData(DateTime? TaxInvoiceDtFrm, DateTime? TaxInvoiceDtTo, string TaxInvoiceNoFrm,
            string TaxInvoiceNoTo, DateTime? SapPostDtFrm, DateTime? SapPostDtTo, string SapDocNoFrm, string SapDocNoTo,
            DateTime? ScanDtFrm, DateTime? ScanDtTo, string Name, DateTime? DLDtFrm, DateTime? DLDtTo, string NPWP,
            string Status, int Start, int Length)
        {
            if (TaxInvoiceNoFrm == "")
            {
                TaxInvoiceNoFrm = null;
            }
            if (TaxInvoiceNoTo == "")
            {
                TaxInvoiceNoTo = null;
            }
            if (SapDocNoFrm == "")
            {
                SapDocNoFrm = null;
            }
            if (SapDocNoTo == "")
            {
                SapDocNoTo = null;
            }
            if (Status == "")
            {
                Status = null;
            }
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvoiceDtFrm = TaxInvoiceDtFrm,
                TaxInvoiceDtTo = TaxInvoiceDtTo,
                TaxInvoiceNoFrm = TaxInvoiceNoFrm,
                TaxInvoiceNoTo = TaxInvoiceNoTo,
                SapPostDtFrm = SapPostDtFrm,
                SapPostDtTo = SapPostDtTo,
                SapDocNoFrm = SapDocNoFrm,
                SapDocNoTo = SapDocNoTo,
                ScanDtFrm = ScanDtFrm,
                ScanDtTo = ScanDtTo,
                Name = Name,
                Status = Status,
                DLDtFrm = DLDtFrm,
                DLDtTo = DLDtTo,
                NPWP = NPWP,
                Start = Start,
                Length = Length
            };

            IEnumerable<VAT_IN_H> result = db.Fetch<VAT_IN_H>("Get_VAT_In_H", args);
            db.Close();
            return result;
        }

        public int CountData(DateTime? TaxInvoiceDtFrm, DateTime? TaxInvoiceDtTo, string TaxInvoiceNoFrm,
            string TaxInvoiceNoTo, DateTime? SapPostDtFrm, DateTime? SapPostDtTo, string SapDocNoFrm, string SapDocNoTo,
            DateTime? ScanDtFrm, DateTime? ScanDtTo, string Name, DateTime? DLDtFrm, DateTime? DLDtTo, string NPWP,
            string Status)
        {
            if (TaxInvoiceNoFrm == "")
            {
                TaxInvoiceNoFrm = null;
            }
            if (TaxInvoiceNoTo == "")
            {
                TaxInvoiceNoTo = null;
            }
            if (SapDocNoFrm == "")
            {
                SapDocNoFrm = null;
            }
            if (SapDocNoTo == "")
            {
                SapDocNoTo = null;
            }
            if (Status == "")
            {
                Status = null;
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvoiceDtFrm = TaxInvoiceDtFrm,
                TaxInvoiceDtTo = TaxInvoiceDtTo,
                TaxInvoiceNoFrm = TaxInvoiceNoFrm,
                TaxInvoiceNoTo = TaxInvoiceNoTo,
                SapPostDtFrm = SapPostDtFrm,
                SapPostDtTo = SapPostDtTo,
                SapDocNoFrm = SapDocNoFrm,
                SapDocNoTo = SapDocNoTo,
                ScanDtFrm = ScanDtFrm,
                ScanDtTo = ScanDtTo,
                Name = Name,
                Status = Status,
                DLDtFrm = DLDtFrm,
                DLDtTo = DLDtTo,
                NPWP = NPWP
            };

            int result = 0;
            try
            {
                result = db.SingleOrDefault<int>("VAT_IN_H_Count", args);
                db.Close();
            }
            catch (Exception)
            {
                db.Close();
                result = 0;
            }

            return result;
        }

        public int EditData(string TaxInvoiceNo, string ReplacementFG, string SAPDocNo, DateTime SAPPostDt, string status)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvoiceNo,
                REPLACEMENT_FG = ReplacementFG,
                SAP_DOC_NO = SAPDocNo,
                SAP_POST_DT = SAPPostDt,
                STATUS = status
            };

            int result = db.Execute("VAT_IN_H_Edit", args);
            db.Close();
            return result;
        }

        public VAT_IN_H GetDetail(string TaxInvoiceNo, string ReplacementFG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvoiceNo,
                REPLACEMENT_FG = ReplacementFG
            };

            VAT_IN_H result = db.SingleOrDefault<VAT_IN_H>("VAT_IN_H_GetByID", args);
            db.Close();

            //result.SUPPLIER.NPWP = HelperRepository.Instance.GenNPWPFormat(result.SUPPLIER.NPWP);
            //result.TAX_INVOICE_NO = HelperRepository.Instance.GenTaxInvoiceFormat(result.TAX_INVOICE_NO);
            return result;
        }

        public bool InsertData(VAT_IN_ScanBarcode data, string Username, string Suppcode)
        {
            bool status = false;
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = data.noFaktur,
                REPLACEMENT_FG = data.fgPengganti,
                SAP_TAX_INVOICE_NO = data.noFaktur.Substring(data.noFaktur.Length - 8, 8),
                TRANS_TYPE_CODE = data.kdJenisTransaksi,
                SUPPLIER_CODE = Suppcode,
                DPP_PRICE = data.JmlDPP,
                VAT_PRICE = data.JmlPPN,
                LUXURY_TAX_PRICE = data.JmlPPnBM,
                STATUS = "1",
                TAX_INVOICE_MONTH = data.TanggalFaktur.Month.ToString(),
                TAX_INVOICE_YEAR = data.TanggalFaktur.Year.ToString(),
                TAX_INVOICE_DT = data.TanggalFaktur,
                IS_CREDITABLE = "1",
                //SAP_DOC_NO = "temp",
                //SAP_POST_DT = DateTime.Now.Date,
                SCAN_DT = DateTime.Now,
                DOWNLOAD_DT = DateTime.Now,
                SCAN_BY = Username,
                QR_CODE_PATH = data.QRPath,
                CREATED_DT = DateTime.Now,
                CHANGED_DT = DateTime.Now,
                CREATED_BY = Username,
                CHANGED_BY = Username
            };

            try
            {
                int result = db.Execute("VAT_IN_H_InsertData", args);
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

        public VAT_IN_H GetListCSVDataByID(string TaxInvNo, string ReplacementFG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo,
                ReplacementFG = ReplacementFG,
            };

            var result = db.SingleOrDefault<VAT_IN_H>("VAT_IN_HGetListCSVDataByID", args);
            db.Close();
            return result;
        }

        public int VAT_INUpdateStatusCSVDownload(string TaxInvNo, string ReplacementFG, string Status)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo,
                ReplacementFG = ReplacementFG,
                DownloadDt = DateTime.Now.Date,
                Status = Status
            };
            int result = db.Execute("VAT_INUpdateStatusCSVDownload", args);
            db.Close();
            return result;
        }

        //public VAT_IN_EXCELREPORT GetListDataExportExcel(DateTime? TaxInvoiceDtFrm, DateTime? TaxInvoiceDtTo, string TaxInvoiceNoFrm,
        //    string TaxInvoiceNoTo, DateTime? SapPostDtFrm, DateTime? SapPostDtTo, string SapDocNoFrm, string SapDocNoTo,
        //    DateTime? ScanDtFrm, DateTime? ScanDtTo, string Name, DateTime? DLDtFrm, DateTime? DLDtTo, string NPWP,
        //    string Status)
        //{
        //    IEnumerable<VAT_IN_H> ListVAT_IN_H = VAT_IN_HRepository.Instance.GetAllData(TaxInvoiceDtFrm, TaxInvoiceDtTo,
        //        TaxInvoiceNoFrm, TaxInvoiceNoTo, SapPostDtFrm, SapPostDtTo, SapDocNoFrm, SapDocNoTo, ScanDtFrm, ScanDtTo,
        //        Name, DLDtFrm, DLDtTo, NPWP, Status);
        //    VAT_IN_EXCELREPORT result = new VAT_IN_EXCELREPORT();
        //    result.VAT_IN = ListVAT_IN_H;
        //    decimal TotalDPPTemp = 0;
        //    decimal TotalVATTemp = 0;
        //    decimal TotalPPnBMTemp = 0;
        //    foreach (var item in result.VAT_IN)
        //    {
        //        TotalDPPTemp += item.DPP_PRICE;
        //        TotalVATTemp += item.VAT_PRICE;
        //        TotalPPnBMTemp += item.LUXURY_TAX_PRICE;
        //    }

        //    result.TotalDPP = TotalDPPTemp;
        //    result.TotalVAT = TotalVATTemp;
        //    result.TotalPPnBM = TotalPPnBMTemp;

        //    return result;
        //}

        public bool GetQR_CODE_PATH(string TaxInvNo)
        {
            bool exists = false;
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvNo
            };

            string result = db.SingleOrDefault<string>("VAT_IN_H_GetQR_CODE_PATH", args);
            db.Close();
            if (result != null)
            {
                return exists = true;
            }
            else
            {
                return exists;
            }

        }

        //Check duplicate based on enhancement request from TMMIN 2015-09-08, Added by Fid.Asep
        public int checkDuplicate(string TaxInvNo, string ReplacementFg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvNo,
                REPLACEMENT_FG = ReplacementFg
            };
            int result = db.SingleOrDefault<int>("VAT_IN_H_checkDuplicateHeader", args);
            db.Close();
            return result;
        }

        //Check original tax invoice based on enhancement request from TMMIN 2015-09-08, Added by Fid.Asep
        public int checkOriginal(string TaxInvNo, string ReplacementFg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvNo,
                REPLACEMENT_FG = ReplacementFg
            };
            int result = db.SingleOrDefault<int>("VAT_IN_H_checkOriginalTaxInvoice", args);
            db.Close();
            return result;
        }
    }
}