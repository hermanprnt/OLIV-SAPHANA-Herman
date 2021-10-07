using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.VAT_OUT
{
    public class VAT_OUTRepository
    {
        private VAT_OUTRepository() { }

        #region Singleton
        private static VAT_OUTRepository instance = null;
        public static VAT_OUTRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_OUTRepository();
                }
                return instance;
            }
        }
        #endregion

        public IEnumerable<VAT_OUT> tempIR { set; get; }
        public IEnumerable<VAT_OUT> GetVAT_OUT()
        {
            List<VAT_OUT> result = new List<VAT_OUT>();
            VAT_OUT vAT_OUT = null;
            Random numGen = new Random();
            for (int count = 1; count < 20; count++)
            {
                vAT_OUT = new VAT_OUT();
                vAT_OUT.TAX_INVOICE_NO = "500815_G0013" + numGen.Next(111, 999);
                vAT_OUT.DEBIT_ADVICE_NO = "5008229282";              
                vAT_OUT.NPWP = "15.0013.0292-" + numGen.Next(11, 99);
                vAT_OUT.NAME = "Ali";
                vAT_OUT.TAX_BASED = "400000";
                vAT_OUT.VAT = numGen.Next(111111111, 999999999);
                vAT_OUT.LUXURY_TAX = numGen.Next(1111111, 9999999);
                vAT_OUT.BUSINESS_UNIT = "tes";
                vAT_OUT.SAP_DOC_NO = "5008110292" + 1;
                vAT_OUT.TAX_INVOICE_DATE = DateTime.Now;
                vAT_OUT.STATUS = "Completed";
                vAT_OUT.INTERFACE_DT = DateTime.Now;
                vAT_OUT.DOWNLOAD_DT = DateTime.Now;
                vAT_OUT.COMPLETE_DT = DateTime.Now;

                result.Add(vAT_OUT);
            }
            tempIR = result;
            return result;
        }

        /*
        public List<string> GetInvoiceReceivingSort(string field, string sort)
        {
            List<String> result = new List<String>();

            List<InvoiceReceiving> returnResult = new List<InvoiceReceiving>();
            switch (field)
            {
                case "CERTIFICATE_ID":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.CERTIFICAT_ID).ToList() : tempIR.OrderByDescending(o => o.CERTIFICAT_ID).ToList());
                    break;
                case "SUPPLIER_CODE":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUPPLIER_CODE).ToList() : tempIR.OrderByDescending(o => o.SUPPLIER_CODE).ToList());
                    break;
                case "SUPPLIER_NAME":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUPPLIER_NAME).ToList() : tempIR.OrderByDescending(o => o.SUPPLIER_NAME).ToList());
                    break;
                case "INVOICE_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_NO).ToList() : tempIR.OrderByDescending(o => o.INVOICE_NO).ToList());
                    break;
                case "CURRENCY":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.CURRENCY).ToList() : tempIR.OrderByDescending(o => o.CURRENCY).ToList());
                    break;
                case "INVOICE_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_AMOUNT).ToList() : tempIR.OrderByDescending(o => o.INVOICE_AMOUNT).ToList());
                    break;
                case "INVOICE_TAX_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_TAX_NO).ToList() : tempIR.OrderByDescending(o => o.INVOICE_TAX_NO).ToList());
                    break;
                case "INVOICE_TAX_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_TAX_AMOUNT).ToList() : tempIR.OrderByDescending(o => o.INVOICE_TAX_AMOUNT).ToList());
                    break;
                case "SUBMIT_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUBMIT_DATE).ToList() : tempIR.OrderByDescending(o => o.SUBMIT_DATE).ToList());
                    break;
                case "SUBMIT_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUBMIT_BY).ToList() : tempIR.OrderByDescending(o => o.SUBMIT_BY).ToList());
                    break;
                case "RECEIVE_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.RECEIVE_STATUS).ToList() : tempIR.OrderByDescending(o => o.RECEIVE_STATUS).ToList());
                    break;
            }

            foreach (InvoiceReceiving item in returnResult)
            {
                result.Add("<tr>" +
                            "<td width=\"160px\" class=\"text-left\">" + item.CERTIFICAT_ID + "" +
                            "</td>" +
                            "<td width=\"80px\" class=\"text-center\">" + item.SUPPLIER_CODE + "" +
                            "</td>" +
                            "<td width=\"200px\" class=\"text-left ellipsis\" style=\"max-width: 201px;\">" + item.SUPPLIER_NAME + "" +
                            "</td>" +
                            "<td width=\"100px\" class=\"text-center\">" + item.INVOICE_NO + "" +
                            "</td>" +
                            "<td width=\"75px\" class=\"text-center\">" + item.CURRENCY + "" +
                            "</td>" +
                            "<td width=\"110px\" class=\"text-right\">" + item.INVOICE_AMOUNT.ToString("N0") + "" +
                            "</td>" +
                            "<td width=\"100px\" class=\"text-center\">" + item.INVOICE_TAX_NO + "" +
                            "</td>" +
                            "<td width=\"100px\" class=\"text-right\">" + item.INVOICE_TAX_AMOUNT.ToString("N0") + "" +
                            "</td>" +
                            "<td width=\"100px\" class=\"text-center\">" + (item.SUBMIT_DATE != DateTime.MinValue ? item.SUBMIT_DATE.ToString("dd.MM.yyyy") : "") + "" +
                            "</td>" +
                            "<td width=\"100px\" class=\"text-center\">" + item.SUBMIT_BY + "" +
                            "</td>" +
                            "<td width=\"100px\" class=\"text-left\">" + item.RECEIVE_STATUS + "" +
                            "</td>" +
                            "<td class=\"text-center\">" + item.NOTICE + "" +
                            "</td>" +
                        "</tr>");
            }

            return result;
        }
         * */
    }
}