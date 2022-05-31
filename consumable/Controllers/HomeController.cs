using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Commons.Constants;
using consumable.Models.SystemProperty;
using consumable.Models.Announcement;
using consumable.Models.InvoiceCreation;
using consumable.Models.Paging;
using consumable.Models;
using consumable.Commons.Helpers;
using consumable.Models.InvoiceInquiry;
using System.Linq;
using consumable.Models.GLAccount;
using System.Web;
using consumable.Models.Message;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Toyota.Common.Database;
using consumable.Models.SUPPLIER;

namespace consumable.Controllers
{
    public class HomeController : PageController
    {
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private AnnouncementRepository announcementRepo = AnnouncementRepository.Instance;
        private InvoiceCreationRevRepository invoiceRepo = InvoiceCreationRevRepository.Instance;
        private InvoiceInquiryRepository invoiceInquiryRepo = InvoiceInquiryRepository.Instance;
        private GLAccountRepository glAccountRepo = GLAccountRepository.Instance;
        private MessageRepository messageRepo = MessageRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private DatabaseManager databaseManager = DatabaseManager.Instance;

        protected override void Startup()
        {
            Settings.Title = "Dashboard";

            //try
            //{
            //    GetTableauGraphUrl();
            //}
            //catch (Exception ex)
            //{                
            //    Console.WriteLine(ex);  
            //}

            ViewData["WitholdingTaxList"] = glAccountRepo.GetGLAccountByCategory(CommonConstant.WITHOLDING_TAX);
            ViewData["AttachmentDocTypeList"] = systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_ATTACHMENT);
            GetDashboardNotice(CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            GetDashboardAnnouncement(CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
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
        public ActionResult WidgetSettings()
        {
            return PartialView("_WidgetSettings");
        }


        private string GetTableauAuthenticationTicket()
        {
            var user = "tableau.viewer"; //UserInformation.GetAuthenticatedUsername();
            var request = (HttpWebRequest)WebRequest.Create("http://"+ CommonConstant.TABLEAU_IP+"/trusted");

            string ticket = null;

            var encoding = new UTF8Encoding();
            var postData = "username=" + user;
            byte[] data = encoding.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                ticket = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                 Console.WriteLine(ex.Status);
                 return null ;
            }            
            return ticket;
        }


        private void GetTableauGraphUrl()
        {
            string ticket = GetTableauAuthenticationTicket();
            string tableauUrl = "";
            
            if (ticket == null)
            {
                tableauUrl = "";
            }
            else
            {
                tableauUrl = "http://"+ CommonConstant.TABLEAU_IP +"/trusted/"+ ticket
                + "/views/06-DJCD/ATSGWEBVIEW?:embed=y";
            }
           
            ViewData["tableauUrl"] = tableauUrl;
        }

        // Get Notice List
        private void GetDashboardNotice(int page, int size)
        {
            string supplierCode = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());

            // update 08-01-2021 [START]
            if (supplierCode == null)
            {
                bool isFinance = Lookup.Get<Toyota.Common.Credential.User>().Roles.Select(x => x.Id).Contains("Liv_Admin");
                bool isVendor = Lookup.Get<Toyota.Common.Credential.User>().Roles.Select(x => x.Id).Contains("Liv_Vendor");

                if (!isFinance && !isVendor)
                {
                    supplierCode = "-";
                }
            }
            // update 08-01-2021 [END]

            int countdata = invoiceRepo.CountDashboardNotice(supplierCode);
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
            Paging pg = new Paging(countdata, page, size);
            ViewData["Paging"] = pg;

            ViewData["NoticeList"] = invoiceRepo.GetDashboardNotice(supplierCode, pg.StartData, pg.EndData);
        }

        public ActionResult SearchDashboardNotice(int page, int size)
        {
            GetDashboardNotice(page, size);

            return PartialView("_NoticeList");
        }

        // Get Announcement List
        private void GetDashboardAnnouncement(int page, int size)
        {
            SystemProperty systemProp = systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CODE_STATUS_ACTIVE, CommonConstant.SYSTEM_TYPE_STATUS_ANNOUNCEMENT);
            string status = systemProp.SYSTEM_VALUE_TEXT;

            int countdata = announcementRepo.CountDashboardAnnouncement(status);

            Paging pg = new Paging(countdata, page, size);
            ViewData["PagingSmall"] = pg;

            ViewData["AnnouncementList"] = announcementRepo.GetDashboardAnnouncement(status, pg.StartData, pg.EndData);
        }

        public ActionResult SearchDashboardAnnouncement(int page, int size)
        {
            GetDashboardAnnouncement(page, size);

            return PartialView("_AnnouncementList");
        }

        public FileResult DownloadAnnouncement(string url)
        {
            //SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_ANNOUNCEMENT, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            //string path = sysProp.SYSTEM_VALUE_TEXT;
            //string filePath = Path.Combine(path, url);

            if (System.IO.File.Exists(url))
            {
                Response.Clear();
                Response.ContentType = MimeExtensionHelper.GetMimeType(url);
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(url));
                Response.TransmitFile(url);
                Response.End();
                return File(url, MimeExtensionHelper.GetMimeType(url));
            }
            else
            {
                return null;
            }
        }

        #region invoice verification

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

        protected void HousekeepingTempFolder(string path, int remainDays)
        {
            var directory = new DirectoryInfo(path);
            DateTime limitDt = DateTime.Now.Date.AddDays(-1 * remainDays);

            directory.GetFiles().Where(file => file.LastWriteTime.Date <= limitDt).ToList().ForEach(file => file.Delete());
        }

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
                    vendor = supp.SUPPLIER_CD + "_" + (supp.SUPPLIER_NAME.Length > 15 ? supp.SUPPLIER_NAME.Substring(0, 15) : supp.SUPPLIER_NAME);

                }
                else
                {
                    InvoiceInquiry invoice = invoiceInquiryRepo.GetInvoiceDetailByInvIdInvNo(invoiceId, invoiceNo);
                    Supplier supp = supplierRepo.GetBySupplierCd(invoice.SUPPLIER_CD);
                    vendor = supp.SUPPLIER_CD + "_" + (supp.SUPPLIER_NAME.Length > 15 ? supp.SUPPLIER_NAME.Substring(0, 15) : supp.SUPPLIER_NAME);

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

        #endregion
    }
}
