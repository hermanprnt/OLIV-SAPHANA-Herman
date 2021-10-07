using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class HelperRepository
    {
        private HelperRepository() { }
        private static HelperRepository instance = null;

        public static HelperRepository Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new HelperRepository();
                }
                return instance;
            }
        }

        public string GenTaxInvoiceFormat(string TaxInvoiceNo)
        {
            try
            {
                //string TaxInP1 = TaxInvoiceNo.Substring(0, 3);
                string TaxInP2 = TaxInvoiceNo.Substring(0, 3);
                string TaxInP3 = TaxInvoiceNo.Substring(3, 2);
                string TaxInP4 = TaxInvoiceNo.Substring(5, TaxInvoiceNo.Count() - 5);
                StringBuilder TaxIn = new StringBuilder();
                TaxIn.Append(TaxInP2).Append("-").Append(TaxInP3).Append(".").Append(TaxInP4);
                return TaxIn.ToString();
            }
            catch (Exception)
            {
                return "invalid";
            }
            
        }

        public string GenNPWPFormat(string NPWP)
        {
            try
            {
                string NPWP_P1 = NPWP.Substring(0, 2);
                string NPWP_P2 = NPWP.Substring(2, 3);
                string NPWP_P3 = NPWP.Substring(5, 3);
                string NPWP_P4 = NPWP.Substring(8, 1);
                string NPWP_P5 = NPWP.Substring(9, 3);
                string NPWP_P6 = NPWP.Substring(12, 3);
                StringBuilder NPWP_No = new StringBuilder();
                NPWP_No.Append(NPWP_P1).Append(".").Append(NPWP_P2).Append(".").Append(NPWP_P3).Append(".").Append(NPWP_P4)
                    .Append("-").Append(NPWP_P5).Append(".").Append(NPWP_P6);
                return NPWP_No.ToString();
            }
            catch (Exception)
            {
                return "invalid";
            }
            
        }

        public string AutoIncNumID(string ID)
        {
            int IDLength = ID.Length;
            int ParsedID = int.Parse(ID);
            ParsedID++;
            string FinalID = ParsedID.ToString();
            //StringBuilder ZeroSeq = new StringBuilder();
            string ZeroSeq = "";
            for (int i = IDLength; i > FinalID.Length; i--)
            {
                ZeroSeq = ZeroSeq + "0";
            }
            return FinalID = ZeroSeq + FinalID;
        }

        //public string Right(string value, int length)
        //{
        //    string result = value.Substring(value.Length - length);
        //    return result;
        //}

        public string Right2(string value, int length)
        {
            string result = value.Substring(value.Length - length);
            return result;
        }


        public AjaxResult SendMail(string templateMail, string createdBy, string paramDear, String to)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            dynamic args = new
            {
                TemplateMail = templateMail,
                CreatedBy = createdBy,
                ParamDear = paramDear,
                To = to
            };

            db.Execute("SendMailNotification", args);
            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

            return ajaxResult;
        }

    }
}