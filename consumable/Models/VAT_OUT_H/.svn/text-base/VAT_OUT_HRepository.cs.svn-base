using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class VAT_OUT_HRepository
    {
        private VAT_OUT_HRepository() { }
        private static VAT_OUT_HRepository instance = null;

        public static VAT_OUT_HRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_OUT_HRepository();
                }
                return instance;
            }
        }

        public IEnumerable<VAT_OUT_H> GetListData(DateTime? TaxInvoiceDateFrom, DateTime? TaxInvoiceDateTo, DateTime? InterfaceDateFrom,
                     DateTime? InterfaceDateTo, DateTime? DownloadDateFrom, DateTime? DownloadDateTo, DateTime? CompleteDateFrom,
                     DateTime? CompleteDateTo, String TaxInvoiceNoFrom, String TaxInvoiceNoTo, String CustomerName,
                     String CustomerNPWP, String Status, int Start, int Length)
        {
            if(TaxInvoiceNoFrom == "")
            {
                TaxInvoiceNoFrom = null;
            }
            if(TaxInvoiceNoTo == "")
            {
                TaxInvoiceNoTo = null;
            }
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_DT_FROM = TaxInvoiceDateFrom,
                TAX_INVOICE_DT_TO = TaxInvoiceDateTo,
                INTERFACE_DT_FROM = InterfaceDateFrom,
                INTERFACE_DT_TO = InterfaceDateTo,
                DOWNLOAD_DT_FROM = DownloadDateFrom,
                DOWNLOAD_DT_TO = DownloadDateTo,
                COMPLETE_DT_FROM = CompleteDateFrom,
                COMPLETE_DT_TO = CompleteDateTo,
                TAX_INVOICE_NO_FROM = TaxInvoiceNoFrom,
                TAX_INVOICE_NO_TO = TaxInvoiceNoTo,
                VAT_OUT_STATUS = Status,
                CustomerName = CustomerName,
                CustomerNPWP = CustomerNPWP,
                Start = Start,
                Length = Length
            };

            IEnumerable<VAT_OUT_H> result = db.Fetch<VAT_OUT_H>("VAT_OUT_H_GetListData", args);
            db.Close();
            return result;
        }

        public int CountData(DateTime? TaxInvoiceDateFrom, DateTime? TaxInvoiceDateTo, DateTime? InterfaceDateFrom,
                     DateTime? InterfaceDateTo, DateTime? DownloadDateFrom, DateTime? DownloadDateTo, DateTime? CompleteDateFrom,
                     DateTime? CompleteDateTo, String TaxInvoiceNoFrom, String TaxInvoiceNoTo, String CustomerName,
                     String CustomerNPWP, String Status)
        {
            if (TaxInvoiceNoFrom == "")
            {
                TaxInvoiceNoFrom = null;
            }
            if (TaxInvoiceNoTo == "")
            {
                TaxInvoiceNoTo = null;
            }
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                TAX_INVOICE_DT_FROM = TaxInvoiceDateFrom,
                TAX_INVOICE_DT_TO = TaxInvoiceDateTo,
                INTERFACE_DT_FROM = InterfaceDateFrom,
                INTERFACE_DT_TO = InterfaceDateTo,
                DOWNLOAD_DT_FROM = DownloadDateFrom,
                DOWNLOAD_DT_TO = DownloadDateTo,
                COMPLETE_DT_FROM = CompleteDateFrom,
                COMPLETE_DT_TO = CompleteDateTo,
                TAX_INVOICE_NO_FROM = TaxInvoiceNoFrom,
                TAX_INVOICE_NO_TO = TaxInvoiceNoTo,
                VAT_OUT_STATUS = Status,
                CustomerName = CustomerName,
                CustomerNPWP = CustomerNPWP
            };

            var result = 0;

            try
            {
                result = db.SingleOrDefault<int>("VAT_OUT_HCountData", args);

                db.Close();
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        //Added By Yayan--
        public VAT_OUT_H GetDetail(string TaxInvoiceNo, string DebitAdviceNo, string ReplacementFG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvoiceNo,
                DEBIT_ADVICE_NO = DebitAdviceNo,
                REPLACEMENT_FG = ReplacementFG
            };
            VAT_OUT_H result = db.SingleOrDefault<VAT_OUT_H>("VAT_OUT_H_GetByID", args);
            db.Close();

            //result.TAX_INVOICE_NO = HelperRepository.Instance.GenTaxInvoiceFormat(result.TAX_INVOICE_NO);
            return result;
        }
        //Added By Yayan--

        public IEnumerable<VAT_OUT_H> GetAllData(DateTime? TaxInvoiceDateFrom, DateTime? TaxInvoiceDateTo, DateTime? InterfaceDateFrom,
                     DateTime? InterfaceDateTo, DateTime? DownloadDateFrom, DateTime? DownloadDateTo, DateTime? CompleteDateFrom,
                     DateTime? CompleteDateTo, String TaxInvoiceNoFrom, String TaxInvoiceNoTo, String CustomerName,
                     String CustomerNPWP, String Status)
        {
            if (TaxInvoiceNoFrom == "")
            {
                TaxInvoiceNoFrom = null;
            }
            if (TaxInvoiceNoTo == "")
            {
                TaxInvoiceNoTo = null;
            }
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_DT_FROM = TaxInvoiceDateFrom,
                TAX_INVOICE_DT_TO = TaxInvoiceDateTo,
                INTERFACE_DT_FROM = InterfaceDateFrom,
                INTERFACE_DT_TO = InterfaceDateTo,
                DOWNLOAD_DT_FROM = DownloadDateFrom,
                DOWNLOAD_DT_TO = DownloadDateTo,
                COMPLETE_DT_FROM = CompleteDateFrom,
                COMPLETE_DT_TO = CompleteDateTo,
                TAX_INVOICE_NO_FROM = TaxInvoiceNoFrom,
                TAX_INVOICE_NO_TO = TaxInvoiceNoTo,
                VAT_OUT_STATUS = Status,
                CustomerName = CustomerName,
                CustomerNPWP = CustomerNPWP
            };

            IEnumerable<VAT_OUT_H> result = db.Fetch<VAT_OUT_H>("VAT_OUT_H_GetAllData", args);
            db.Close();
            return result;
        }

        #region Upload Tax Invoice From SAP
        public int DeleteTempData()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            int result = db.Execute("TaxInvUploadDeleteRows");
            db.Close();
            return result;
        }

        public void SaveTempData(string insertCommand)
        {
            string constr = DatabaseManager.Instance.GetDefaultConnectionDescriptor().ConnectionString;
            using (SqlConnection SqlCon = new SqlConnection(constr))
            {
                SqlCon.Open();
                using (SqlCommand SqlCmd = new SqlCommand(insertCommand, SqlCon))
                {
                    SqlCmd.CommandTimeout = 0;
                    SqlCmd.ExecuteNonQuery();
                }
            }
        }

        #endregion Upload Tax Invoice From SAP

        //Added By Andy--
        public string GetFileName(string TaxInvoiceNo)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TAX_INVOICE_NO = TaxInvoiceNo
            };
            string result = db.SingleOrDefault<string>("GetFIleName", args);
            db.Close();
            return result;
        }

        public void UpdatePdfFilenameandstatus(string pdffilename, string TaxInvoiceNo)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                PdfFileName = pdffilename,
                TaxInvoiceNo = TaxInvoiceNo
            };
            db.Execute("UploadApproved_UpdatePdfFileNameandStatus", args);
            db.Close();
        }

        public int VAT_OUTUpdateStatusCSVDownload(string TaxInvNo, string ReplacementFG, string DebitAdviceNo, string Status)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo,
                ReplacementFG = ReplacementFG,
                DebitAdviceNo = DebitAdviceNo,
                DownloadDt = DateTime.Now.Date,
                Status = Status
            };
            int result = db.Execute("VAT_OUTUpdateStatusCSVDownload", args);
            db.Close();
            return result;
        }

        public int CheckdataExist(string TaxInvNo)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo
            };
            int result = db.SingleOrDefault<int>("UpploadApproved_CheckDataExist", args);
            db.Close();
            return result;
        }

        public int CheckStatusRegister(string TaxInvNo)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                TaxInvNo = TaxInvNo
            };
            int result = db.SingleOrDefault<int>("UpploadApproved_CheckStatusRegister", args);
            db.Close();
            return result;
        }
    }
}