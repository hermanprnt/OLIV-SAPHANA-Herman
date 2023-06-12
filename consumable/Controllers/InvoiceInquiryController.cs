using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using consumable.Common.Data;
using consumable.Commons.Constants;
using System.Web.Script.Serialization;
using consumable.Commons.Helpers;
//using consumable.Logic;
using consumable.Models;
using consumable.Models.GLAccount;
using consumable.Models.InvoiceCreation;
using consumable.Models.InvoiceInquiry;
using consumable.Models.Message;
using consumable.Models.Paging;
using consumable.Models.ReportView;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;
using consumable.Models.WithholdingTax;
using Toyota.Common.Credential;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class InvoiceInquiryController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private InvoiceInquiryRepository invoiceInquiryRepo = InvoiceInquiryRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private WithholdingTaxRepository withholdingTaxRepo = WithholdingTaxRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private GLAccountRepository glAccountRepo = GLAccountRepository.Instance;
        private MessageRepository messageRepo = MessageRepository.Instance;


        public static readonly string BARCODE_START_MARK = "*";
        public static readonly string BARCODE_END_MARK = "*";

        protected override void Startup()
        {
            string username = Lookup.Get<Toyota.Common.Credential.User>().Username;

            var role = Lookup.Get<Toyota.Common.Credential.User>();

            Settings.Title = "Invoice Inquiry";

            //Toyota.Common.Credential.User u = Lookup.Get<Toyota.Common.Credential.User>();
            //var role1 = from role2 in u.Roles
            //              where role2.Id == "Liv_Admin"
            //              select role2.Id;

            string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
            ViewData["PartnerBankList"] = new List<Supplier>();

            ViewData["vendCodeLogin"] = vendCodeLogin;
            if (vendCodeLogin != null)
            {
                ViewData["readonly"] = "readonly";
                ViewData["disabled"] = "disabled";

                ViewData["visibilityUser"] = "false";
                ViewData["visibilityVndr"] = "true";
            }
            else
            {
                ViewData["readonly"] = "";
                ViewData["disabled"] = "";

                ViewData["visibilityUser"] = "true";
                ViewData["visibilityVndr"] = "false";
            }
            List<GLAccount> listWitholdingTax = (List<GLAccount>)glAccountRepo.GetGLAccountByCategory(CommonConstant.WITHOLDING_TAX);
            ViewData["WitholdingTaxList"] = listWitholdingTax;

            //Get attachment doc type from tb m system
            List<SystemProperty> listAttachmentDocType = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_ATTACHMENT);
            ViewData["AttachmentDocTypeList"] = listAttachmentDocType;
            constructComboBoxes();
            // remark by fid.ahmad 05-04-2022
            SystemProperty INIT_DATE_MONTH_RANGE = systemPropertyRepo.GetSysPropByCodeAndType(
                                CommonConstant.SYSTEM_CD_RANGE_SUBMISSION_DATE_SEARCH, CommonConstant.SYSTEM_TYPE_INVOICE_INQUIRY);

            DateTime now = DateTime.Now;
            // now = new DateTime(now.Year, 1, 23);//testing January is current
            var addMonth = (INIT_DATE_MONTH_RANGE == null) ? 1 : INIT_DATE_MONTH_RANGE.SYSTEM_VALUE_NUM;
            var startDate = now.AddMonths(-addMonth);
            var submissionDateString = startDate.ToString("dd.MM.yyyy") + " - " + now.ToString("dd.MM.yyyy");

            getListInvoiceInquiry("", submissionDateString, vendCodeLogin, "", "", "", "", "",
                CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());

            ViewData["DefaultSubmissionDate"] = submissionDateString;
            //add by fid.ahmad 17-01-2023
            string RegNo = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
            ViewData["RegNoByUserLogin"] = RegNo;
        }

        public void constructComboBoxes()
        {
            List<SystemProperty> statusInvoiceList = new List<SystemProperty>();
            List<SystemProperty> listSystemProperty = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_STATUS_INVOICE);
            List<SystemProperty> listHardCopyStatus = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_STATUS_HARDCOPY);

            foreach (SystemProperty sysProp in listSystemProperty)
            {
                if (!sysProp.SYSTEM_CD.Equals("ERROR_POSTING"))
                {
                    statusInvoiceList.Add(sysProp);
                }
            }

            ViewData["StatusInvoiceList"] = statusInvoiceList;
            ViewData["StatusHardCopyList"] = listHardCopyStatus;

            //ViewData["Suppliers"] = (List<Supplier>)supplierRepo.GetSupplier("A", 1, 10);
            getLookupSupplierSearch(
                CommonFunction.Instance.DefaultStringValue(),
                CommonFunction.Instance.DefaultPage(),
                CommonFunction.Instance.DefaultSize());
            //ViewData["WithholdingTaxes"] = (List<WithholdingTax>)withholdingTaxRepo.GetWitholdingTax("A", 1, 10);

            List<SystemProperty> listTermPaym = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.TERM_PAY);
            ViewData["TermPaymList"] = listTermPaym;

            List<SystemProperty> listPaymMethod = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.PAY_METHOD);
            ViewData["PaymMethodList"] = listPaymMethod;

            List<GLAccount> listWitholdingTax = (List<GLAccount>)glAccountRepo.GetGLAccountByCategory(CommonConstant.WITHOLDING_TAX);
            ViewData["WitholdingTaxList"] = listWitholdingTax;

            List<GLAccount> listGlAccount = (List<GLAccount>)glAccountRepo.GetGLAccountByCategory(CommonConstant.GL_ACCOUNT);
            ViewData["GlAccountList"] = listGlAccount;

            List<SystemProperty> listTaxCode = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.TAX_CODE);
            ViewData["TaxCodeList"] = listTaxCode;

            //add by fid.ahmad 16-03-2022
            List<SystemProperty> defaultTaxCode = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.DEFAULT_TAX_CODE);
            if (defaultTaxCode.Count() > 0)
            {
                ViewBag.DefaultTaxCode = defaultTaxCode[0].SYSTEM_VALUE_TEXT;
            }
            else
            {
                ViewBag.DefaultTaxCode = "I2";
            }
            //end by fid.ahmad
            //add by riani (20220426)-->config default tax (this case 11%) and special tax (this case 0%)            
            string specialtaxcalculates = "";
            SystemProperty sysPropsp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType("TAX_CALCULATE", "SPECIAL_TAX");
            specialtaxcalculates = sysPropsp.SYSTEM_VALUE_TEXT;
            ViewData["specialtaxcalculates"] = specialtaxcalculates;

            string defaulttaxcalculates = "";
            SystemProperty sysPropdf =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType("TAX_CALCULATE", "DEFAULT_TAX");
            defaulttaxcalculates = sysPropdf.SYSTEM_VALUE_TEXT;
            ViewData["defaulttaxcalculates"] = defaulttaxcalculates;
            SystemProperty sysPropTaxCal =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType("TAX_CALCULATE", "TAX_CALCULATE_CD");
            ViewData["TaxCalList"] = sysPropTaxCal.SYSTEM_VALUE_TEXT.Split(';').ToList();
            //

        }

        //add by fid.ahmad 04-10-2022
        public void constructComboBoxesTaxCode(string InvoiceNo, string SupplierCode)
        {
            List<SystemProperty> listTaxCode = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.TAX_CODE);
            ViewData["TaxCodeList"] = listTaxCode;
            string TaxCode = invoiceInquiryRepo.GetTaxCode(InvoiceNo, SupplierCode);
            ViewBag.DefaultTaxCode = TaxCode;
        }

        public ActionResult onLookupSupplier(string Parameter, string PartialViewSearchAndInput, int Page)
        {
            if (PartialViewSearchAndInput.Equals("_LookupTableSupplier"))
            {
                getLookupSupplierSearch(
                    Parameter,
                    Page,
                    CommonFunction.Instance.DefaultSize());
            }

            return PartialView(PartialViewSearchAndInput);
        }
        public JsonResult GetInvoiceDetailByInvIdInvNo(string invoiceId, string invoiceNo)
        {
            AjaxResult results = new AjaxResult();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                InvoiceInquiry data = new InvoiceInquiry();
                data = invoiceInquiryRepo.GetInvoiceDetailByInvIdInvNo(invoiceId, invoiceNo);


                //string companyId = Lookup.Get<Toyota.Common.Credential.User>().Company.Id;
                //List<SystemProperty> listCompanyCodeTmmin = systemPropertyRepo.GetBySystemPropertyType("TMMIN_COMPANY_CD");
                //bool isSupplier = !listCompanyCodeTmmin.Select(x => x.SYSTEM_CD).Contains(companyId);
                //string userLoginAs = isSupplier ? "Supplier" : "Finance";

                bool isVendor = Lookup.Get<Toyota.Common.Credential.User>().Roles.Select(x => x.Id).Contains("Liv_Vendor");
                string userLoginAs = isVendor ? "Supplier" : "Finance";

                if (data != null)
                {
                    invoiceInquiryRepo.InsertExistingAttachmentToTemp(invoiceId, NoReg);
                    data.InvoiceAttachment = invoiceInquiryRepo.GetExistingAttachmentFromTemp(invoiceId) ?? null;
                    data.HistoryChat = invoiceInquiryRepo.GetHistoryChat(invoiceId);
                    data.ROLE = userLoginAs;
                }

                results.Data = data;
                results.Result = AjaxResult.VALUE_SUCCESS;
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            return Json(results);
        }
        public JsonResult Verify(string invoiceId, string invoiceNo)
        {
            AjaxResult results = new AjaxResult();

            try
            {
                string username = Lookup.Get<Toyota.Common.Credential.User>().Username;
                invoiceInquiryRepo.Verify(invoiceId, invoiceNo, username);
                results.Result = AjaxResult.VALUE_SUCCESS;
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
               string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            return Json(results);
        }
        //add riani (20220425)--> system master untuk tax calculate
        public JsonResult getTaxCalculate()
        {
            string taxcalculates = "";
            SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType("TAX_CALCULATE", "TAX_CALCULATE_CD");
            taxcalculates = sysProp.SYSTEM_VALUE_TEXT;

            if (taxcalculates != null)
            {
                return Json(taxcalculates.Split(';').ToList());
            }
            else
            {
                return null;
            }

        }
        public JsonResult NoticeChat(string invoiceId, string invoiceNo, string noticeChat)
        {
            AjaxResult results = new AjaxResult();

            try
            {

                string companyId = Lookup.Get<Toyota.Common.Credential.User>().Company.Id;
                string username = Lookup.Get<Toyota.Common.Credential.User>().Username;

                //cara 1
                //List<SystemProperty> listCompanyCodeTmmin = systemPropertyRepo.GetBySystemPropertyType("TMMIN_COMPANY_CD");
                //bool isSupplier = !listCompanyCodeTmmin.Select(x=>x.SYSTEM_CD).Contains(companyId);
                //string userLoginAs = isSupplier? "Supplier" : "Finance";
                string vendor = "";
                //cara 2
                bool isVendor = Lookup.Get<Toyota.Common.Credential.User>().Roles.Select(x => x.Id).Contains("Liv_Vendor");
                string userLoginAs = isVendor ? "Supplier" : "Finance";


                if (isVendor)
                {
                    string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
                    Supplier supp = supplierRepo.GetBySupplierCd(vendCodeLogin);
                    //vendor = supp?.SUPPLIER_CD + "_" + supp?.SUPPLIER_NAME.Substring(0, 15);
                    vendor = supp.SUPPLIER_CD + "_" + (supp.SUPPLIER_NAME.Length > 15 ? supp.SUPPLIER_NAME.Substring(0, 15) : supp.SUPPLIER_NAME);

                }
                else
                {
                    InvoiceInquiry invoice = invoiceInquiryRepo.GetInvoiceDetailByInvIdInvNo(invoiceId, invoiceNo);
                    Supplier supp = supplierRepo.GetBySupplierCd(invoice.SUPPLIER_CD);
                    //vendor = supp?.SUPPLIER_CD + "_" + supp?.SUPPLIER_NAME.Substring(0, 15);
                    vendor = supp.SUPPLIER_CODE + "_" + (supp.SUPPLIER_NAME.Length > 15 ? supp.SUPPLIER_NAME.Substring(0, 15) : supp.SUPPLIER_NAME);

                }

                SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType("INVOICE_INQUIRY", "FINANCE_NOTICE_LABEL");
                string finance = sysProp == null ? "Finance" : sysProp.SYSTEM_VALUE_TEXT;
                string chatFrom = isVendor ? vendor : finance;
                string chatTo = isVendor ? finance : vendor;
                invoiceInquiryRepo.NoticeChat(invoiceId, noticeChat, userLoginAs, username, chatFrom, chatTo);



                results.Data = invoiceInquiryRepo.GetHistoryChat(invoiceId);
                results.Role = userLoginAs;
                results.Result = AjaxResult.VALUE_SUCCESS;
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
               string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            return Json(results);
        }

        private void getLookupSupplierSearch(string parameter, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier(parameter), page, size);

            ViewData["LookupPaging"] = pg;

            ViewData["Suppliers"] = supplierRepo.GetSupplier(parameter, pg.StartData, pg.EndData);
        }

        public ActionResult search(string createdDateSearch, string submissionDateSearch, string supplierSearch, string invoiceDateSearch, string statusSearch, string statusHardcopySearch,
            string planPaymentDateSearch, string invoiceNoSearch, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListInvoiceInquiry(createdDateSearch, submissionDateSearch, supplierSearch, invoiceDateSearch, statusSearch, statusHardcopySearch, planPaymentDateSearch, invoiceNoSearch,
                page, size);

            return PartialView("_GridView");
        }

        private void getListInvoiceInquiry(string createdDate, string submissionDate, string supplier, string invoiceDate, string status, string statusHardcopy,
            string planPaymentDate, string invoiceNo, int page, int size)
        {
            int countdata = 0;
            countdata = invoiceInquiryRepo.countInvoiceInquiry(createdDate, submissionDate, supplier,
                invoiceDate, status, statusHardcopy, planPaymentDate, invoiceNo);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<InvoiceInquiry> invoiceInquiryList = invoiceInquiryRepo.GetInvoiceInquiryList(createdDate, submissionDate,
                supplier, invoiceDate, status, statusHardcopy, planPaymentDate, invoiceNo,
                pg.StartData, pg.EndData);
            ViewData["InvoiceInquiry"] = invoiceInquiryList;
        }

        [HttpGet]
        public ContentResult GetInvoiceInquirySort(string createdDateSearch, string submissionDateSearch, string invoiceDateSearch, string supplierSearch,
            string planPaymentDateSearch, string invoiceNoSearch, string statusSearch, string statusHardcopySearch, int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = invoiceInquiryRepo.countInvoiceInquiry(createdDateSearch, submissionDateSearch, supplierSearch,
                invoiceDateSearch, statusSearch, statusHardcopySearch, planPaymentDateSearch, invoiceNoSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<String> result = new List<String>();
            result = invoiceInquiryRepo.GetInvoiceInquirySort(createdDateSearch, submissionDateSearch, supplierSearch,
                invoiceDateSearch, statusSearch, statusHardcopySearch, planPaymentDateSearch, invoiceNoSearch, pg.StartData, pg.EndData,
                field, sort);

            return Content(String.Join("", result.ToArray()));
        }

        public FileResult PrintCertificate(string invoiceId, string supplierCode, string certificateId)
        {
            string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
            if (vendCodeLogin != null)
            {
                if (!vendCodeLogin.Equals(supplierCode))
                {
                    return null;
                }
            }

            if (certificateId == null || invoiceId == null || supplierCode == null)
            {
                return null;
            }

            ReportViewModel reportViewModel = consumable.Commons.PrintReport.Instance.GetReportViewModelInvoice(invoiceId, supplierCode, certificateId);
            if (reportViewModel == null)
            {
                return null;
            }

            byte[] renderedBytes = reportViewModel.RenderReport();
            //renderedBytes = this.ConvertPdfBecomeSilentPrint(renderedBytes);

            if (reportViewModel.ViewAsAttachment)
            {
                Response.AddHeader("content-disposition", reportViewModel.ReportExportFileName);
            }

            return File(renderedBytes, reportViewModel.LastMimeType);

        }

        public ActionResult GetInvoiceInquiryDetail(string invoiceId)
        {
            ViewData["InvoiceInquiryDetail"] = invoiceInquiryRepo.GetInvoiceInquiryDetail(invoiceId);

            return PartialView("_InvoiceInquiryDetailPopUp");
        }


        public ActionResult GetInvoiceInquiryError(string invoiceId)
        {
            ViewData["InvoiceInquiryErrorDetail"] = invoiceInquiryRepo.GetInvoiceInquiryDetailError(invoiceId);

            return PartialView("_InvoiceInquiryDetailErrorPopUp");
        }


        public ActionResult ShowFileDownload(string invoiceId)
        {
            ViewData["ListFileDownload"] = invoiceInquiryRepo.GetListFileDownload(invoiceId);

            return PartialView("_FileDownloadPopUp");
        }

        public ActionResult ShowFilePpvAttachement(string invoiceId)
        {
            ViewData["ListFilePpvDownload"] = invoiceInquiryRepo.GetListFilePpvDownload(invoiceId);

            return PartialView("_FileDownloadPpvPopUp");
        }

        public JsonResult process(List<InvoiceInquiry> invoiceInquiry)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                //SAPLogic sap = new SAPLogic();

                for (int j = 0; j < invoiceInquiry.Count; j++)
                {
                    if (CommonConstant.PROCESS_TYPE_CANCEL_INVOICE.Equals(invoiceInquiry[0].PROCESS_TYPE))
                    {

                        results = invoiceInquiryRepo.cancelInvoice(db, invoiceInquiry[j].INVOICE_ID,
                            invoiceInquiry[j].TAX_INVOICE_NO, NoReg);
                    }

                    else if (CommonConstant.PROCESS_TYPE_POST_INVOICE.Equals(invoiceInquiry[0].PROCESS_TYPE))
                    {
                        CREATE_INV_TABLE e = new CREATE_INV_TABLE();
                        //string SHKZG = "";
                        Double stampAmount = 0;

                        List<InvoiceInquiryDetail> details = invoiceInquiryRepo
                            .GetPOForPosting(invoiceInquiry[j].INVOICE_ID);

                        // untuk masing-masing GR
                        List<InvoiceInquiryDetail> detailsGR = invoiceInquiryRepo
                           .GetInvoiceInquiryDetail(invoiceInquiry[j].INVOICE_ID);

                        Double withholdingTaxAmount = 0;
                        GLAccount witholdingTax = new GLAccount();
                        GLAccount stamp = new GLAccount();

                        // WITHOLDING
                        if (invoiceInquiry[j].WITHHOLDING_TAX != null && !"".Equals(invoiceInquiry[j].WITHHOLDING_TAX.Trim()))
                        {
                            witholdingTax = glAccountRepo.GetGLAccountByCode(invoiceInquiry[j].WITHHOLDING_TAX);
                            withholdingTaxAmount = (witholdingTax.PERCENTAGE / 100 * invoiceInquiry[j].TURN_OVER);
                        }

                        if (details != null && details.Count > 0)
                        {
                            e.INPUT = new List<CREATE_INV_IN>();
                            int i = 0;
                            foreach (InvoiceInquiryDetail item in details)
                            {
                                CREATE_INV_IN create_inv_in = new CREATE_INV_IN();
                                create_inv_in.INV_DATE = DateTime.ParseExact(invoiceInquiry[j].INVOICE_DATE_STR, "dd.MM.yyyy", null);
                                create_inv_in.POST_DATE = DateTime.ParseExact(invoiceInquiry[j].POSTED_DT_STR, "dd.MM.yyyy", null);
                                create_inv_in.REF_INV = invoiceInquiry[j].INVOICE_NO;

                                if (i == 0)
                                {
                                    create_inv_in.AMOUNT = (item.PO_TOTAL_AMOUNT + item.TAX_INVOICE_AMOUNT + item.STAMP_AMOUNT - withholdingTaxAmount).ToString();
                                    create_inv_in.TAX_AMT = (item.TAX_INVOICE_AMOUNT).ToString();
                                    //create_inv_in.AMOUNT = (item.PO_TOTAL_AMOUNT + (item.PO_TOTAL_AMOUNT * (item.CALCULATE_TAX / 100))
                                    //    + item.STAMP_AMOUNT - withholdingTaxAmount).ToString();
                                    //create_inv_in.AMOUNT = (item.MATDOC_AMOUNT + (item.MATDOC_AMOUNT * (item.CALCULATE_TAX / 100))
                                    //    + item.STAMP_AMOUNT - withholdingTaxAmount).ToString();
                                }
                                else
                                {
                                    create_inv_in.AMOUNT = item.PO_TOTAL_AMOUNT.ToString();
                                    create_inv_in.TAX_AMT = "0";
                                    //create_inv_in.AMOUNT = (item.PO_TOTAL_AMOUNT + (item.PO_TOTAL_AMOUNT * (item.CALCULATE_TAX / 100))).ToString();
                                    //create_inv_in.AMOUNT = (item.MATDOC_AMOUNT + (item.MATDOC_AMOUNT * (item.CALCULATE_TAX / 100))).ToString();
                                }

                                //create_inv_in.TAX_AMT = (item.PO_TOTAL_AMOUNT * (item.CALCULATE_TAX / 100)).ToString();
                                //create_inv_in.TAX_AMT = (item.MATDOC_AMOUNT * (item.CALCULATE_TAX / 100)).ToString();
                                create_inv_in.ITEM_TEXT = invoiceInquiry[j].INVOICE_NOTE;
                                create_inv_in.PO_NUMBER = item.PO_NUMBER;
                                create_inv_in.PO_ITEM = item.PO_ITEM; ;
                                create_inv_in.BASE_DATE = DateTime.ParseExact(invoiceInquiry[j].BASE_DATE_STR, "dd.MM.yyyy", null);
                                create_inv_in.PAY_METHOD = invoiceInquiry[j].PAY_METHOD;
                                create_inv_in.HEAD_TEXT = invoiceInquiry[j].TAX_INVOICE_NO;
                                create_inv_in.TERM_PAY = invoiceInquiry[j].TERM_PAY;
                                create_inv_in.TAX_CODE = invoiceInquiry[j].TAX_CODE;
                                create_inv_in.BVTYP = invoiceInquiry[j].PARTNER_BANK ?? "";

                                if (invoiceInquiry[j].TAX_INVOICE_DT_STR != null && !"".Equals(invoiceInquiry[j].TAX_INVOICE_DT_STR.Trim()))
                                {
                                    create_inv_in.TAX_DATE = DateTime.ParseExact(invoiceInquiry[j].TAX_INVOICE_DT_STR, "dd.MM.yyyy", null);
                                }
                                //SystemProperty sysProp = (SystemProperty)systemPropertyRepo
                                //    .GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_TAX_CODE)[0];
                                //create_inv_in.TAX_CODE = sysProp.SYSTEM_CD;

                                e.INPUT.Add(create_inv_in);

                                if (item.STAMP_AMOUNT > 0)
                                {
                                    stamp = glAccountRepo.GetGLAccountByCategory(CommonConstant.STAMP)[0];
                                    stampAmount = item.STAMP_AMOUNT;
                                }

                                i++;
                            }
                        }


                        if (detailsGR != null && detailsGR.Count > 0)
                        {
                            e.GR_DATA = new List<CREATE_INV_GR_DATA>();
                            foreach (InvoiceInquiryDetail item in detailsGR)
                            {
                                CREATE_INV_GR_DATA grData = new CREATE_INV_GR_DATA();
                                grData.PO_NUMBER = item.PO_NUMBER;
                                grData.PO_ITEM = item.PO_ITEM;
                                grData.GR_NUMBER = item.MATDOC_NUMBER;
                                grData.GR_YEAR = item.MATDOC_YEAR;
                                grData.GR_ITEM = item.MATDOC_ITEM;
                                e.GR_DATA.Add(grData);
                            }
                        }

                        // STAMP
                        if (stampAmount > 0)
                        {
                            CREATE_INV_GL_ACCOUNT create_inv_gl1 = new CREATE_INV_GL_ACCOUNT();
                            create_inv_gl1.GL_ACCOUNT = stamp.GL_ACCOUNT_NO.Trim();
                            create_inv_gl1.AMOUNT = stampAmount.ToString();
                            create_inv_gl1.SHKZG = stamp.TYPE;
                            e.GL_ACCOUNT.Add(create_inv_gl1);
                        }

                        // WITHOLDING
                        if (invoiceInquiry[j].WITHHOLDING_TAX != null && !"".Equals(invoiceInquiry[j].WITHHOLDING_TAX.Trim()))
                        {
                            CREATE_INV_GL_ACCOUNT create_inv_gl2 = new CREATE_INV_GL_ACCOUNT();
                            create_inv_gl2.GL_ACCOUNT = witholdingTax.GL_ACCOUNT_NO.Trim(); // "2140030";
                            create_inv_gl2.AMOUNT = (witholdingTax.PERCENTAGE / 100 * invoiceInquiry[j].TURN_OVER).ToString();
                            create_inv_gl2.SHKZG = witholdingTax.TYPE;
                            e.GL_ACCOUNT.Add(create_inv_gl2);
                        }


                        // GL ACCOUNT
                        if (invoiceInquiry[j].GL_ACCOUNT != null && !"".Equals(invoiceInquiry[j].GL_ACCOUNT.Trim()))
                        {
                            CREATE_INV_GL_ACCOUNT create_inv_gl3 = new CREATE_INV_GL_ACCOUNT();
                            create_inv_gl3.GL_ACCOUNT = invoiceInquiry[j].GL_ACCOUNT.Trim();
                            create_inv_gl3.AMOUNT = invoiceInquiry[j].GL_AMOUNT;
                            create_inv_gl3.SHKZG = "H";
                            //create_inv_gl1.GL_ACCOUNT = "2140030";
                            //create_inv_gl1.AMOUNT = "3000";
                            e.GL_ACCOUNT.Add(create_inv_gl3);
                        }

                        //results = sap.CreateInv(e, items[j], NoReg);
                        results = invoiceInquiryRepo.SavePostInvoiceNonDirect(invoiceInquiry[j], e, NoReg, db);
                    }

                    else if (CommonConstant.PROCESS_TYPE_REVERSE_INVOICE.Equals(invoiceInquiry[0].PROCESS_TYPE))
                    {
                        CANCEL_INV_TABLE e = new CANCEL_INV_TABLE();
                        e.INPUT = new List<CANCEL_INV_IN>();
                        CANCEL_INV_IN cancel_inv_in = new CANCEL_INV_IN();
                        cancel_inv_in.INVOICE_NO = invoiceInquiry[j].LOG_DOC_NO;
                        cancel_inv_in.YEAR = invoiceInquiry[j].SAP_YEAR;

                        //SystemProperty sysProp = (SystemProperty)systemPropertyRepo
                        //           .GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_REVERSE_REASON)[0];
                        //cancel_inv_in.REASON = sysProp.SYSTEM_CD;
                        cancel_inv_in.REASON = invoiceInquiry[j].REVERSE_REASON;
                        cancel_inv_in.POST_DATE = DateTime.ParseExact(invoiceInquiry[j].REVERSE_POST_DT_STRING, "dd.MM.yyyy", null);
                        String reverseRemarks = invoiceInquiry[j].REVERSE_REMARKS;

                        e.INPUT.Add(cancel_inv_in);

                        //SAP Direct
                        //e = sap.CancelInv(e);
                        //results = invoiceInquiryRepo.SaveReverseInvoiceDirect(db, e, items[0].INVOICE_ID,
                        //        items[0].TAX_INVOICE_NO, NoReg);

                        //SAP Non Direct
                        results = invoiceInquiryRepo.SaveReverseInvoiceNonDirect(db, e, invoiceInquiry[0].INVOICE_ID,
                               invoiceInquiry[0].TAX_INVOICE_NO, NoReg);
                    }

                    if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                    {
                        db.AbortTransaction();
                    }
                    else
                    {
                        db.CommitTransaction();

                        if (CommonConstant.PROCESS_TYPE_CANCEL_INVOICE.Equals(invoiceInquiry[0].PROCESS_TYPE))
                        {
                            invoiceInquiryRepo.updateUsedFlagEFakturCancelInvoice(db,
                            invoiceInquiry[j].TAX_INVOICE_NO, NoReg);

                            //FID.Ridwan: 20220719
                            invoiceInquiryRepo.DeleteInvoiceFromPAS(invoiceInquiry[j].INVOICE_ID);

                        }


                    }
                }

            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            finally
            {
                db.Close();
            }

            return Json(results);
        }

        public JsonResult processPost(InvoiceInquiry invoiceInquiry, List<HttpPostedFileBase> file)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                //SAPLogic sap = new SAPLogic();
                if (CommonConstant.PROCESS_TYPE_POST_INVOICE.Equals(invoiceInquiry.PROCESS_TYPE))
                {
                    CREATE_INV_TABLE e = new CREATE_INV_TABLE();
                    //string SHKZG = "";
                    Double stampAmount = 0;
                    string glAmount = "";

                    List<InvoiceInquiryDetail> details = invoiceInquiryRepo
                        .GetPOForPosting(invoiceInquiry.INVOICE_ID);

                    // untuk masing-masing GR
                    List<InvoiceInquiryDetail> detailsGR = invoiceInquiryRepo
                       .GetInvoiceInquiryDetail(invoiceInquiry.INVOICE_ID);

                    Double withholdingTaxAmount = 0;
                    GLAccount witholdingTax = new GLAccount();
                    GLAccount stamp = new GLAccount();

                    // WITHOLDING
                    if (invoiceInquiry.WITHHOLDING_TAX != null && !"".Equals(invoiceInquiry.WITHHOLDING_TAX.Trim()))
                    {
                        witholdingTax = glAccountRepo.GetGLAccountByCode(invoiceInquiry.WITHHOLDING_TAX);
                        withholdingTaxAmount = (witholdingTax.PERCENTAGE / 100 * invoiceInquiry.TURN_OVER);
                    }

                    if (details != null && details.Count > 0)
                    {
                        e.INPUT = new List<CREATE_INV_IN>();
                        int i = 0;
                        foreach (InvoiceInquiryDetail item in details)
                        {
                            CREATE_INV_IN create_inv_in = new CREATE_INV_IN();
                            create_inv_in.INV_DATE = DateTime.ParseExact(invoiceInquiry.INVOICE_DATE_STR, "dd.MM.yyyy", null);
                            create_inv_in.POST_DATE = DateTime.ParseExact(invoiceInquiry.POSTED_DT_STR, "dd.MM.yyyy", null);
                            create_inv_in.REF_INV = invoiceInquiry.INVOICE_NO;

                            glAmount = (string.IsNullOrEmpty(invoiceInquiry.GL_AMOUNT)) ? "0" : invoiceInquiry.GL_AMOUNT;

                            if (i == 0)
                            {
                                create_inv_in.AMOUNT = (item.PO_TOTAL_AMOUNT + item.TAX_INVOICE_AMOUNT + item.STAMP_AMOUNT - withholdingTaxAmount + double.Parse(glAmount)).ToString();
                                create_inv_in.TAX_AMT = (item.TAX_INVOICE_AMOUNT).ToString();
                                //create_inv_in.AMOUNT = (item.PO_TOTAL_AMOUNT + (item.PO_TOTAL_AMOUNT * (item.CALCULATE_TAX / 100))
                                //    + item.STAMP_AMOUNT - withholdingTaxAmount).ToString();
                                //create_inv_in.AMOUNT = (item.MATDOC_AMOUNT + (item.MATDOC_AMOUNT * (item.CALCULATE_TAX / 100))
                                //    + item.STAMP_AMOUNT - withholdingTaxAmount).ToString();
                            }
                            else
                            {
                                create_inv_in.AMOUNT = item.PO_TOTAL_AMOUNT.ToString();
                                create_inv_in.TAX_AMT = "0";
                                //create_inv_in.AMOUNT = (item.PO_TOTAL_AMOUNT + (item.PO_TOTAL_AMOUNT * (item.CALCULATE_TAX / 100))).ToString();
                                //create_inv_in.AMOUNT = (item.MATDOC_AMOUNT + (item.MATDOC_AMOUNT * (item.CALCULATE_TAX / 100))).ToString();
                            }

                            //create_inv_in.TAX_AMT = (item.PO_TOTAL_AMOUNT * (item.CALCULATE_TAX / 100)).ToString();
                            //create_inv_in.TAX_AMT = (item.MATDOC_AMOUNT * (item.CALCULATE_TAX / 100)).ToString();
                            create_inv_in.ITEM_TEXT = invoiceInquiry.INVOICE_NOTE;
                            create_inv_in.PO_NUMBER = item.PO_NUMBER;
                            create_inv_in.PO_ITEM = item.PO_ITEM; ;
                            create_inv_in.BASE_DATE = DateTime.ParseExact(invoiceInquiry.BASE_DATE_STR, "dd.MM.yyyy", null);
                            create_inv_in.PAY_METHOD = invoiceInquiry.PAY_METHOD;
                            create_inv_in.HEAD_TEXT = invoiceInquiry.TAX_INVOICE_NO;
                            create_inv_in.TERM_PAY = invoiceInquiry.TERM_PAY;
                            create_inv_in.TAX_CODE = invoiceInquiry.TAX_CODE;
                            create_inv_in.BVTYP = invoiceInquiry.PARTNER_BANK ?? "";

                            if (invoiceInquiry.TAX_INVOICE_DT_STR != null && !"".Equals(invoiceInquiry.TAX_INVOICE_DT_STR.Trim()))
                            {
                                create_inv_in.TAX_DATE = DateTime.ParseExact(invoiceInquiry.TAX_INVOICE_DT_STR, "dd.MM.yyyy", null);
                            }
                            //SystemProperty sysProp = (SystemProperty)systemPropertyRepo
                            //    .GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_TAX_CODE)[0];
                            //create_inv_in.TAX_CODE = sysProp.SYSTEM_CD;

                            e.INPUT.Add(create_inv_in);

                            if (item.STAMP_AMOUNT > 0)
                            {
                                stamp = glAccountRepo.GetGLAccountByCategory(CommonConstant.STAMP)[0];
                                stampAmount = item.STAMP_AMOUNT;
                            }

                            i++;
                        }
                    }


                    if (detailsGR != null && detailsGR.Count > 0)
                    {
                        e.GR_DATA = new List<CREATE_INV_GR_DATA>();
                        foreach (InvoiceInquiryDetail item in detailsGR)
                        {
                            CREATE_INV_GR_DATA grData = new CREATE_INV_GR_DATA();
                            grData.PO_NUMBER = item.PO_NUMBER;
                            grData.PO_ITEM = item.PO_ITEM;
                            grData.GR_NUMBER = item.MATDOC_NUMBER;
                            grData.GR_YEAR = item.MATDOC_YEAR;
                            grData.GR_ITEM = item.MATDOC_ITEM;
                            e.GR_DATA.Add(grData);
                        }
                    }

                    // STAMP
                    if (stampAmount > 0)
                    {
                        CREATE_INV_GL_ACCOUNT create_inv_gl1 = new CREATE_INV_GL_ACCOUNT();
                        create_inv_gl1.GL_ACCOUNT = stamp.GL_ACCOUNT_NO.Trim();
                        create_inv_gl1.AMOUNT = stampAmount.ToString();
                        create_inv_gl1.SHKZG = stamp.TYPE;
                        e.GL_ACCOUNT.Add(create_inv_gl1);
                    }

                    // WITHOLDING
                    if (invoiceInquiry.WITHHOLDING_TAX != null && !"".Equals(invoiceInquiry.WITHHOLDING_TAX.Trim()))
                    {
                        CREATE_INV_GL_ACCOUNT create_inv_gl2 = new CREATE_INV_GL_ACCOUNT();
                        create_inv_gl2.GL_ACCOUNT = witholdingTax.GL_ACCOUNT_NO.Trim(); // "2140030";
                        create_inv_gl2.AMOUNT = (witholdingTax.PERCENTAGE / 100 * invoiceInquiry.TURN_OVER).ToString();
                        create_inv_gl2.SHKZG = witholdingTax.TYPE;
                        e.GL_ACCOUNT.Add(create_inv_gl2);
                    }


                    // GL ACCOUNT
                    if (invoiceInquiry.GL_ACCOUNT != null && !"".Equals(invoiceInquiry.GL_ACCOUNT.Trim()))
                    {
                        CREATE_INV_GL_ACCOUNT create_inv_gl3 = new CREATE_INV_GL_ACCOUNT();
                        create_inv_gl3.GL_ACCOUNT = invoiceInquiry.GL_ACCOUNT.Trim();
                        if (double.Parse(glAmount) < 0)
                        {
                            create_inv_gl3.AMOUNT = (double.Parse(glAmount) * -1).ToString();
                            create_inv_gl3.SHKZG = "H";
                        }
                        else
                        {
                            create_inv_gl3.AMOUNT = glAmount.ToString();
                            create_inv_gl3.SHKZG = "S";
                        }
                        //create_inv_gl1.GL_ACCOUNT = "2140030";
                        //create_inv_gl1.AMOUNT = "3000";
                        e.GL_ACCOUNT.Add(create_inv_gl3);
                    }

                    //results = sap.CreateInv(e, items[j], NoReg);
                    results = invoiceInquiryRepo.SavePostInvoiceNonDirect(invoiceInquiry, e, NoReg, db);
                }

                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else
                {
                    db.CommitTransaction();

                }


            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            finally
            {
                db.Close();
            }

            return Json(results);
        }

        public ActionResult refreshPartnerBank(string supplierCd, string invoiceNo)
        {
            IDBContext db = databaseManager.GetContext();
            List<Supplier> partnerBankList = invoiceInquiryRepo.GetPartnerBankList(supplierCd, db);
            ViewData["PartnerBankList"] = partnerBankList;



            constructComboBoxes();
            constructComboBoxesTaxCode(invoiceNo, supplierCd);
            return PartialView("_PostInvoicePopUp");
        }

        public ActionResult SubmitNotice(List<InvoiceInquiry> invoiceList)
        {
            //string message = "";
            //int results = 0;
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                results = invoiceInquiryRepo.SubmitNotice(db, invoiceList);
                //message = "Success";

                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else
                {
                    db.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            finally
            {
                db.Close();
            }

            return Json(results);
        }

        public void DownloadInvoiceInquiry(string createdDateSearch, string supplierSearch,
            string invoiceDateSearch, string statusSearch,
            string planPaymentDateSearch, string invoiceNoSearch, string submissionDateSearch, string statusHardcopySearch)
        {
            string fileName = string.Format("InvoiceInquiry{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // Get User Id
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;
            List<InvoiceInquiry> invoiceInquiryList = invoiceInquiryRepo.GetInvoiceInquiryExcel(createdDateSearch, supplierSearch,
                invoiceDateSearch, statusSearch, statusHardcopySearch, planPaymentDateSearch, invoiceNoSearch, submissionDateSearch).ToList();

            byte[] result = invoiceInquiryRepo.GenerateDownloadFile(invoiceInquiryList);
            SendToClientBrowser(fileName, result);
        }

        protected void SendToClientBrowser(string fileName, byte[] hasil)
        {
            Response.Clear();
            //Response.ContentType = result.MimeType;
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Expires = -1;
            Response.Buffer = true;

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", Convert.ToString(hasil.Length));

            Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", fileName));
            Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

            Response.BinaryWrite(hasil);
            Response.End();
        }

        public ActionResult UploadPpv(HttpPostedFileBase file)
        {
            string path = null;

            try
            {
                SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_PPV, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                path = sysProp.SYSTEM_VALUE_TEXT;
                //path = "D:\\g\\file\\";

                //if (Directory.Exists(path))
                //{
                //    HousekeepingTempFolder(path, 2);
                //}
            }
            catch (Exception ex)
            {
                AjaxResult ajaxResult = new AjaxResult();
                //Debug.WriteLine(ex.StackTrace);

                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };

                //return Content(new JavaScriptSerializer().Serialize(ajaxResult),
                //    "text/plain", System.Text.Encoding.UTF8);
            }

            return UploadFile(file, path);
        }

        public FileResult DownloadFileInvoice(string fileName, string fileNameServer)
        {
            IList<string> paths = new List<string>();

            // File Folder
            SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            paths.Add(sysProp.SYSTEM_VALUE_TEXT);
            //paths.Add("D:\\g\\file\\");

            // add new foldering
            InvoiceInquiry invoice = invoiceInquiryRepo.getInvoiceByAttachmentFile(fileName);

            string tahun = invoice.INVOICE_DATE != null ? ((DateTime)invoice.INVOICE_DATE).ToString("yyyy") : DateTime.Now.ToString("yyyy");
            string bulan = invoice.INVOICE_DATE != null ? ((DateTime)invoice.INVOICE_DATE).ToString("MMMM") : DateTime.Now.ToString("MMMM");
            string supplier = invoice.SUPPLIER_CD;
            string addPath = Path.Combine(tahun, bulan.ToUpper(), supplier);

            return DownloadFile(Path.Combine(addPath, fileNameServer), paths, fileName);
        }

        // Summary:
        //     Handle Download File Request
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file
        //     fileDownloadName : the filename to be send to browser
        protected FileResult DownloadFile(string fileName, IList<string> paths, string fileDownloadName)
        {
            string fileNameServerFullPath = null;
            foreach (string path in paths)
            {
                fileNameServerFullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fileNameServerFullPath))
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(fileDownloadName))
            {
                fileDownloadName = fileName;
            }

            //Response.AddHeader("Set-Cookie", "fileDownload1=true; path=/");
            //Response.AddHeader("Set-Cookie", "inline; path=/");

            return File(fileNameServerFullPath, MimeExtensionHelper.GetMimeType(fileDownloadName));/*MimeMapping.GetMimeMapping(fileName)*/
        }

        protected ActionResult UploadFile(HttpPostedFileBase file, string path)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                if (file != null)
                {
                    //string path = ConfigurationManager.AppSettings[APP_SETTING_TEMP_UPLOAD_FILE_PATH];
                    string fileNameOrigin = file.FileName;
                    string fileNameServer = Guid.NewGuid().ToString() + "_" + fileNameOrigin;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileNameServerFullPath = Path.Combine(path, fileNameServer);
                    file.SaveAs(fileNameServerFullPath);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                    ajaxResult.Params = new object[] {
                        fileNameOrigin, fileNameServer
                    };
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { "No file uploaded, please reupload again the file" };
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.StackTrace);

                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }

            return Content(new JavaScriptSerializer().Serialize(ajaxResult),
                "text/plain", System.Text.Encoding.UTF8);
        }

        public ActionResult DeleteUploadFile(string fileNameServer)
        {
            IList<string> paths = new List<string>();

            // File Folder
            SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_PPV, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            paths.Add(sysProp.SYSTEM_VALUE_TEXT);
            //paths.Add("D:\\g\\file\\");

            return DeleteFile(fileNameServer, paths);
        }

        // Summary:
        //     Handle Delete File Request
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file (only delete first found file)
        protected ActionResult DeleteFile(string fileName, IList<string> paths)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    DeleteFileProcess(fileName, paths);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] { "No define the file to be deleted" };
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.StackTrace);

                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                };
            }

            return Json(ajaxResult);
        }

        // Summary:
        //     Delete File Process
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file (only delete first found file)
        protected void DeleteFileProcess(string fileName, IList<string> paths)
        {
            string fileNameFullPath = null;
            foreach (string path in paths)
            {
                fileNameFullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fileNameFullPath))
                {
                    break;
                }
            }

            //string path = ConfigurationManager.AppSettings[APP_SETTING_TEMP_UPLOAD_FILE_PATH];
            //string fileNameFullPath = Path.Combine(path, fileName);

            if (System.IO.File.Exists(fileNameFullPath))
            {
                System.IO.File.Delete(fileNameFullPath);
            }
        }

        public FileResult DownloadFilePPV(string fileName, string fileNameServer)
        {
            IList<string> paths = new List<string>();

            // File Folder
            SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_PPV, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            paths.Add(sysProp.SYSTEM_VALUE_TEXT);
            //paths.Add("D:\\g\\file\\");

            return DownloadFile(fileNameServer, paths, fileName);
        }

        // Summary:
        //     Handle Download File Request
        //
        // Parameters:
        //     fileName : the filename to be download
        //     paths : list path will be search the file
        //     fileDownloadName : the filename to be send to browser

        //20200706
        public string invoiceWHT(string invoiceNo)
        {
            string result = null;

            result = invoiceInquiryRepo.invoiceWHT(invoiceNo);

            return result;
        }
        protected void HousekeepingTempFolder(string path, int remainDays)
        {
            var directory = new DirectoryInfo(path);
            DateTime limitDt = DateTime.Now.Date.AddDays(-1 * remainDays);

            directory.GetFiles().Where(file => file.LastWriteTime.Date <= limitDt).ToList().ForEach(file => file.Delete());
        }
        #region Invoice Attachment
        public ActionResult UploadAttachment(string attachmentType, string invoiceId, HttpPostedFileBase file)
        {
            AjaxResult ajaxResult = new AjaxResult();
            string path = null;

            if (file != null)
            {
                bool upload = true;

                //check file format
                SystemProperty sysFileFormat = systemPropertyRepo.GetSysPropByCodeAndType(
                                attachmentType, CommonConstant.SYSTEM_TYPE_ATTACHMENT_FILE_FORMAT);

                string fileExt = Path.GetExtension(file.FileName).Substring(1);
                string[] allowedExt = sysFileFormat.SYSTEM_VALUE_TEXT.Split(';');

                bool errorExt = true;
                foreach (var allowed in allowedExt)
                {
                    if (fileExt.ToUpper().Equals(allowed.ToUpper()))
                    {
                        errorExt = false;
                        break;
                    }
                }

                if (errorExt)
                {
                    upload = false;

                    Message msg = messageRepo.GetMessageById("INVCRE000002");

                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] {
                    string.Format(msg.MSG_TEXT, String.Join(" / ", allowedExt))
                };
                    return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
                }

                //check file size
                SystemProperty sysFileSize = systemPropertyRepo.GetSysPropByCodeAndType(
                                attachmentType, CommonConstant.SYSTEM_TYPE_ATTACHMENT_MAX_SIZE);

                decimal fileSize = file.ContentLength;
                decimal maxSize = int.Parse(sysFileSize.SYSTEM_VALUE_TEXT) * 1024 * 1024;

                if (fileSize > maxSize)
                {
                    upload = false;

                    Message msg = messageRepo.GetMessageById("INVCRE000003");

                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new string[] {
                            string.Format(msg.MSG_TEXT, sysFileSize.SYSTEM_VALUE_TEXT)
                        };
                    return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
                }

                if (upload)
                {
                    try
                    {
                        SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                                CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                        path = sysProp.SYSTEM_VALUE_TEXT;

                        if (Directory.Exists(path))
                        {
                            HousekeepingTempFolder(path, 2);
                        }
                        else
                        {
                            Directory.CreateDirectory(path);
                        }

                        string fileNameOrigin = file.FileName;
                        //string fileNameServer = Guid.NewGuid().ToString() + "_" + fileNameOrigin;
                        string fileNameServer = Guid.NewGuid().ToString() + "_" + Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber + Path.GetExtension(file.FileName);

                        string fileNameServerFullPath = Path.Combine(path, fileNameServer);
                        file.SaveAs(fileNameServerFullPath);

                        //insert into temp table
                        string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;

                        ajaxResult = invoiceInquiryRepo.insertInvoiceAttachmentTemp(attachmentType, invoiceId, fileNameOrigin, fileNameServer, NoReg);
                    }
                    catch (Exception ex)
                    {
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        ajaxResult.ErrMesgs = new string[] {
                            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                        };
                    }
                }
            }
            else
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                    string.Format("Please choose file to upload!")
                };
            }

            return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
        }

        public ActionResult DeleteAttachment(string attachmentType, string fileName, string fileNameServer)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                            CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                string path = sysProp.SYSTEM_VALUE_TEXT;

                string fileNameServerFullPath = Path.Combine(path, fileNameServer);
                if (System.IO.File.Exists(fileNameServerFullPath))
                {
                    System.IO.File.Delete(fileNameServerFullPath);
                }

                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                ajaxResult = invoiceInquiryRepo.deleteInvoiceAttachmentTemp(attachmentType, fileName, NoReg);
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                        string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                    };
            }

            return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
        }

        public FileResult ViewAttachment(string fileName, string fileNameServer, string invoiceDate, string supplierCd)
        {
            SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                        CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            string path = sysProp.SYSTEM_VALUE_TEXT;
            string tahun = "";
            string bulan = "";
            string fileNameServerFullPath = "";
            if (fileName == fileNameServer)
            {
                if (string.IsNullOrEmpty(invoiceDate))
                {
                    tahun = DateTime.Now.ToString("yyyy");
                    bulan = DateTime.Now.ToString("MMMM");
                }
                else
                {
                    string[] arrayInvDate = invoiceDate.Split('.');
                    DateTime temp = new DateTime(Int32.Parse(arrayInvDate[2]), Int32.Parse(arrayInvDate[1]), Int32.Parse(arrayInvDate[0]));
                    tahun = temp.ToString("yyyy");
                    bulan = temp.ToString("MMMM");
                }
                string supplier = supplierCd;
                string addPath = Path.Combine(tahun, bulan.ToUpper(), supplier);
                string newPath = Path.Combine(path, addPath);
                fileNameServerFullPath = Path.Combine(newPath, fileNameServer);
            }
            else
            {
                fileNameServerFullPath = Path.Combine(path, fileNameServer);
            }

            if (System.IO.File.Exists(fileNameServerFullPath))
            {
                return File(fileNameServerFullPath, MimeExtensionHelper.GetMimeType(fileName));
            }
            else
            {
                return null;
            }
        }

        #endregion
        public JsonResult SaveInvoice(Invoice invoice, List<HttpPostedFileBase> file, List<InvoiceAttachment> attachments)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;

                SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                    CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);

                string path = sysProp.SYSTEM_VALUE_TEXT;
                //string companyId = Lookup.Get<Toyota.Common.Credential.User>().Company.Id;
                //List<SystemProperty> listCompanyCodeTmmin = systemPropertyRepo.GetBySystemPropertyType("TMMIN_COMPANY_CD");
                //bool isSupplier = !listCompanyCodeTmmin.Select(x => x.SYSTEM_CD).Contains(companyId);
                //string userLoginAs = isSupplier ? "Supplier" : "Finance";
                bool isVendor = Lookup.Get<Toyota.Common.Credential.User>().Roles.Select(x => x.Id).Contains("Liv_Vendor");
                string userLoginAs = isVendor ? "Supplier" : "Finance";
                results = invoiceInquiryRepo.SaveInvoice(db, invoice, NoReg, attachments, path, isVendor);



                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else if (AjaxResult.VALUE_SUCCESS.Equals(results.Result))
                {
                    db.CommitTransaction();

                    results.Result = AjaxResult.VALUE_SUCCESS;
                    results.SuccMesgs = new String[] {
                                    string.Format("{0}", "Save data finish successfully")};
                }
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            finally
            {
                db.Close();
            }

            return Json(results);
        }

        public ActionResult DeleteAttachmentTemp(string invoiceId)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                ajaxResult = invoiceInquiryRepo.deleteAllInvoiceAttachmentTemp(invoiceId);
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                        string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
                    };
            }

            return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
        }

    }
}
