using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Common.Data;
using NPOI.SS.UserModel;
using consumable.Controllers;
using NPOI.HSSF.UserModel;
using System.IO;
using consumable.Models.InvoiceCreation;
using consumable.Commons.Constants;
using consumable.Models.SUPPLIER;
using consumable.Commons;
using consumable.Models.SystemProperty;
using System.Data.SqlClient;

namespace consumable.Models.InvoiceInquiry
{
    public class InvoiceInquiryRepository
    {
        private InvoiceInquiryRepository() { }

        #region Singleton
        private static InvoiceInquiryRepository instance = null;
        public static InvoiceInquiryRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InvoiceInquiryRepository();
                }
                return instance;
            }
        }
        #endregion
        private DatabaseManager databaseManager = DatabaseManager.Instance;//20200706


        public int countInvoiceInquiry(string createdDate, string submissionDate, string supplierSearch, string invoiceDate, string statusSearch, string statusHardcopy,
            string planPaymentDate, string invoiceNo)
        {
            string createdDateFrom = "";
            string createdDateTo = "";
            if (createdDate != null && !"".Equals(createdDate))
            {
                string[] createdDateArray = createdDate.Split('-');
                createdDateFrom = createdDateArray[0].Trim();
                createdDateTo = createdDateArray[1].Trim();
            }

            string submissionDateFrom = "";
            string submissionDateTo = "";
            if (submissionDate != null && !"".Equals(submissionDate))
            {
                string[] submissionDateArray = submissionDate.Split('-');
                submissionDateFrom = submissionDateArray[0].Trim();
                submissionDateTo = submissionDateArray[1].Trim();
            }

            string invoiceDateFrom = "";
            string invoiceDateTo = "";
            if (invoiceDate != null && !"".Equals(invoiceDate))
            {
                string[] invoiceDateArray = invoiceDate.Split('-');
                invoiceDateFrom = invoiceDateArray[0].Trim();
                invoiceDateTo = invoiceDateArray[1].Trim();
            }

            string planPaymentDateFrom = "";
            string planPaymentDateTo = "";
            if (planPaymentDate != null && !"".Equals(planPaymentDate))
            {
                string[] planPaymentDateArray = planPaymentDate.Split('-');
                planPaymentDateFrom = planPaymentDateArray[0].Trim();
                planPaymentDateTo = planPaymentDateArray[1].Trim();
            }


            if (supplierSearch != null && !"".Equals(supplierSearch))
            {
                string[] supplierSearchArray = supplierSearch.Split(';');

                for (int i = 0; i < supplierSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplierSearch = "'" + supplierSearchArray[i] + "'";
                    }
                    else
                    {
                        supplierSearch = supplierSearch + ",'" + supplierSearchArray[i] + "'";
                    }
                }
            }

            if (statusSearch != null && !"".Equals(statusSearch))
            {
                string[] statusSearchArray = statusSearch.Split(';');

                for (int i = 0; i < statusSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        statusSearch = "'" + statusSearchArray[i] + "'";
                    }
                    else
                    {
                        statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
                    }
                }
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CREATED_DATE_FROM = createdDateFrom,
                CREATED_DATE_TO = createdDateTo,
                SUBMISSION_DATE_FROM = submissionDateFrom,
                SUBMISSION_DATE_TO = submissionDateTo,
                INVOICE_DATE_FROM = invoiceDateFrom,
                INVOICE_DATE_TO = invoiceDateTo,
                PLAN_PAYMENT_DATE_FROM = planPaymentDateFrom,
                PLAN_PAYMENT_DATE_TO = planPaymentDateTo,
                INVOICE_NO = invoiceNo,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                HARDCOPY_STATUS = statusHardcopy
            };
            return db.SingleOrDefault<int>("CountInvoiceInquiry", args);
        }

        public List<InvoiceInquiry> GetInvoiceInquiryList(string createdDate, string submissionDate, string supplierSearch, string invoiceDate, string statusSearch, string statusHardcopySearch,
            string planPaymentDate, string invoiceNo, int fromNumber, int toNumber)
        {
            string createdDateFrom = "";
            string createdDateTo = "";
            if (createdDate != null && !"".Equals(createdDate))
            {
                string[] createdDateArray = createdDate.Split('-');
                createdDateFrom = createdDateArray[0].Trim();
                createdDateTo = createdDateArray[1].Trim();
            }

            string submissionDateFrom = "";
            string submissionDateTo = "";
            if (submissionDate != null && !"".Equals(submissionDate))
            {
                string[] submissionDateArray = submissionDate.Split('-');
                submissionDateFrom = submissionDateArray[0].Trim();
                submissionDateTo = submissionDateArray[1].Trim();
            }

            string invoiceDateFrom = "";
            string invoiceDateTo = "";
            if (invoiceDate != null && !"".Equals(invoiceDate))
            {
                string[] invoiceDateArray = invoiceDate.Split('-');
                invoiceDateFrom = invoiceDateArray[0].Trim();
                invoiceDateTo = invoiceDateArray[1].Trim();
            }

            string planPaymentDateFrom = "";
            string planPaymentDateTo = "";
            if (planPaymentDate != null && !"".Equals(planPaymentDate))
            {
                string[] planPaymentDateArray = planPaymentDate.Split('-');
                planPaymentDateFrom = planPaymentDateArray[0].Trim();
                planPaymentDateTo = planPaymentDateArray[1].Trim();
            }


            if (supplierSearch != null && !"".Equals(supplierSearch))
            {
                string[] supplierSearchArray = supplierSearch.Split(';');

                for (int i = 0; i < supplierSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplierSearch = "'" + supplierSearchArray[i] + "'";
                    }
                    else
                    {
                        supplierSearch = supplierSearch + ",'" + supplierSearchArray[i] + "'";
                    }
                }
            }

            if (statusSearch != null && !"".Equals(statusSearch))
            {
                string[] statusSearchArray = statusSearch.Split(';');

                for (int i = 0; i < statusSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        statusSearch = "'" + statusSearchArray[i] + "'";
                    }
                    else
                    {
                        statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
                    }
                }
            }

            SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
            SystemProperty.SystemProperty system = systemPropertyRepo.GetSysPropByCodeAndType("IP_ADDRESS", "INVOICE_INQUIRY");

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CREATED_DATE_FROM = createdDateFrom,
                CREATED_DATE_TO = createdDateTo,
                SUBMISSION_DATE_FROM = submissionDateFrom,
                SUBMISSION_DATE_TO = submissionDateTo,
                INVOICE_DATE_FROM = invoiceDateFrom,
                INVOICE_DATE_TO = invoiceDateTo,
                PLAN_PAYMENT_DATE_FROM = planPaymentDateFrom,
                PLAN_PAYMENT_DATE_TO = planPaymentDateTo,
                INVOICE_NO = invoiceNo,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                HARDCOPY_STATUS = statusHardcopySearch,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString(),
                LinkedServer = system.SYSTEM_VALUE_TEXT
            };

            List<InvoiceInquiry> result = db.Fetch<InvoiceInquiry>("GetInvoiceInquiry", args);
            db.Close();
                return result;
        }

        public List<String> GetInvoiceInquirySort(string createdDate, string submissionDate, string supplierSearch, string invoiceDateSearch,
            string statusSearch, string statusHardcopySearch, string planPaymentDateSearch, string invoiceNoSearch, int fromNumber, int toNumber,
            string field, string sort)
        {
            List<String> result = new List<String>();
            List<InvoiceInquiry> resultItem = new List<InvoiceInquiry>();
            resultItem = GetInvoiceInquiryList(createdDate, submissionDate, supplierSearch, invoiceDateSearch, statusSearch, statusHardcopySearch,
                planPaymentDateSearch, invoiceNoSearch, fromNumber, toNumber);

            List<InvoiceInquiry> returnResult = new List<InvoiceInquiry>();
            switch (field)
            {
                case "INVOICE_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INVOICE_NO).ToList() : resultItem.OrderByDescending(o => o.INVOICE_NO).ToList());
                    break;
                case "INVOICE_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INVOICE_DATE).ToList() : resultItem.OrderByDescending(o => o.INVOICE_DATE).ToList());
                    break;
                case "CURRENCY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CURRENCY).ToList() : resultItem.OrderByDescending(o => o.CURRENCY).ToList());
                    break;
                case "TOTAL_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TOTAL_AMOUNT).ToList() : resultItem.OrderByDescending(o => o.TOTAL_AMOUNT).ToList());
                    break;
                case "TURN_OVER":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TURN_OVER).ToList() : resultItem.OrderByDescending(o => o.TURN_OVER).ToList());
                    break;
                case "STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.STATUS).ToList() : resultItem.OrderByDescending(o => o.STATUS).ToList());
                    break;
                case "SUPPLIER_CD":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUPPLIER_CD).ToList() : resultItem.OrderByDescending(o => o.SUPPLIER_CD).ToList());
                    break;
                case "SUPPLIER_NAME":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUPPLIER_NAME).ToList() : resultItem.OrderByDescending(o => o.SUPPLIER_NAME).ToList());
                    break;
                case "PROGRESS_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PROGRESS_STATUS).ToList() : resultItem.OrderByDescending(o => o.PROGRESS_STATUS).ToList());
                    break;
                case "PAYMENT_DOC_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PAYMENT_DOC_NO).ToList() : resultItem.OrderByDescending(o => o.PAYMENT_DOC_NO).ToList());
                    break;
                case "PAYMENT_PLAN_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PAYMENT_PLAN_DATE).ToList() : resultItem.OrderByDescending(o => o.PAYMENT_PLAN_DATE).ToList());
                    break;
                case "PAYMENT_ACTUAL_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PAYMENT_ACTUAL_DATE).ToList() : resultItem.OrderByDescending(o => o.PAYMENT_ACTUAL_DATE).ToList());
                    break;
                case "CREATED_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CREATED_BY).ToList() : resultItem.OrderByDescending(o => o.CREATED_BY).ToList());
                    break;
                case "CREATED_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CREATED_DT).ToList() : resultItem.OrderByDescending(o => o.CREATED_DT).ToList());
                    break;
                case "SUBMIT_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUBMIT_BY).ToList() : resultItem.OrderByDescending(o => o.SUBMIT_BY).ToList());
                    break;
                case "SUBMIT_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUBMIT_DT).ToList() : resultItem.OrderByDescending(o => o.SUBMIT_DT).ToList());
                    break;
                case "POSTED_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.POSTED_BY).ToList() : resultItem.OrderByDescending(o => o.POSTED_BY).ToList());
                    break;
                case "POSTED_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.POSTED_DT).ToList() : resultItem.OrderByDescending(o => o.POSTED_DT).ToList());
                    break;
                case "SAP_DOC_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SAP_DOC_NO).ToList() : resultItem.OrderByDescending(o => o.SAP_DOC_NO).ToList());
                    break;
            }

            if (returnResult != null && returnResult.Count > 0)
            {
                foreach (InvoiceInquiry ii in returnResult)
                {
                    string checkbox = null;
                    if (ii.STATUS == "CANCELLED" || ii.STATUS == "PAID")
                    {
                        checkbox = "<input type=\"checkbox\" name=\"selected\" class=\"check\" disabled />";
                    }
                    else
                    {
                        checkbox = "<input type=\"checkbox\" name=\"selected\" class=\"check\"  />";
                    }

                    string STATUS = null;
                    if (ii.STATUS != "CANCELLED")
                    {
                        STATUS = "<td class=\"text-center cursor-link\" width=\"100px\"><a onclick=\"showDetail('" + ii.INVOICE_ID + "', '" + ii.INVOICE_NO + "'); return false;\">" + ii.INVOICE_NO + "</a></td>";
                    }
                    else
                    {
                        STATUS = "<td class=\"text-center\" width=\"100px\">" + ii.INVOICE_NO + "</td>";
                    }

                    string INVOICE_DATE = ii.INVOICE_DATE.HasValue ? ii.INVOICE_DATE.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string TOTAL_AMOUNT;
                    if (ii.CURRENCY.Equals("IDR"))
                    { TOTAL_AMOUNT = ii.TOTAL_AMOUNT.FormatCommaSeparator(); }
                    else
                    { TOTAL_AMOUNT = ii.TOTAL_AMOUNT.FormatCommaSeparatorWithDecimal(); }

                    string TURN_OVER;
                    if (ii.CURRENCY.Equals("IDR"))
                    { TURN_OVER = ii.TURN_OVER.FormatCommaSeparator(); }
                    else
                    { TURN_OVER = ii.TURN_OVER.FormatCommaSeparatorWithDecimal(); }

                    string PAYMENT_PLAN_DATE = ii.PAYMENT_PLAN_DATE.HasValue ? ii.PAYMENT_PLAN_DATE.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string PAYMENT_ACTUAL_DATE = ii.PAYMENT_ACTUAL_DATE.HasValue ? ii.PAYMENT_ACTUAL_DATE.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string SUBMIT_DT = ii.SUBMIT_DT.HasValue ? ii.SUBMIT_DT.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string POSTED_DT = ii.POSTED_DT.HasValue ? ii.POSTED_DT.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string NOTICE = null;
                    if (ii.NOTICE_DATE.HasValue)
                    {
                        NOTICE = "<td></td>";
                    }
                    else
                    {
                        NOTICE = "<td class=\"text-center cursor-link\"><i class=\"fa fa-envelope\" onclick=\"openPopupNotice('" + ii.NOTICE_BY + "', '" + (ii.INVOICE_DATE.HasValue ? ii.INVOICE_DATE.Value.ToString("dd.MM.yyyy") : string.Empty + "' , '" + ii.NOTICE_REMARK) + "')\"></i></td>";
                    }

                    result.Add(
                    "<tr>" +
                        "<td class=\"text-center\" width=\"30px\">" +
                            checkbox +
                        "</td>" +
                        "<td class=\"text-center cursor-link\" width=\"30px\">" +
                            "<i class=\"fa fa-file-pdf-o\" onclick=\"showFileDownload('" + ii.INVOICE_ID + "','" + ii.INVOICE_NO + "');\"></i>" +
                        "</td>" +
                        STATUS +
                        "<td class=\"text-center\" width=\"75px\">" +
                            INVOICE_DATE +
                        "</td>" +
                        "<td class=\"text-center\" width=\"70px\">" +
                            ii.CURRENCY +
                        "</td>" +
                        "<td class=\"text-right\" width=\"100px\">" +
                            TOTAL_AMOUNT +
                        "</td>" +
                        "<td class=\"text-right\" width=\"100px\">" +
                            TURN_OVER +
                        "</td>" +
                        "<td class=\"text-left\" width=\"75px\">" +
                            ii.STATUS +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            ii.SUPPLIER_CD +
                        "</td>" +
                        "<td class=\"text-left\"  width=\"200px\">" +
                            ii.SUPPLIER_NAME +
                        "</td>" +
                        "<td class=\"text-center\" width=\"80px\">" +
                            PAYMENT_PLAN_DATE +
                        "</td>" +
                        "<td class=\"text-center\" width=\"80px\">" +
                            PAYMENT_ACTUAL_DATE +
                        "</td>" +
                        "<td class=\"text-center\" width=\"110px\">" +
                            ii.PROGRESS_STATUS +
                        "</td>" +
                        "<td class=\"text-center ellipsis\" width=\"100px\" style=\"max-width: 100px;\" title=" + ii.SUBMIT_BY + ">" +
                            ii.CREATED_BY +
                        "</td>" +
                        "<td class=\"text-center\" width=\"80px\">" +
                            ii.CREATED_DT +
                        "</td>" +
                        "<td class=\"text-center ellipsis\" width=\"100px\" style=\"max-width: 100px;\" title=" + ii.SUBMIT_BY + ">" +
                            ii.SUBMIT_BY +
                        "</td>" +
                        "<td class=\"text-center\" width=\"80px\">" +
                            SUBMIT_DT +
                        "</td>" +
                        "<td class=\"text-center ellipsis\" width=\"100px\" style=\"max-width: 100px;\" title=" + ii.POSTED_BY + ">" +
                            ii.POSTED_BY +
                        "</td>" +
                        "<td class=\"text-center\" width=\"80px\">" +
                            POSTED_DT +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            ii.SAP_DOC_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            ii.PAYMENT_DOC_NO +
                        "</td>" +
                        NOTICE +
                    "</tr>");

                }
            }
            else
            {
                result.Add(
                    "<tr>" +
                    "<td colspan=\"25\" class=\"text-center\">" +
                    "No data retrieved." +
                    "</td>" +
                    "</tr>");
            }

            return result;
        }

        public List<InvoiceInquiryDetail> GetInvoiceInquiryDetail(string invoiceId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId
            };

            List<InvoiceInquiryDetail> result = db.Fetch<InvoiceInquiryDetail>("GetInvoiceInquiryDetail", args);
            db.Close();

            return result;
        }

        public List<InvoiceErrorDetail> GetInvoiceInquiryDetailError(string invoiceId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId
            };

            List<InvoiceErrorDetail> result = db.Fetch<InvoiceErrorDetail>("GetInvoiceInquiryErrorDetail", args);
            return result;

        }





        public List<InvoiceAttachment> GetListFileDownload(string invoiceId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId
            };

            List<InvoiceAttachment> result = db.Fetch<InvoiceAttachment>("GetListFileDownloadInquiry", args);
            db.Close();

            return result;
        }

        public List<PpvAttachment> GetListFilePpvDownload(string invoiceId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId
            };

            List<PpvAttachment> result = db.Fetch<PpvAttachment>("GetListFilePpvDownloadInquiry", args);
            db.Close();

            return result;
        }

        public void updateUsedFlagEFakturCancelInvoice(IDBContext db, string taxInvoiceNo, string NoReg)
        {
            dynamic args = new
            {
                TAX_INVOICE_NO = taxInvoiceNo,
                UPDATED_BY = NoReg
            };

            db.Execute("UpdateEfakturCancelInvoice", args);

        }


        public AjaxResult cancelInvoice(IDBContext db, string invoiceId, string taxInvoiceNo, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    INVOICE_ID = invoiceId,
                    TAX_INVOICE_NO = taxInvoiceNo,
                    UPDATED_BY = NoReg,
                    UPDATED_DT = DateTime.Now
                };

                db.Execute("CancelInvoiceInquiry", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        //FID.Ridwan: 20220719
        public void DeleteInvoiceFromPAS(string invoiceId)
        {
            //modify by fid.ahmad change db config PAS to dblink system master
            //IDBContext db = DatabaseManager.Instance.GetContext("PAS_DB");
            IDBContext db = DatabaseManager.Instance.GetContext("");
            dynamic args = new
            {
                INVOICE_ID = invoiceId
            };

            db.Execute("DeleteInvoiceFromPAS", args);

        }

        public List<InvoiceInquiryDetail> GetPOForPosting(string invoiceId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId
            };

            List<InvoiceInquiryDetail> result = db.Fetch<InvoiceInquiryDetail>("GetPOForPosting", args);
            db.Close();

            return result;
        }


        public AjaxResult updateInvoiceWithCancelGR(string invoiceId, string newStatus, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    INVOICE_ID = invoiceId,
                    NEW_STATUS = newStatus,
                    UPDATED_BY = NoReg,
                    UPDATED_DT = DateTime.Now
                };

                db.Execute("UpdateInvoiceWithCancelGR", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            finally
            {
                db.Close();
            }

            return ajaxResult;
        }

        //for direct SAP Access
        public AjaxResult SavePostInvoiceDirect(InvoiceInquiry invoice, CREATE_INV_TABLE e, string newStatus, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    INVOICE_ID = invoice.INVOICE_ID,
                    NEW_STATUS = newStatus,
                    INVOICE_NOTE = invoice.INVOICE_NOTE,
                    TAX_CODE = invoice.TAX_CODE,
                    ASSIGNMENT = invoice.ASSIGNMENT,
                    BASELINE_DT = e.INPUT[0].BASE_DATE.Value.ToString("yyyy-MM-dd"),
                    TERM_PAY = invoice.TERM_PAY,
                    PAY_METHOD = invoice.PAY_METHOD,
                    LOG_DOC_NO = e.OUTPUT[0].INVOICE_NO,
                    SAP_DOC_NO = e.OUTPUT[0].ACCDOC_NO,
                    POSTED_BY = NoReg,
                    POSTED_DT = e.INPUT[0].POST_DATE.Value.ToString("yyyy-MM-dd"),
                    SAP_YEAR = e.OUTPUT[0].YEAR,
                    UPDATED_BY = NoReg,
                    UPDATED_DT = DateTime.Now
                };

                db.Execute("SavePostInvoice", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            finally
            {
                db.Close();
            }

            return ajaxResult;
        }

        public AjaxResult SaveReverseInvoiceNonDirect(IDBContext db, CANCEL_INV_TABLE data, string invoiceId,
            string taxInvoiceNo, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    INVOICE_NO_IN = data.INPUT[0].INVOICE_NO,
                    YEAR_IN = data.INPUT[0].YEAR,
                    REASON_IN = data.INPUT[0].REASON,
                    POST_DATE_IN = data.INPUT[0].POST_DATE,

                    //INVOICE_NO_OUT = data.OUTPUT[0].INVOICE_NO,
                    //YEAR_OUT = data.OUTPUT[0].YEAR,
                    //REV_INV_NO_OUT = data.OUTPUT[0].REV_INV_NO,
                    //REV_YEAR_OUT = data.OUTPUT[0].REV_YEAR,
                    //TYPE_OUT = data.OUTPUT[0].TYPE,
                    //MESSAGE_OUT = data.OUTPUT[0].MESSAGE,

                    INVOICE_ID = invoiceId,
                    CREATED_DT = DateTime.Now,
                    CREATED_BY = NoReg
                };

                db.Execute("InsertReverseInvoiceLog", args);

                dynamic args2 = new
                {
                    REASON_IN = data.INPUT[0].REASON,
                    POST_DATE_IN = data.INPUT[0].POST_DATE,

                    //INVOICE_NO_OUT = data.OUTPUT[0].INVOICE_NO,
                    //YEAR_OUT = data.OUTPUT[0].YEAR,
                    //REV_INV_NO_OUT = data.OUTPUT[0].REV_INV_NO,
                    //REV_YEAR_OUT = data.OUTPUT[0].REV_YEAR,
                    UPDATED_BY = NoReg,

                    INVOICE_ID = invoiceId,
                    TAX_INVOICE_NO = taxInvoiceNo,
                };

                db.Execute("UpdateReverseInvoiceSAP", args2);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.SuccMesgs = new String[] { "Reverse submitted, wait for batch" };

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }


        public AjaxResult SaveReverseInvoiceDirect(IDBContext db, CANCEL_INV_TABLE data, string invoiceId,
            string taxInvoiceNo, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    INVOICE_NO_IN = data.INPUT[0].INVOICE_NO,
                    YEAR_IN = data.INPUT[0].YEAR,
                    REASON_IN = data.INPUT[0].REASON,
                    POST_DATE_IN = data.INPUT[0].POST_DATE,

                    INVOICE_NO_OUT = data.OUTPUT[0].INVOICE_NO,
                    YEAR_OUT = data.OUTPUT[0].YEAR,
                    REV_INV_NO_OUT = data.OUTPUT[0].REV_INV_NO,
                    REV_YEAR_OUT = data.OUTPUT[0].REV_YEAR,
                    TYPE_OUT = data.OUTPUT[0].TYPE,
                    MESSAGE_OUT = data.OUTPUT[0].MESSAGE,

                    //INVOICE_ID = invoiceId,
                    CREATED_DT = DateTime.Now,
                    CREATED_BY = NoReg
                };

                db.Execute("InsertReverseInvoiceLogDirect", args);

                if (CommonConstant.STATUS_SUCCESS_SAP.Equals(data.OUTPUT[0].TYPE))
                {
                    dynamic args2 = new
                    {
                        REASON_IN = data.INPUT[0].REASON,
                        POST_DATE_IN = data.INPUT[0].POST_DATE,

                        INVOICE_NO_OUT = data.OUTPUT[0].INVOICE_NO,
                        YEAR_OUT = data.OUTPUT[0].YEAR,
                        REV_INV_NO_OUT = data.OUTPUT[0].REV_INV_NO,
                        REV_YEAR_OUT = data.OUTPUT[0].REV_YEAR,
                        UPDATED_BY = NoReg,

                        INVOICE_ID = invoiceId,
                        TAX_INVOICE_NO = taxInvoiceNo,
                    };

                    db.Execute("UpdateReverseInvoiceSAPDirect", args2);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                    ajaxResult.SuccMesgs = new String[] { data.OUTPUT[0].MESSAGE + " : " + data.OUTPUT[0].REV_INV_NO };
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new String[] { data.OUTPUT[0].MESSAGE };
                }
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public AjaxResult SubmitNotice(IDBContext db, List<InvoiceInquiry> invoiceList)
        {
            Console.Write("Submit Notice Invoice Inquiry");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (InvoiceInquiry item in invoiceList)
                {
                    dynamic args = new
                    {
                        InvoiceId = item.INVOICE_ID,
                        NoticeDate = item.NOTICE_DATE,
                        Remarks = item.NOTICE_REMARK,
                        NoticeBy = "temp.mr1",
                        ChangeBy = "temp.mr1"
                    };

                    result = db.Execute("NoticeInvoiceInquiry", args);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

                }
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public List<InvoiceInquiry> GetInvoiceInquiryExcel(string createdDate, string supplierSearch, string invoiceDate, string statusSearch, string statusHardcopySearch,
            string planPaymentDate, string invoiceNo, string submissionDate)
        {
            string createdDateFrom = "";
            string createdDateTo = "";
            if (createdDate != null && !"".Equals(createdDate))
            {
                string[] createdDateArray = createdDate.Split('-');
                createdDateFrom = createdDateArray[0].Trim();
                createdDateTo = createdDateArray[1].Trim();
            }

            string invoiceDateFrom = "";
            string invoiceDateTo = "";
            if (invoiceDate != null && !"".Equals(invoiceDate))
            {
                string[] invoiceDateArray = invoiceDate.Split('-');
                invoiceDateFrom = invoiceDateArray[0].Trim();
                invoiceDateTo = invoiceDateArray[1].Trim();
            }

            string planPaymentDateFrom = "";
            string planPaymentDateTo = "";
            if (planPaymentDate != null && !"".Equals(planPaymentDate))
            {
                string[] planPaymentDateArray = planPaymentDate.Split('-');
                planPaymentDateFrom = planPaymentDateArray[0].Trim();
                planPaymentDateTo = planPaymentDateArray[1].Trim();
            }


            if (supplierSearch != null && !"".Equals(supplierSearch))
            {
                string[] supplierSearchArray = supplierSearch.Split(';');

                for (int i = 0; i < supplierSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplierSearch = "'" + supplierSearchArray[i] + "'";
                    }
                    else
                    {
                        supplierSearch = supplierSearch + ",'" + supplierSearchArray[i] + "'";
                    }
                }
            }

            if (statusSearch != null && !"".Equals(statusSearch))
            {
                string[] statusSearchArray = statusSearch.Split(';');

                for (int i = 0; i < statusSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        statusSearch = "'" + statusSearchArray[i] + "'";
                    }
                    else
                    {
                        statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
                    }
                }
            }

            string submissionDateFrom = "";
            string submissionDateTo = "";
            if (submissionDate != null && !"".Equals(submissionDate))
            {
                string[] submissionDateArray = submissionDate.Split('-');
                submissionDateFrom = submissionDateArray[0].Trim();
                submissionDateTo = submissionDateArray[1].Trim();
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CREATED_DATE_FROM = createdDateFrom,
                CREATED_DATE_TO = createdDateTo,
                INVOICE_DATE_FROM = invoiceDateFrom,
                INVOICE_DATE_TO = invoiceDateTo,
                PLAN_PAYMENT_DATE_FROM = planPaymentDateFrom,
                PLAN_PAYMENT_DATE_TO = planPaymentDateTo,
                INVOICE_NO = invoiceNo,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                HARDCOPY_STATUS = statusHardcopySearch,
                SUBMISSION_DATE_FROM = submissionDateFrom,
                SUBMISSION_DATE_TO = submissionDateTo
            };

            List<InvoiceInquiry> result = db.Fetch<InvoiceInquiry>("GetInvoiceInquiryExcel", args);

            db.Close();
            return result;
        }


        public byte[] GenerateDownloadFile(List<InvoiceInquiry> invoiceInquiryList)
        {
            byte[] result = null;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                result = CreateFile(invoiceInquiryList);
            }
            finally
            {
                db.Close();
            }

            return result;
        }


        private byte[] CreateFile(List<InvoiceInquiry> invoiceInquiryList)
        {
            if (invoiceInquiryList == null)
                throw new ArgumentNullException("Data source cannot be null!!!");

            Dictionary<string, string> headers = null;
            //int fieldLength;
            ISheet sheet = null;
            HSSFWorkbook workbook = null;
            byte[] result;
            int startRow = 0;

            workbook = new HSSFWorkbook();

            IDataFormat dataFormat = workbook.CreateDataFormat();
            short dateTimeFormat = dataFormat.GetFormat("dd/MM/yyyy HH:mm:ss");

            ICellStyle cellStyleData = NPOIWriter.createCellStyleData(workbook);
            ICellStyle cellStyleHeader = NPOIWriter.createCellStyleColumnHeader(workbook);
            ICellStyle cellStyleDataDateTime = NPOIWriter.createCellStyleDataDate(workbook, dateTimeFormat);
            ICellStyle cellStyleHeaderDateTime = NPOIWriter.createCellStyleColumnHeader(workbook, dateTimeFormat);
            //ICellStyle cellHolidayStyle = createHolidayCellStyleData(workbook);

            //fieldLength = 5;
            sheet = workbook.CreateSheet(NPOIWriter.EscapeSheetName("Invoice Inquiry"));
            sheet.FitToPage = false;

            // write header manually
            headers = new Dictionary<string, string>();
            //headers.Add("Process ID", processID);


            WriteDetail(workbook, sheet, startRow, cellStyleHeader, cellStyleData, invoiceInquiryList);

            //for (int i = 0; i < fieldLength; i++)
            //{
            //    sheet.AutoSizeColumn(i);
            //}
            //SetColumnSizeLogMonitoringDetail(sheet);

            using (MemoryStream buffer = new MemoryStream())
            {
                workbook.Write(buffer);
                result = buffer.GetBuffer();
            }

            workbook = null;

            return result;
        }


        private void WriteDetail(
        HSSFWorkbook wb,
        ISheet sheet,
        int startRow,
        ICellStyle cellStyleHeader,
        ICellStyle cellStyleData,
        List<InvoiceInquiry> invoiceInquiryList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            int colHeader = 0;
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Invoice No");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Invoice Date");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Currency");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Total AMount");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Amount Before Tax");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Status");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Supplier Code");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Supplier Name");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 1, cellStyleHeader, "Payment Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Plan");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Actual");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Progress Status");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Hardcopy Status");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 1, cellStyleHeader, "Created");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Date");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 1, cellStyleHeader, "Submit");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Date");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 2, cellStyleHeader, "Posting");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Ap Doc No");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Certificate ID");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Payment Doc No");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 2, cellStyleHeader, "Notice");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Problem Description");

            rowIdx = 2;
            foreach (InvoiceInquiry item in invoiceInquiryList)
            {
                WriteDetailSingleData(wb, cellStyleData, item, sheet, ++itemCount, rowIdx++);
            }
        }


        private void WriteDetailSingleData(
          HSSFWorkbook wb,
          ICellStyle cellStyle,
          InvoiceInquiry item,
          ISheet sheet,
          int rowCount,
          int rowIndex)
        {
            IRow row = sheet.CreateRow(rowIndex);
            int startDtlColumnIdx = 11;
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, item.INVOICE_NO);

            NPOIWriter.createCellText(row, cellStyle, col++, item.INVOICE_DATE.HasValue ? item.INVOICE_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.CURRENCY);
            NPOIWriter.createCellDouble(row, cellStyle, col++, item.TOTAL_AMOUNT);

            NPOIWriter.createCellDouble(row, cellStyle, col++, item.TURN_OVER);

            if (item.STATUS != null)
            {
                if (item.STATUS.Equals("ERROR_POSTING"))
                {
                    if (item.AUTO_POSTING_FLAG != null && item.AUTO_POSTING_FLAG.Equals("Y"))
                    {
                        NPOIWriter.createCellText(row, cellStyle, col++, "ERROR_POSTING_A");
                    }
                    else
                    {
                        NPOIWriter.createCellText(row, cellStyle, col++, "ERROR_POSTING_M");
                    }
                }
                // update 19-12-2020 [START]
                else if (item.STATUS.Equals("CREATED"))
                {
                    if (item.NOTICE_FLAG != null && item.NOTICE_FLAG.Equals("Y"))
                    {
                        NPOIWriter.createCellText(row, cellStyle, col++, "NOTICE");
                    }
                    else
                    {
                        NPOIWriter.createCellText(row, cellStyle, col++, "CREATED");
                    }
                }
                // update 19-12-2020 [END]
                else
                {
                    NPOIWriter.createCellText(row, cellStyle, col++, item.STATUS);
                }
            }
            else
            {
                NPOIWriter.createCellText(row, cellStyle, col++, "");
            }

            NPOIWriter.createCellText(row, cellStyle, col++, item.SUPPLIER_CD);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUPPLIER_NAME);

            // update 19-12-2020 [START]
            //NPOIWriter.createCellText(row, cellStyle, col++, item.PAYMENT_PLAN_DATE.HasValue ? item.PAYMENT_PLAN_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.BASE_DATE);
            // update 19-12-2020 [END]

            NPOIWriter.createCellText(row, cellStyle, col++, item.PAYMENT_ACTUAL_DATE.HasValue ? item.PAYMENT_ACTUAL_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PROGRESS_STATUS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.HARDCOPY_STATUS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.CREATED_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.CREATED_DT.HasValue ? item.CREATED_DT.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUBMIT_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUBMIT_DT.HasValue ? item.SUBMIT_DT.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.POSTED_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.POSTED_DT.HasValue ? item.POSTED_DT.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SAP_DOC_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.CERTIFICATE_ID);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PAYMENT_DOC_NO);

            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_DATE.HasValue ? item.NOTICE_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_REMARK);

        }


        public AjaxResult SavePostInvoiceNonDirect(InvoiceInquiry invoice, CREATE_INV_TABLE e, string NoReg, IDBContext db)
        {
            AjaxResult ajaxResult = new AjaxResult();

            double totalAmount = 0;

            DateTime postDate = e.INPUT[0].POST_DATE.Value;
            try
            {
                foreach (CREATE_INV_IN input in e.INPUT)
                {
                    totalAmount = totalAmount + double.Parse(input.AMOUNT);

                    dynamic args = new
                    {
                        INVOICE_ID = invoice.INVOICE_ID,
                        INV_DATE = input.INV_DATE.Value.ToString("yyyy-MM-dd"),
                        POST_DATE = input.POST_DATE.Value.ToString("yyyy-MM-dd"),
                        REF_INV = input.REF_INV,
                        AMOUNT = input.AMOUNT,
                        TAX_AMT = input.TAX_AMT,
                        ITEM_TEXT = input.ITEM_TEXT,
                        PO_NUMBER = input.PO_NUMBER,
                        PO_ITEM = input.PO_ITEM,
                        BASE_DATE = input.BASE_DATE.Value.ToString("yyyy-MM-dd"),
                        PAY_METHOD = input.PAY_METHOD,
                        HEAD_TEXT = input.HEAD_TEXT,
                        TERM_PAY = input.TERM_PAY,
                        //TAX_CODE = input.TAX_CODE,
                        TAX_CODE = invoice.TAX_CODE,
                        BVTYP = input.BVTYP,
                        TAX_DATE = input.TAX_DATE.HasValue ? input.TAX_DATE.Value.ToString("yyyy-MM-dd") : null,
                        CREATED_BY = NoReg
                    };

                    db.Execute("SavePostInvoiceSAPInput", args);
                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                    ajaxResult.SuccMesgs = new String[] { "Sucess Posting" };
                }

                int i = 0;
                if (invoice.PpvAttachmentList != null)
                {
                    foreach (PpvAttachment ppvAttachment in invoice.PpvAttachmentList)
                    {
                        char c = Convert.ToChar(65 + i);

                        dynamic argsPpvAttach = new
                        {
                            INVOICE_ID = invoice.INVOICE_ID,
                            FILE_NAME = ppvAttachment.FILE_NAME,
                            FILE_NAME_SERVER = ppvAttachment.FILE_NAME_SERVER,
                            CREATED_BY = NoReg
                        };
                        db.Execute("InsertPpvAttachment", argsPpvAttach);

                        i++;
                    }
                }


                foreach (CREATE_INV_GR_DATA gr in e.GR_DATA)
                {
                    dynamic args = new
                    {
                        INVOICE_ID = invoice.INVOICE_ID,
                        PO_NUMBER = gr.PO_NUMBER,
                        PO_ITEM = gr.PO_ITEM,
                        GR_NUMBER = gr.GR_NUMBER,
                        GR_YEAR = gr.GR_YEAR,
                        GR_ITEM = gr.GR_ITEM,
                        CREATED_BY = NoReg
                    };

                    db.Execute("SavePostInvoiceSAPGRData", args);
                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                    ajaxResult.SuccMesgs = new String[] { "Sucess Posting" };
                }


                foreach (CREATE_INV_GL_ACCOUNT gl in e.GL_ACCOUNT)
                {
                    dynamic args = new
                    {
                        INVOICE_ID = invoice.INVOICE_ID,
                        GL_ACCOUNT = gl.GL_ACCOUNT,
                        AMOUNT = gl.AMOUNT,
                        SHKZG = gl.SHKZG,
                        CREATED_BY = NoReg
                    };

                    db.Execute("SavePostInvoiceSAPGL", args);
                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                    ajaxResult.SuccMesgs = new String[] { "Sucess Posting" };
                }

                dynamic args3 = new
                {
                    INVOICE_ID = invoice.INVOICE_ID,
                    POSTED_DT = postDate.ToString("yyyy-MM-dd"),
                    POSTED_BY = NoReg,
                    PPV_AMOUNT = invoice.GL_AMOUNT,
                    UPDATED_BY = NoReg,
                    PPV_ACCOUNT = (string.IsNullOrEmpty(invoice.GL_ACCOUNT)) ? "" : invoice.GL_ACCOUNT.Trim(),
                    TOTAL_AMOUNT = totalAmount,
                    TAX_CODE = invoice.TAX_CODE,// add by fid.ahmad 16-03-2022
                    WHT_BASE_AMOUNT = invoice.TURN_OVER,
                    WITHOLDING_TAX_CD = invoice.WITHHOLDING_TAX
                };
                db.Execute("SavePostInvoiceSAP", args3);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.SuccMesgs = new String[] { "Sucess Posting" };

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                        string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                    };
            }

            return ajaxResult;
        }


        public List<Supplier> GetPartnerBankList(string supplierCd, IDBContext db)
        {
            dynamic args = new
            {
                SUPPLIER_CD = supplierCd
            };

            List<Supplier> result = db.Fetch<Supplier>("GetPartnerBankList", args);
            db.Close();

            return result;
        }

        //202007
        public string invoiceWHT(string invoiceNo)
        {
            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                INVOICE_NO = invoiceNo
            };
            string result = db.SingleOrDefault<string>("getWHT", args);
            db.Close();
            return result;

        }

        //get Invoice By Attachment Filename
        public InvoiceInquiry getInvoiceByAttachmentFile(string fileName)
        {
            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                FILE_NAME = fileName
            };

            InvoiceInquiry result = db.SingleOrDefault<InvoiceInquiry>("GetInvoiceByAttachmentFile", args);
            db.Close();

            return result;
        }

        public InvoiceInquiry GetInvoiceDetailByInvIdInvNo(string invoiceId, string invoiceNo)
        {
            SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
            SystemProperty.SystemProperty system = systemPropertyRepo.GetSysPropByCodeAndType("IP_ADDRESS", "INVOICE_INQUIRY");

            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId,
                INVOICE_NO = invoiceNo,
                LinkedServer = system.SYSTEM_VALUE_TEXT
            };

            InvoiceInquiry result = db.SingleOrDefault<InvoiceInquiry>("GetInvoiceDetailByInvIdInvNo", args);
            db.Close();

            return result;
        }
        public void InsertExistingAttachmentToTemp(string invoiceId, string noreg)
        {
            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId,
                CREATED_BY = noreg
            };

            db.Execute("InsertExistingAttachmentToTemp", args);
            db.Close();
        }
        public List<NoticeChat> GetHistoryChat(string invoiceId)
        {
            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId,
            };

            List<NoticeChat> result = db.Fetch<NoticeChat>("GetHistoryChat", args);
            db.Close();

            return result;
        }
        public List<InvoiceAttachment> GetExistingAttachmentFromTemp(string invoiceId)
        {
            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                INVOICE_ID = invoiceId,
            };

            List<InvoiceAttachment> result = db.Fetch<InvoiceAttachment>("GetExistingAttachmentFromTemp", args);
            db.Close();

            return result;
        }
        public void Verify(string invoiceId, string invoiceNo, string updateBy)
        {
            IDBContext db = databaseManager.GetContext();

            //dynamic args = new
            //{
            //    Username = username,
            //    INVOICE_ID = invoiceId
            //};

            // (24-11-2020) update verify proccess with SP

            DateTime start = DateTime.Today.AddDays(7);
            DayOfWeek day = DayOfWeek.Wednesday;

            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            DateTime nextWednesday = start.AddDays(daysToAdd);
            DateTime paymentPlanDate = nextWednesday;

            InvoiceInquiry invoice = GetInvoiceDetailByInvIdInvNo(invoiceId, invoiceNo);

            SqlParameter outputRetVal = new SqlParameter("RetVal", System.Data.SqlDbType.Int);
            outputRetVal.Direction = System.Data.ParameterDirection.Output;
            outputRetVal.Value = -1;

            dynamic args = new
            {
                RetVal = outputRetVal,
                CERTIFICATE_ID = invoice.CERTIFICATE_ID,
                PAYMENT_PLAN_DATE = paymentPlanDate.ToString("yyyy-MM-dd"),
                UPDATED_BY = updateBy
            };

            db.Execute("Verify", args);
            db.Close();
        }
        public void NoticeChat(string invoiceId, string noticeChat, string loginAs, string username, string chatFrom, string chatTo)
        {
            IDBContext db = databaseManager.GetContext();
            dynamic args = new
            {
                CHAT_MESSAGE = noticeChat,
                INVOICE_ID = invoiceId,
                CHAT_FROM = chatFrom,
                CHAT_TO = chatTo,
                CREATED_BY = username
            };

            db.Execute("NoticeChat", args);
            db.Close();
        }
        #region invoice attachment temp
        public AjaxResult insertInvoiceAttachmentTemp(string attachmentType, string invoiceId, string filename, string filenameServer, string createdBy)
        {
            AjaxResult ajaxResult = new AjaxResult();

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                ATTACHMENT_TYPE = attachmentType,
                FILE_NAME = filename,
                FILE_NAME_SERVER = filenameServer,
                CREATED_BY = createdBy,
                INVOICE_ID = invoiceId
            };
            db.Execute("InsertInvoiceAttachmentTemp2", args);

            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.Params = new object[] { filename, filenameServer };
            ajaxResult.SuccMesgs = new String[] { "Upload Attachment " + attachmentType.Replace('_', ' ') + " Success" };

            return ajaxResult;
        }

        public AjaxResult deleteInvoiceAttachmentTemp(string attachmentType, string filename, string createdBy)
        {
            AjaxResult ajaxResult = new AjaxResult();

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                ATTACHMENT_TYPE = attachmentType,
                FILE_NAME = filename,
                CREATED_BY = createdBy
            };
            db.Execute("DeleteInvoiceAttachmentTemp", args);

            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.SuccMesgs = new String[] { "Delete Attachment " + attachmentType.Replace('_', ' ') + " Success" };

            return ajaxResult;
        }

        public string ConvertFileNameAttachment(string attachmentType, string fileName, string supplierCode, string createdBy, string invoiceId)
        {
            string result;
            string attachmentCode;

            fileName = fileName.Replace(' ', '_');

            SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
            SystemProperty.SystemProperty system = systemPropertyRepo.GetSysPropByCodeAndType(attachmentType, CommonConstant.SYSTEM_TYPE_ATTACHMENT_FORMAT_FILENAME);

            attachmentCode = system.SYSTEM_VALUE_TEXT;

            // (24-11-2020) update format filename with invoice id
            result = String.Format("{0}_{1}_{2}_{3}", attachmentCode, supplierCode, invoiceId, fileName);

            //if (attachmentType.ToUpper().Equals("INVOICE"))
            //{
            //    IDBContext db = DatabaseManager.Instance.GetContext();
            //    dynamic args = new
            //    {
            //        SUPPLIER_CD = supplierCode,
            //        CREATED_BY = createdBy,
            //        SEQ_TYPE = "INVOICE ATTACHMENT"
            //    };
            //    string runNo = db.SingleOrDefault<string>("GetSeqNoInvoiceBySupplier", args);
            //    db.Close();

            //    result = String.Format("{0}_{1}_{2}_{3}", attachmentCode, supplierCode, runNo.PadLeft(3, '0'), fileName);
            //}
            //else
            //{
            //    result = String.Format("{0}_{1}_{2}", attachmentCode, supplierCode, fileName);
            //}

            return result;
        }
        #endregion
        public AjaxResult SaveInvoice(IDBContext db, Invoice invoice, string createdBy, List<InvoiceAttachment> attachments, string path, bool isSupplier)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            string tahun = "";
            string bulan = "";
            try
            {
                int i = 0;

                //delete all attachment
                dynamic argsDeleteExistingAttachment = new
                {
                    INVOICE_ID = invoice.INVOICE_ID
                };
                db.Execute("DeleteExistingAttachment", argsDeleteExistingAttachment);

                //insert invoice attachment new
                if (attachments != null)
                {
                    foreach (InvoiceAttachment attachment in attachments)
                    {
                        //get attachment temp
                        dynamic argsAttachmentGet = new
                        {
                            ATTACHMENT_TYPE = attachment.ATTACHMENT_TYPE,
                            FILE_NAME = attachment.FILE_NAME,
                            CREATED_BY = createdBy
                        };
                        InvoiceAttachment attachmentTemp = db.SingleOrDefault<InvoiceAttachment>("GetInvoiceAttachmentTemp", argsAttachmentGet);

                        // check attachment 
                        if (attachmentTemp.FILE_NAME == attachmentTemp.FILE_NAME_SERVER)
                        {
                            // attachment not change

                            //insert new attachment
                            dynamic argsAttachmentInsert = new
                            {
                                INVOICE_ID = invoice.INVOICE_ID,
                                ATTACHMENT_TYPE = attachmentTemp.ATTACHMENT_TYPE,
                                FILE_NAME = attachmentTemp.FILE_NAME,
                                FILE_NAME_SERVER = attachmentTemp.FILE_NAME_SERVER,
                                CREATED_BY = createdBy
                            };
                            db.Execute("InsertInvoiceAttachment", argsAttachmentInsert);
                        }
                        else
                        {
                            // attachment change

                            //convert filename attachment
                            string filenameNew = ConvertFileNameAttachment(attachmentTemp.ATTACHMENT_TYPE, attachmentTemp.FILE_NAME, invoice.SUPPLIER_CD, createdBy, invoice.INVOICE_ID);

                            string[] arrayInvDate = invoice.S_INVOICE_DATE.Split('.');
                            DateTime temp = new DateTime(Int32.Parse(arrayInvDate[2]), Int32.Parse(arrayInvDate[1]), Int32.Parse(arrayInvDate[0]));
                            tahun = temp.ToString("yyyy");
                            bulan = temp.ToString("MMMM");

                            string supplier = invoice.SUPPLIER_CD;
                            string addPath = Path.Combine(tahun, bulan.ToUpper(), supplier);
                            string newPath = Path.Combine(path, addPath);
                            System.IO.Directory.CreateDirectory(newPath);

                            string sourceFile = Path.Combine(path, attachmentTemp.FILE_NAME_SERVER);
                            string destFile = Path.Combine(newPath, filenameNew);

                            var sourceDirectory = new FileInfo(sourceFile);
                            var destDirectory = new FileInfo(destFile);
                            if (sourceDirectory.Exists)
                            {
                                if (destDirectory.Exists)
                                {
                                    System.IO.File.Delete(destFile);
                                }
                                System.IO.File.Move(sourceFile, destFile);

                                //insert new attachment
                                dynamic argsAttachmentInsert = new
                                {
                                    INVOICE_ID = invoice.INVOICE_ID,
                                    ATTACHMENT_TYPE = attachmentTemp.ATTACHMENT_TYPE,
                                    FILE_NAME = filenameNew,
                                    FILE_NAME_SERVER = filenameNew,
                                    CREATED_BY = createdBy
                                };
                                db.Execute("InsertInvoiceAttachment", argsAttachmentInsert);
                            }
                        }
                    }

                    //delete all attachment temp
                    dynamic argsAttachmentDelete = new
                    {
                        INVOICE_ID = invoice.INVOICE_ID
                    };
                    db.Execute("DeleteInvoiceAttachmentTemp2", argsAttachmentDelete);

                    //Update Notice Flag (khusus supplier)
                    if (isSupplier)
                    {
                        dynamic argsUpdateNoticeFlag = new
                        {
                            INVOICE_ID = invoice.INVOICE_ID,
                            CREATED_BY = createdBy
                        };
                        db.Execute("UpdateNoticeFlag", argsUpdateNoticeFlag);
                    }

                }

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.Params = new String[] { invoice.INVOICE_ID, invoice.SUPPLIER_CD, invoice.CERTIFICATE_ID };
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }

            return ajaxResult;
        }

        public AjaxResult deleteAllInvoiceAttachmentTemp(string invoiceId)
        {
            AjaxResult ajaxResult = new AjaxResult();

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic argsAttachmentDelete = new
            {
                INVOICE_ID = invoiceId
            };
            db.Execute("DeleteInvoiceAttachmentTemp2", argsAttachmentDelete);

            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.SuccMesgs = new String[] { "Delete Temporary Attachment Success" };

            return ajaxResult;
        }

        public string GetTaxCode( string InvoiceNo, string SupplierCode)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            string TaxCode = "I2";

            try
            {
                dynamic args = new
                {
                    INVOICE_NO = InvoiceNo,
                    SUPPLIER_CD = SupplierCode
                };

                TaxCode = db.ExecuteScalar<string>("GetTaxCode", args);

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }

            return TaxCode;
        }

    }
}