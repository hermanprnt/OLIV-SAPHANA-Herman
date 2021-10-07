using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class INV_POSTED_SAPRepository
    {
        private INV_POSTED_SAPRepository() { }
        private static INV_POSTED_SAPRepository instance = null;

        public static INV_POSTED_SAPRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new INV_POSTED_SAPRepository();
                }
                return instance;
            }
        }

        public string UploadToDatabase(List<INV_POSTED_SAP> list, string p_ProcessID, string Filenamepath, string filename, string pathandfilesuccess)
        {

            int i = 0;
            int max = list.Count() - 1;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                string status = "";
                db.BeginTransaction();
                foreach (INV_POSTED_SAP item in list)
                {
                    if (i == max)
                    {
                        status = "TRUE";
                    }
                    if (i >= 3 && i <= max)
                    {
                        dynamic args = new
                        {
                            VendorNo = item.VENDOR_NO,
                            VendorName = item.VENDOR_NAME,
                            Reference = item.REFERENCE,
                            Npwp = item.NPWP,
                            FakturPajak = item.FAKTUR_PAJAK,
                            DueOn = item.DUE_ON,
                            DocCurr = item.DOC_CURRENCY,
                            PPn_InDOcCurr = Convert.ToDecimal(item.PPN_IN_DOC_CURR),
                            PPn_InLocCurr = Convert.ToDecimal(item.PPN_IN_LOCAL_CURR),
                            DocNo = item.DOCUMENT_NO,
                            ClearingDocNo = item.CLEARING_DOC_NO,
                            Increment = i,
                            FinishInsertTmpStatus = status,
                            ProcessID = p_ProcessID,
                            Filenamepath = Filenamepath,
                            Filename = filename,
                            pathandfilesuccess = pathandfilesuccess
                        };
                        db.Execute("INV_POSTED_SAP_InsertTempTable", args);
                    }
                    db.CommitTransaction();
                    i++;
                }
                return "";
            }
            catch (Exception e)
            {
                db.AbortTransaction();
                return e.Message.ToString();
                //throw;
            }
        }
    }
}