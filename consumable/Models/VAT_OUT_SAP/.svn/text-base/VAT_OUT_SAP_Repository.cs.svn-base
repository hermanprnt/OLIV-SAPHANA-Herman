using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Credential;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class VAT_OUT_SAP_Repository
    {
        private VAT_OUT_SAP_Repository() { }
        private static VAT_OUT_SAP_Repository instance = null;

        public static VAT_OUT_SAP_Repository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_OUT_SAP_Repository();
                }
                return instance;
            }
        }
        public string UploadToDatabase(List<VAT_OUT_SAP_Model> list, string p_ProcessID, string Filenamepath, string filename, string username)
        {

            int i = 0;
            int max = list.Count()-1;
            IDBContext db = DatabaseManager.Instance.GetContext();
            
            try
            {
                string status = "";
                db.BeginTransaction();
                foreach (VAT_OUT_SAP_Model item in list)
                {
                    if (i == max)
                    {
                        status = "TRUE";
                    }
                    if (i >= 1 && i <= max)
                    {
                        dynamic args = new
                        {
                            TAX_INVOICE_NO = item.TAX_INVOICE_NO,
                            REPLACEMENT_FG = item.REPLACEMENT_FG,
                            DEBIT_ADVICE_NO = item.DEBIT_ADVICE_NO,
                            TRANS_TYPE_CODE = item.TRANS_TYPE_CODE,
                            INVOICE_MONTH = item.INVOICE_MONTH,
                            INVOICE_YEAR = item.INVOICE_YEAR,
                            INVOICE_DATE = item.INVOICE_DATE,
                            DPP_PRICE_H = item.DPP_PRICE,
                            VAT_PRICE_H = item.VAT_PRICE,
                            LUXURY_TAX_PRICE_H = item.LUXURY_TAX_PRICE,
                            REFERENCE = item.REFERENCE,
                            ADD_REMARK_ID = item.ADD_REMARK_ID,
                            BUSINESS_UNIT = item.BUSINESS_UNIT,
                            SAP_DOC_NO = item.SAP_DOC_NO,
                            PRODUCT_CODE = item.PRODUCT_CODE,
                            PRODUCT_NM = item.PRODUCT_NM,
                            UNIT_PRICE_D = item.UNIT_PRICE,
                            PRODUCT_QTY_D = item.PRODUCT_QTY,
                            TOTAL_PRICE_D = item.TOTAL_PRICE,
                            DISC_PRICE_D = item.DISC_PRICE,
                            DPP_PRICE_D = item.DPP_PRICE_D,
                            VAT_PRICE_D = item.VAT_PRICE_D,
                            LUXURY_TAX_PERC_D = item.LUXURY_TAX_PERC_D,
                            LUXURY_TAX_PRICE_D = item.LUXURY_TAX_PRICE_D,                            
                            NAME = item.NAME,
                            NPWP = item.NPWP,
                            ADDRESS = item.ADDRESS,
                            Increment = i,
                            FinishInsertTmpStatus = status,
                            ProcessID = p_ProcessID,
                            Filenamepath = Filenamepath,
                            Filename = filename,
                            UserName = username
                        };
                        db.Execute("INSERT_TEMP_VAT_OUT_SAP", args);
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