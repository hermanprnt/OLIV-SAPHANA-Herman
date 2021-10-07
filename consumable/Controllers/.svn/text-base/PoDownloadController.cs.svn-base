using System.Collections.Generic;
using consumable.Models;
using consumable.Models.Paging;
using consumable.Models.PoDownload;
using consumable.Models.SUPPLIER;
using Toyota.Common.Web.Platform;
using System.Web.Mvc;
using Toyota.Common.Database;
using System;
using consumable.Models.ReportView;
using System.Web;
using consumable.Models.SystemProperty;
using System.Web.Script.Serialization;
using System.IO;
using consumable.Commons.Constants;
using System.Linq;
using consumable.Commons.Helpers;

namespace consumable.Controllers
{
    public class PoDownloadController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private PoDownloadRepository poDownloadRepo = PoDownloadRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Purchase Order";
            string statusDefault = "";
            string positionCode = "";
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
            List<PoApproval> poApprovals = poDownloadRepo.GetPoApprovalByNoReg(NoReg);

            if (poApprovals != null && poApprovals.Count > 0)
            {
                positionCode = poApprovals[0].POSITION_CODE;
            }

            if ("SH".Equals(positionCode))
            {
                statusDefault = "Not Yet";
            }
            else if ("DPH".Equals(positionCode))
            {
                statusDefault = "Approved By SH";
            }
            else if ("DH".Equals(positionCode))
            {
                statusDefault = "Approved By DPH";
            }
            else
            {
                statusDefault = "All";
            }


            ViewData["ApprovalStatus"] = statusDefault;

            string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
            ViewData["vendCodeLogin"] = vendCodeLogin;
            if (vendCodeLogin != null)
            {
                ViewData["readonly"] = "readonly";
                ViewData["disabled"] = "disabled";
            }
            else
            {
                ViewData["readonly"] = "";
                ViewData["disabled"] = "";
            }

            constructComboBoxes();
            getListPo(null, vendCodeLogin, null, statusDefault, null, null, null, null,
               CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }

        public ActionResult search(string createdDate, string supplierSearch, string releasedDate, string statusSearch,
            string poDescription, string poNumber, string poAmountFrom, string poAmountTo, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListPo(createdDate, supplierSearch, releasedDate, statusSearch,
                poDescription, poNumber, poAmountFrom, poAmountTo,
                page, size);

            return PartialView("_GridView");
        }

        private void getListPo(string createdDate, string supplierSearch, string releasedDate, string statusSearch,
            string poDescription, string poNumber, string poAmountFrom, string poAmountTo, int page, int size)
        {
            int countdata = 0;
            countdata = poDownloadRepo.countPoList(createdDate, supplierSearch, releasedDate, statusSearch,
                poDescription, poNumber, poAmountFrom, poAmountTo);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<PoDownload> poList = poDownloadRepo.GetPoList(createdDate, supplierSearch, releasedDate, statusSearch,
                poDescription, poNumber, poAmountFrom, poAmountTo,
                pg.StartData, pg.EndData);

            ViewData["PoDownload"] = poList;
        }

        public void constructComboBoxes()
        {
            getLookupSupplierSearch(
                CommonFunction.Instance.DefaultStringValue(),
                CommonFunction.Instance.DefaultPage(),
                CommonFunction.Instance.DefaultSize());
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

        private void getLookupSupplierSearch(string parameter, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier(parameter), page, size);

            ViewData["LookupPaging"] = pg;

            ViewData["Suppliers"] = supplierRepo.GetSupplier(parameter, pg.StartData, pg.EndData);
        }

        public ActionResult GetPoItemList(string poNumber)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            ViewData["PoItemList"] = poDownloadRepo.GetPoItemList(poNumber);

            return PartialView("_PoItemPopUp");
        }

        public ActionResult SubmitNotice(List<PoDownload> poList)
        {
            //string message = "";
            //int results = 0;
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                results = poDownloadRepo.SubmitNotice(db, poList);
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


        public FileResult PrintPo(string poNumber, string vendCode)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            ReportViewModel reportViewModel = consumable.Commons.PrintReport.Instance.GetReportViewModelPo(poNumber, vendCode, NoReg);

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

        public JsonResult ApprovePo(List<PoDownload> items)
        {
            AjaxResult results = new AjaxResult();
            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();
                Boolean errorFlag = true;
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                string positionCode = null;

                List<PoApproval> poApprovals = poDownloadRepo.GetPoApprovalByNoReg(NoReg);

                if (poApprovals != null && poApprovals.Count > 0)
                {
                    positionCode = poApprovals[0].POSITION_CODE;
                }

                if (positionCode != null && !"".Equals(positionCode))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if ("SH".Equals(positionCode))
                        {
                            if (items[i].APPROVAL_STATUS == null || "".Equals(items[i].APPROVAL_STATUS))
                            {
                                errorFlag = false;
                            }
                        }
                        else if ("DPH".Equals(positionCode))
                        {
                            if ((items[i].APPROVAL_STATUS == null || "".Equals(items[i].APPROVAL_STATUS))
                                    || items[i].APPROVAL_STATUS.Equals("Approved By SH"))
                            {
                                errorFlag = false;
                            }
                        }
                        else if ("DH".Equals(positionCode))
                        {
                            if ((items[i].APPROVAL_STATUS == null || "".Equals(items[i].APPROVAL_STATUS))
                                    || items[i].APPROVAL_STATUS.Equals("Approved By SH") || 
                                        items[i].APPROVAL_STATUS.Equals("Approved By DPH"))
                            {
                                errorFlag = false;
                            }
                        }

                        if (!errorFlag)
                        {
                            results = poDownloadRepo.ApproveRejectPo(db, items[i].PO_NUMBER, items[i].VEND_CODE, NoReg, positionCode, "APPROVE");
                        }
                        else 
                        {
                            results.Result = AjaxResult.VALUE_ERROR;
                            results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", "ERROR", "Approval Is Not Allowed")};
                        }

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
                else
                {
                    results.Result = AjaxResult.VALUE_ERROR;
                    results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", "ERROR", "Not Authorized")};
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

        public JsonResult RejectPo(List<PoDownload> items)
        {
            AjaxResult results = new AjaxResult();
            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();
                Boolean errorFlag = true;
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                string positionCode = null;

                List<PoApproval> poApprovals = poDownloadRepo.GetPoApprovalByNoReg(NoReg);

                if (poApprovals != null && poApprovals.Count > 0)
                {
                    positionCode = poApprovals[0].POSITION_CODE;
                }

                if (positionCode != null && !"".Equals(positionCode))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if ("SH".Equals(positionCode))
                        {
                            if (items[i].APPROVAL_STATUS == null || "".Equals(items[i].APPROVAL_STATUS))
                            {
                                errorFlag = false;
                            }
                        }
                        else if ("DPH".Equals(positionCode))
                        {
                            if ((items[i].APPROVAL_STATUS == null || "".Equals(items[i].APPROVAL_STATUS))
                                    && (!items[i].APPROVAL_STATUS.Equals("Approved By DPH")
                                        || !items[i].APPROVAL_STATUS.Equals("Approved By DH"))
                                    && (!items[i].APPROVAL_STATUS.Equals("Rejected By SH")
                                        || !items[i].APPROVAL_STATUS.Equals("Rejected By DPH")
                                        || !items[i].APPROVAL_STATUS.Equals("Rejected By DH")) )
                            {
                                errorFlag = false;
                            }
                        }
                        else if ("DH".Equals(positionCode))
                        {
                            if ((items[i].APPROVAL_STATUS == null || "".Equals(items[i].APPROVAL_STATUS))
                                    && (!items[i].APPROVAL_STATUS.Equals("Approved By DH"))
                                    && (!items[i].APPROVAL_STATUS.Equals("Rejected By SH")
                                        || !items[i].APPROVAL_STATUS.Equals("Rejected By DPH")
                                        || !items[i].APPROVAL_STATUS.Equals("Rejected By DH")) )
                            {
                                errorFlag = false;
                            }
                        }

                        if (!errorFlag)
                        {
                            results = poDownloadRepo.ApproveRejectPo(db, items[i].PO_NUMBER, items[i].VEND_CODE, NoReg, positionCode, "REJECT");
                        }
                        else
                        {
                            results.Result = AjaxResult.VALUE_ERROR;
                            results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", "ERROR", "Reject Is Not Allowed")};
                        }

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
                else
                {
                    results.Result = AjaxResult.VALUE_ERROR;
                    results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", "ERROR", "Not Authorized")};
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

        public ActionResult UploadPo(HttpPostedFileBase file)
        {
            string path = null;

            try
            {
                SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_PO, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                path = sysProp.SYSTEM_VALUE_TEXT;
                //path = "D:\\g\\file\\";

                if (Directory.Exists(path))
                {
                    HousekeepingTempFolder(path, 2);
                }
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

        protected void HousekeepingTempFolder(string path, int remainDays)
        {
            var directory = new System.IO.DirectoryInfo(path);
            DateTime limitDt = DateTime.Now.Date.AddDays(-1 * remainDays);

            directory.GetFiles().Where(file => file.LastWriteTime.Date <= limitDt).ToList().ForEach(file => file.Delete());
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
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_PO, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
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

        public JsonResult SaveFilePo(PoDownload poDownload, List<HttpPostedFileBase> file)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                results = poDownloadRepo.SaveFilePo(db, poDownload);

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

        public ActionResult ShowFileDownload(string poNumber)
        {
            ViewData["ListFileDownload"] = poDownloadRepo.GetListFileDownload(poNumber);

            return PartialView("_FileDownloadPopUp");
        }

        public FileResult DownloadFilePo(string fileName, string fileNameServer)
        {
            IList<string> paths = new List<string>();

            // File Folder
            SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
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

    }
}
