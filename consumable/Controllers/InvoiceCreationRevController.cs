using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using consumable.Common.Data;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.EFaktur;
using consumable.Models.GR_IR;
using consumable.Models.InvoiceCreation;
using consumable.Models.Paging;
using consumable.Models.ReceivingList;
using consumable.Models.ReportView;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;
using consumable.Models.Message;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Models.GLAccount;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;//20200514
using consumable.Commons.Helpers;
using System.Text.RegularExpressions;

namespace consumable.Controllers
{
    public class InvoiceCreationRevController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private ReceivingListRepository receivingListRepo = ReceivingListRepository.Instance;
        private InvoiceCreationRevRepository invoiceCreationRevRepo = InvoiceCreationRevRepository.Instance;
        private EFakturRepository eFakturRepo = EFakturRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private GR_IRRepository grIrRepo = GR_IRRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private GLAccountRepository glAccountRepo = GLAccountRepository.Instance;//20200514
        private MessageRepository messageRepo = MessageRepository.Instance;

        public const int DATA_ROW_INDEX_START = 1;//20200707

        protected override void Startup()
        {
            Settings.Title = "Invoice Creation";

            string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
            ViewData["vendCodeLogin"] = vendCodeLogin;
            if (vendCodeLogin != null)
            {
                ViewData["readonly"] = "readonly";
                ViewData["disabled"] = "disabled";

                ViewData["visibilityVndr"] = "true";
            }
            else
            {
                ViewData["readonly"] = "";
                ViewData["disabled"] = "";

                ViewData["visibilityVndr"] = "false";
            }

            constructComboBoxes();
            //ViewData["InvoiceCreation"] = (List<InvoiceCreation>)invoiceCreationRepo.GetInvoiceCreation();
            //ViewData["MaterialList"] = (List<InvoiceCreation>)invoiceCreationRepo.GetMaterialList();

            getInvoiceCreationList("", vendCodeLogin, "", "", "", "", "",
              CommonFunction.Instance.DefaultPage(), 100);

            //int countdata = 0;
            //countdata = eFakturRepo.countEfaktur(null, vendCodeLogin);
            //Paging pgLookupEfaktur = new Paging(countdata,
            //    CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            //ViewData["LookupPagingEfaktur"] = pgLookupEfaktur;
            //ViewData["eFakturList"] = (List<EFaktur>)eFakturRepo.GetData(null, vendCodeLogin, 
            //    CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());

            //Paging pg = new Paging(0, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            //ViewData["CountData"] = 0;
            //ViewData["Paging"] = pg;

            //20200514
            List<GLAccount> listWitholdingTax = (List<GLAccount>)glAccountRepo.GetGLAccountByCategory(CommonConstant.WITHOLDING_TAX);
            ViewData["WitholdingTaxList"] = listWitholdingTax;

            //Get attachment doc type from tb m system
            List<SystemProperty> listAttachmentDocType = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_ATTACHMENT);
            ViewData["AttachmentDocTypeList"] = listAttachmentDocType;

            //Get string info forbidden character on upload popup
            var FORBIDDEN_CHARACTER = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType("FORBIDDEN_INVOICE_FILENAME", CommonConstant.SYSTEM_TYPE_ATTACHMENT_FORMAT_FILENAME);
            var _MESSAGE = (Message)messageRepo.GetMessageById("INVCRE000106");
            ViewData["INFO_MESSAGE_FORBIDDEN_CHARACTER_UPLOAD"] = string.Format(_MESSAGE.MSG_TEXT, FORBIDDEN_CHARACTER.SYSTEM_VALUE_TEXT);
        }

        public ActionResult search(string poNoSearch, string supplierSearch, string poDateSearch,
            string poText, string receivingDateSearch, string matDocNoSearch, string dnNoSearch, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getInvoiceCreationList(poNoSearch, supplierSearch, poDateSearch, poText, receivingDateSearch, matDocNoSearch,
                dnNoSearch, page, size);

            return PartialView("_GridView");
        }

        private void getInvoiceCreationList(string poNoSearch, string supplierSearch, string poDateSearch,
            string poText, string receivingDateSearch, string matDocNoSearch, string dnNoSearch, int page, int size)
        {
            int countdata = 0;
            countdata = invoiceCreationRevRepo.countInvoiceCreationList(poNoSearch, supplierSearch, poDateSearch,
                CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch, dnNoSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            ViewData["InvoiceCreation"] = invoiceCreationRevRepo.GetInvoiceCreation(poNoSearch, supplierSearch,
                poDateSearch, CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch, dnNoSearch, pg.StartData, pg.EndData);
        }

        public string getSupplierEditAmountFlag(string supplierCode)
        {
            Supplier supplier = supplierRepo.GetBySupplierCd(supplierCode);
            if (supplier != null && supplier.EDIT_AMOUNT_FLAG != null)
            {
                return supplier.EDIT_AMOUNT_FLAG;
            }
            else
            {
                return null;
            }

        }

        [HttpGet]
        public ContentResult GetInvoiceCreationSort(string poNoSearch, string supplierSearch, string poDateSearch,
            string poText, string receivingDateSearch, string matDocNoSearch, string dnNoSearch, int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = invoiceCreationRevRepo.countInvoiceCreationList(poNoSearch, supplierSearch, poDateSearch,
                CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch, dnNoSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<String> result = new List<String>();
            result = invoiceCreationRevRepo.GetInvoiceCreationSort(poNoSearch, supplierSearch, poDateSearch,
                CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch, dnNoSearch, pg.StartData, pg.EndData,
                field, sort);

            return Content(String.Join("", result.ToArray()));
        }

        public ActionResult GetInvoiceCreationDetail(string poNumber, string poItem, string poDate, string vendCode, string grStatus, string invoiceId)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            ViewData["MaterialList"] = receivingListRepo.GetReceivingListDetail(poNumber, poItem, poDate, vendCode, grStatus, invoiceId);

            return PartialView("_MatDocDetailPopUp");
        }

        public void constructComboBoxes()
        {
            //ViewData["Suppliers"] = (List<Supplier>)supplierRepo.GetSupplier("A", 1, 10);
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

        public ActionResult onLookupEfaktur(string Parameter, string VendCode, string PartialViewSearchAndInput, string SearchCode, int Page)
        {
            string validatemessage = string.Empty;
            Paging pg = new Paging(0, Page, CommonFunction.Instance.DefaultSize());
            List<EFaktur> eFakturList = null;

            if (PartialViewSearchAndInput.Equals("_LookupTableEfaktur"))
            {
                if (String.IsNullOrEmpty(SearchCode))
                {
                    pg = new Paging(eFakturRepo.countEfaktur(Parameter, VendCode), Page, CommonFunction.Instance.DefaultSize());

                    eFakturList = (List<EFaktur>)eFakturRepo.GetData(Parameter, VendCode, pg.StartData, pg.EndData);
                }
                else
                {
                    if (!String.IsNullOrEmpty(Parameter))
                    {
                        SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                                        CommonConstant.SYSTEM_CD_EFAKTUR_EXPIRED, CommonConstant.SYSTEM_TYPE_CREATE_INV);

                        validatemessage = eFakturRepo.ValidateEfakturLookup(Parameter, sysProp.SYSTEM_VALUE_NUM);
                    }

                    if (String.IsNullOrEmpty(validatemessage))
                    {
                        pg = new Paging(eFakturRepo.countEfakturWithValidate(Parameter, VendCode), Page, CommonFunction.Instance.DefaultSize());

                        eFakturList = (List<EFaktur>)eFakturRepo.GetDataEfakturWithValidate(Parameter, VendCode, pg.StartData, pg.EndData);
                    }
                   
                }

            }
            ViewData["validatemessage"] = validatemessage;
            if ((eFakturList == null || eFakturList.Count() == 0) && String.IsNullOrEmpty(validatemessage))
            {
                ViewData["validatemessage"] = "INVCRE000105";
            }
            ViewData["LookupPagingEfaktur"] = pg;
            ViewData["eFakturList"] = eFakturList;

            return PartialView(PartialViewSearchAndInput);
        }

        #region onlookupefakturexcel 20200909
        public ActionResult onLookupEfakturExcel(string Parameter, string VendCode, string PartialViewSearchAndInput, int Page)
        {
            if (PartialViewSearchAndInput.Equals("_LookupTableEfakturExcel"))
            {
                Paging pg = new Paging(eFakturRepo.countEfaktur(Parameter, VendCode), Page, CommonFunction.Instance.DefaultSize());

                ViewData["LookupPagingEfaktur"] = pg;

                ViewData["eFakturList"] = (List<EFaktur>)eFakturRepo.GetData(Parameter, VendCode, pg.StartData, pg.EndData);
            }

            return PartialView(PartialViewSearchAndInput);
        }
        #endregion

        public JsonResult SaveInvoice(Invoice invoice, List<GR_IR_DATA> items, List<HttpPostedFileBase> file, List<InvoiceAttachment> attachments)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                string seqCertificate = invoiceCreationRevRepo.getNewCertificateSeq(db, items[0].VEND_CODE, invoice.INVOICE_NO,
                    DateTime.Now.ToString("dd.MM.yyyy"));

                string certificateId = items[0].VEND_CODE + invoice.INVOICE_NO + DateTime.Now.ToString("yyyyMMdd") + seqCertificate;
                invoice.CERTIFICATE_ID = certificateId;

                SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                    CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);

                string path = sysProp.SYSTEM_VALUE_TEXT;

                results = invoiceCreationRevRepo.SaveInvoice(db, invoice, items, NoReg, attachments, path);

                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else if (AjaxResult.VALUE_SUCCESS.Equals(results.Result))
                {
                    db.CommitTransaction();

                    //taro execute update efaktur ke sini
                    invoiceCreationRevRepo.updateUsedFlagEfaktur(db, invoice, NoReg, invoice.INVOICE_NO);



                    results.Result = AjaxResult.VALUE_SUCCESS;
                    results.SuccMesgs = new String[] { 
                                    string.Format("{0} = {1}", "Certificate ID ", certificateId)};
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

        //public ActionResult UploadEFaktur(HttpPostedFileBase file)
        //{
        //    Stream tes = file.InputStream;
        //    AjaxResult ajaxResult = new AjaxResult();

        //    // Get User Id
        //    string NoRegLogin = Lookup.Get<Toyota.Common.Credential.User>().Id;

        //    // Get Username
        //    string username = Lookup.Get<Toyota.Common.Credential.User>().Username;

        //    // register all dll references to your project
        //    // this code need only be invoked at the beginning of your prgram
        //    WorkRegistry.Reset();

        //    // load PDF document
        //    PDFDocument doc = new PDFDocument(tes);
        //    //pdf.ConvertToImages(ImageType.PNG, @"C:\output\", "test");

        //    // get the page you want to scan
        //    BasePage page = doc.GetPage(0);

        //    // set reader setting
        //    ReaderSettings setting = new ReaderSettings();

        //    // set type to read
        //    setting.AddTypesToRead(BarcodeType.QRCode);

        //    // read barcode from PDF page
        //    Barcode[] barcodes = BarcodeReader.ReadBarcodes(setting, page);

        //    String qrCode = "";
        //    foreach (Barcode barcode in barcodes)
        //    {
        //        // print the loaction of barcode on image
        //        Console.WriteLine(barcode.BoundingRectangle.X + "  " + barcode.BoundingRectangle.Y);

        //        // output barcode data onto screen 
        //        Console.WriteLine(barcode.DataString);
        //        qrCode = barcode.DataString;
        //    }

        //    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
        //    ajaxResult.Params = new string[] { qrCode };

        //    return Json(ajaxResult);
        //}

        //20200812 download gr start

        #region downloadgr 20200812 start
        public void DownloadGR(string dnNoSearch, string poNoSearch, string receivingDateSearch,
            string supplierSearch, string poDateSearch, string matDocNoSearch, string statusSearch)
        {
            string fileName = string.Format("InvoiceCreation{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // Get User Id
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;
            List<GR_IR_DATA> invoiceCreationList = invoiceCreationRevRepo.GetDownloadGR(dnNoSearch, poNoSearch, receivingDateSearch,
            supplierSearch, poDateSearch, matDocNoSearch, statusSearch);

            byte[] result = invoiceCreationRevRepo.GenerateDownloadFile(invoiceCreationList);
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
        #endregion

        //public ActionResult Upload(string jenis, HttpPostedFileBase file)
        //{
        //    ISheet sheet;
        //    AjaxResult ajaxResult = null;

        //    // Get User Id
        //    string NoRegLogin = Lookup.Get<Toyota.Common.Credential.User>().Id;

        //    // Get Username
        //    string username = Lookup.Get<Toyota.Common.Credential.User>().Username;

        //    string filename = Path.GetFileName(Server.MapPath(file.FileName));
        //    var fileExt = Path.GetExtension(filename);

        //    List<string> errMesgs = new List<string>();

        //    if (fileExt == ".xls")
        //    {
        //        HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);
        //        sheet = hssfwb.GetSheetAt(0);
        //    }
        //    else
        //    {
        //        XSSFWorkbook hssfwb = new XSSFWorkbook(file.InputStream);
        //        sheet = hssfwb.GetSheetAt(0);
        //    }

        //    IRow row = null;
        //    ICell cell = null;

        //    string invoiceNo = "";

        //    for (int indexRow = 1; indexRow <= sheet.LastRowNum; indexRow++)
        //    {
        //        Boolean found = false;
        //        row = sheet.GetRow(indexRow);


        //        if (row != null) //null is when the row only contains empty cells 
        //        {
        //            int cellCount = row.LastCellNum;

        //            /*KOLOM Invoice No*/
        //            try
        //            {
        //                cell = row.GetCell(12);
        //                if (cell != null)
        //                {
        //                    if (!string.IsNullOrEmpty(cell.StringCellValue.Trim()))
        //                    {
        //                        invoiceNo = cell.StringCellValue.Trim();
        //                        //string[] strDate = cell.StringCellValue.Trim().Split('-');
        //                        //personel.TanggalExcel = cell.StringCellValue.Trim();
        //                        //personel.Tanggal = strDate[2] + "-" + strDate[1] + "-" + strDate[0];
        //                    }

        //                    //if (cell.NumericCellValue != null)
        //                    //{
        //                    //    TextValue tv = new TextValue();
        //                    //    tv.Text = "A";
        //                    //    tv.Value = cell.NumericCellValue.ToString();
        //                    //    //otTotal = double.Parse(tv.Value) + otTotal;
        //                    //    overtimesArray.Add(tv);
        //                    //}
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                errMesgs.Add(string.Format("Tidak bisa mengambil nilai TANGGAL di baris {0}, Pesan Error : {1}", indexRow + 1, "Format Tanggal salah, seharusnya DD-MM-YYYY"));
        //                break;
        //            }

        //        }

        //        //ajaxResult = invoiceCreationRepo.SaveData(overtimesArray, personel.Alasan,
        //        //   personel.Tanggal, NoRegLogin, username, temp.POSITION_ABBR,
        //        //  new AjaxResult(), personel.TanggalExcel);

        //        //finish: ;
        //    }

        //    return Json(ajaxResult);
        //}

        //public ActionResult UploadInvoice(List<HttpPostedFileBase> file)
        //{
        //    AjaxResult ajaxResult = new AjaxResult();

        //    Stream fis = file[0].InputStream;

        //    string filename = Path.GetFileName(Server.MapPath(file[0].FileName));
        //    var fileExt = Path.GetExtension(filename);

        //    if (fileExt == ".pdf")
        //    {
        //        //var fileStream = System.IO.File.Create("D:\\g\\file\\" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
        //        var fileStream = System.IO.File.Create("D:\\g\\file\\" + DateTime.Now.ToString("yyyyMMdd")+"_"+ file[0].FileName);
        //        fis.Seek(0, SeekOrigin.Begin);
        //        fis.CopyTo(fileStream);
        //        fileStream.Close();

        //        ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
        //        ajaxResult.Params = new string[] { "Sukses" };
        //    }
        //    else
        //    {
        //        ajaxResult.Result = AjaxResult.VALUE_ERROR;
        //        ajaxResult.Params = new String[] { 
        //                            string.Format("{0}", "File harus dalam bentuk .pdf")};
        //    }

        //    return Json(ajaxResult);
        //}

        public ActionResult UploadInvoice(HttpPostedFileBase file)
        {
            string path = null;

            try
            {
                SystemProperty sysProp =
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
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
            var directory = new DirectoryInfo(path);
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
                    (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
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

        public string getToleranceValue()
        {
            string result = "";
            result = invoiceCreationRevRepo.getToleranceValue();
            return result;
        }

        public string checkGRStatus(string param)
        {
            string result = "Y";
            result = invoiceCreationRevRepo.checkGRStatus(param);
            return result;

        }

        #region po category 20200515
        public string getPoCategory(string keys)
        {
            string result = "";
            result = invoiceCreationRevRepo.getPoCategory(keys);
            return result;
        }
        #endregion

        #region po category excel 20200717
        public string getPoCategoryExcel(string keys)
        {
            string result = "";
            result = invoiceCreationRevRepo.getPoCategoryExcel(keys);
            return result;
        }
        #endregion

        #region validasi VAT WHT 20200518
        public string getValidVATWHT(string suppCode)
        {
            string result = null;
            DateTime date = DateTime.Now;
            ViewData["data"] = invoiceCreationRevRepo.getValidVATWHT(suppCode);
            List<Supplier> datas = ViewData["data"] != null ? (List<Supplier>)ViewData["data"] : new List<Supplier>();
            if (datas.Count > 0)
            {
                foreach (Supplier supp in datas)
                {
                    //jika PKP = Y, SKB Valid, PLAT KUNING = N 
                    if ((supp.PKP_FLAG == "Y") && (supp.SKB_FLAG == "Y") && (date >= supp.SKB_VALID_FROM && date <= supp.SKB_VALID_TO) && (supp.PLAT_KUNING_FLAG == "N"))
                    {
                        result = "1";
                    }

                    //jika PKP = Y, SKB Invalid, PLAT KUNING = N
                    if ((supp.PKP_FLAG == "Y") && (supp.SKB_FLAG == "N") && (supp.PLAT_KUNING_FLAG == "N"))
                    {
                        result = "2";
                    }

                    //jika PKP = Y, SKB Invalid, PLAT KUNING = N
                    if ((supp.PKP_FLAG == "Y") && (supp.SKB_FLAG == "Y") && (date < supp.SKB_VALID_FROM || date > supp.SKB_VALID_TO) && (supp.PLAT_KUNING_FLAG == "N"))
                    {
                        result = "2";
                    }

                    //jika PKP = Y, SKB Valid, PLAT KUNING = Y
                    if ((supp.PKP_FLAG == "Y") && (supp.SKB_FLAG == "Y") && (date >= supp.SKB_VALID_FROM && date <= supp.SKB_VALID_TO) && (supp.PLAT_KUNING_FLAG == "Y"))
                    {
                        result = "3";
                    }

                    //jika PKP = Y, SKB Invalid, PLAT KUNING = Y
                    if ((supp.PKP_FLAG == "Y") && (supp.SKB_FLAG == "N") && (supp.PLAT_KUNING_FLAG == "Y"))
                    {
                        result = "4";
                    }

                    //jika PKP = Y, SKB Invalid, PLAT KUNING = Y
                    if ((supp.PKP_FLAG == "Y") && (supp.SKB_FLAG == "Y") && (date < supp.SKB_VALID_FROM || date > supp.SKB_VALID_TO) && (supp.PLAT_KUNING_FLAG == "Y"))
                    {
                        result = "4";
                    }

                    //jika PKP = N, SKB Invalid, PLAT KUNING = N
                    if ((supp.PKP_FLAG == "N") && (supp.SKB_FLAG == "N") && (supp.PLAT_KUNING_FLAG == "N"))
                    {
                        result = "5";
                    }

                    //jika PKP = N, SKB Invalid, PLAT KUNING = N
                    if ((supp.PKP_FLAG == "N") && (supp.SKB_FLAG == "Y") && (date < supp.SKB_VALID_FROM || date > supp.SKB_VALID_TO) && (supp.PLAT_KUNING_FLAG == "N"))
                    {
                        result = "5";
                    }

                }
            }
            return result;
        }
        #endregion

        #region get wht supp 20200714
        public string getSuppWHT(string suppCode)
        {
            string result = null;
            result = invoiceCreationRevRepo.getSuppWHT(suppCode);
            return result;
        }
        #endregion

        #region existInvo 20200720
        public string existInvo(string kode)
        {
            string result = null;
            result = invoiceCreationRevRepo.existInvo(kode);
            return result;
        }
        #endregion

        #region uploadExcel
        public ActionResult UploadDataFile(HttpPostedFileBase file, string uploadMode)
        {
            AjaxResult ajaxResult = new AjaxResult();
            IDBContext db = databaseManager.GetContext();
            try
            {
                IList<string> errMesg = new List<string>();
                IList<string> exception = new List<string>();
                IList<GR_IR_DATA> data = getDataLocalUploadExcel(file, errMesg, exception, db);

                //setAjaxResultMsg(errMesg, GetMemberName(() => errMesg), ajaxResult);
                //setAjaxResultMsg(exception, GetMemberName(() => exception), ajaxResult);

                

                #region CommentCode
                /*
                    if (errMesg.Count >= 1) {
                        string[] mesg = new string[errMesg.Count];
                        for (int i = 0; i < errMesg.Count; i++) {
                            mesg[i] = errMesg[i];
                            ajaxResult.ErrMesgs = mesg; 
                        }
                    }
                    if (exception.Count >= 1) {
                        string[] ex = new string[exception.Count];
                        for (int i = 0; i < exception.Count; i++) {
                            ex[i] = exception[i];
                            ajaxResult.ExceptionErrors = ex;
                        }
                    }
                 */
                #endregion
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.Params = new object[] { data };

                

            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ExceptionErrors = new string[] { string.Format(ex.GetType().FullName + " : <br>" + ex.Message) };
            }
            finally
            {
                db.Close();
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        //private void setAjaxResultMsg(IList<string> message, string varName, AjaxResult ajaxResult)
        //{
        //    if (message.Count >= 1)
        //    {
        //        string[] mesg = new string[message.Count];
        //        for (int i = 0; i < message.Count; i++)
        //        {
        //            mesg[i] = message[i];
        //            if (varName.Equals("errMesg"))
        //            {
        //                ajaxResult.ErrMesgs = mesg;
        //            }
        //            else if (varName.Equals("exception"))
        //            {
        //                ajaxResult.ExceptionErrors = mesg;
        //            }
        //        }
        //    }
        //}

        private IList<GR_IR_DATA> getDataLocalUploadExcel(HttpPostedFileBase file, IList<string> errMesgs, IList<string> exception, IDBContext db)
        {
            HSSFWorkbook hssfwb = null;
            //getDataCombobox();

            using (System.IO.Stream file2 = file.InputStream)
            {
                hssfwb = new HSSFWorkbook(file2);
            }

            if (hssfwb == null)
            {
                throw new ArgumentNullException("Cannot create workbook object from excel file " + file.FileName);
            }

            IRow row = null;
            ICell cell = null;
            IList<GR_IR_DATA> listGrIr = new List<GR_IR_DATA>();

            ISheet sheet = hssfwb.GetSheetAt(0);
            //if (!sheet.SheetName.Equals("RDDPPageList"))
            //{
            //    throw new Exception("The template doesn't match, please download the template first");
            //}
            //else
            //{
                for (int indexRow = DATA_ROW_INDEX_START; indexRow <= sheet.LastRowNum; indexRow++)
                {
                    row = sheet.GetRow(indexRow);
                    // null is when the row only contains empty cells
                    if (row != null)
                    {
                        GR_IR_DATA grIrData = new GR_IR_DATA();
                        //for (int x = 1; x < row.Cells.Count; x++) {
                        //    bool loopBreak = getCell(x, indexRow, cell, row, sheet, errMesgs, exception, rddpData, db);
                        //    if (!loopBreak) {
                        //        break;
                        //    }
                        //}

                        //rddpData.rddpTypePreview = (List<rddp>)ViewData["listRddpType"];
                        //rddpData.projectIdPreview = (List<rddp>)ViewData["listProjectCode"];
                        //rddpData.designIdPreview = (List<rddp>)ViewData["listDesignCode"];
                        //rddpData.suppIdPreview = (List<rddp>)ViewData["listSupplier"];

                        //Check if the excel has the last data or not
                        List<int> isFirstRowEmpty = new List<int>();
                        List<int> isSecondRowEmpty = new List<int>();
                        for (int i = 1; i < row.Cells.Count; i++)
                        {
                            if (row.GetCell(i) == null || row.GetCell(i).CellType == CellType.BLANK)
                            {
                                isFirstRowEmpty.Add(i);
                            }
                        }
                        if (isFirstRowEmpty.Count == (row.Cells.Count) - 1)
                        {
                            row = sheet.GetRow(indexRow + 1);
                            for (int j = 1; j < row.Cells.Count; j++)
                            {
                                if (row.GetCell(j) == null || row.GetCell(j).CellType == CellType.BLANK)
                                {
                                    isSecondRowEmpty.Add(j);
                                }
                            }
                            if (isSecondRowEmpty.Count == (row.Cells.Count) - 1)
                            {
                                break;
                            }
                            else
                            {
                                row = sheet.GetRow(indexRow);
                            }
                        }
                        for (int x = 0; x < row.Cells.Count; x++)
                        {
                            getCell(x, indexRow, cell, row, sheet, errMesgs, exception, grIrData, db);
                        }
                        listGrIr.Add(grIrData);
                    }
                }
            //}
            return listGrIr;
        }

        private void getCell(int x, int indexRow, ICell cell, IRow row, ISheet sheet, IList<string> errMesgs, IList<string> exception, GR_IR_DATA grIrData, IDBContext db)
        {
            try
            {
                cell = row.GetCell(x);
                //if (cell == null || cell.CellType == CellType.BLANK)
                //{
                //    //if (x == 1) {
                //    //    cell = row.GetCell(x + 1);
                //    //    if (indexRow != DATA_ROW_INDEX_START && (cell == null || cell.CellType == CellType.BLANK)) {
                //    //        return false;
                //    //    }
                //    //}
                //    row = sheet.GetRow(DATA_ROW_INDEX_START - 1);
                //    errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 4) + " Is Empty"));
                //    //errMesgs.Add("data kosong");
                //}
                //else
                //{
                    row = sheet.GetRow(DATA_ROW_INDEX_START - 1);
                    switch (x)
                    {
                        case 0:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else 
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.PO_NUMBER = Convert.ToString(cell.NumericCellValue);
                                }
                                if(cell.CellType == CellType.STRING)
                                {
                                    grIrData.PO_NUMBER = cell.StringCellValue;
                                }
                            }
                            break;
                        case 1:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.PO_ITEM = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.STRING) 
                                {
                                    grIrData.PO_ITEM = cell.StringCellValue;
                                }
                            }
                            break;
                        case 2:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                //errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                                grIrData.DN_NO = "";
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.DN_NO = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.STRING) 
                                { 
                                    grIrData.DN_NO = cell.StringCellValue; 
                                }
                            }
                            break;
                        case 3:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                //errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                                grIrData.DN_NO_ITEM = "";
                            }
                            else
                            {
                                if(cell.CellType == CellType.NUMERIC)
                                { 
                                    grIrData.DN_NO_ITEM = Convert.ToString(cell.NumericCellValue); 
                                }
                                if (cell.CellType == CellType.STRING)
                                {
                                    grIrData.DN_NO_ITEM = cell.StringCellValue;
                                }
                            }
                            break;
                        case 4:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                //errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                                grIrData.MAT_DESCR = "";
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.MAT_DESCR = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.STRING)
                                {
                                    grIrData.MAT_DESCR = cell.StringCellValue;
                                }
                            }
                            break;
                        case 5:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.PO_DATE = Convert.ToDateTime(cell.StringCellValue);
                            }
                            break;
                        case 6:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.MATDOC_DATE = Convert.ToDateTime(cell.StringCellValue);
                            }
                            break;
                        case 7:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.VEND_CODE = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.STRING)
                                {
                                    grIrData.VEND_CODE = cell.StringCellValue;
                                }
                            }
                            break;
                        case 8:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.SUPPLIER_NAME = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.SUPPLIER_NAME = cell.StringCellValue;
                                }
                            }
                            break;
                        case 9:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.MATDOC_NUMBER = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.STRING)
                                { 
                                    grIrData.MATDOC_NUMBER = cell.StringCellValue;
                                }
                            }
                            break;
                        case 10:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.MATDOC_ITEM = Convert.ToString(cell.NumericCellValue);
                                }
                                if (cell.CellType == CellType.STRING)
                                {
                                    grIrData.MATDOC_ITEM = cell.StringCellValue;
                                }
                            }
                            break;
                        case 11:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.MATDOC_QTY = Convert.ToDouble(cell.NumericCellValue);
                            }
                            break;
                        case 12:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.MATDOC_AMOUNT = Convert.ToDouble(cell.NumericCellValue);
                            }
                            break;
                        case 13:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                //errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                                grIrData.HEADER_TEXT = "";
                            }
                            else
                            {
                                if (cell.CellType == CellType.NUMERIC)
                                {
                                    grIrData.HEADER_TEXT = Convert.ToString(cell.NumericCellValue);
                                }
                                else if (cell.CellType == CellType.STRING)
                                {
                                    grIrData.HEADER_TEXT = cell.StringCellValue;
                                }
                            }
                            break;
                        case 14:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.MATDOC_CURRENCY = cell.StringCellValue;
                            }
                            break;
                        case 15:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow + 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.SYSTEM = cell.StringCellValue;
                            }
                            break;
                        case 16:
                            if (cell == null || cell.CellType == CellType.BLANK)
                            {
                                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty"));
                            }
                            else
                            {
                                grIrData.MATERIAL = cell.StringCellValue;
                            }
                            break;
                    }
                }
            //}
            catch (Exception ex)
            {
                row = sheet.GetRow(DATA_ROW_INDEX_START - 1);
                errMesgs.Add(string.Format(row.GetCell(x) + " at Row " + (indexRow - 1) + " Is Empty, Error Mesg : " + ex.Message));
            }
            //return true;
        }
        #endregion

        #region get gr ir 20200723
        public string getGrIr(string kode)
        {
            string result = "";
            result = invoiceCreationRevRepo.getGrIr(kode);
            return result;
        }
        #endregion

        #region checkgrstatus 20200814
        public string checkGrStatusExcel(string keyy)
        {
            string hasil = null;
            hasil = invoiceCreationRevRepo.checkGrStatusExcel(keyy);
            return hasil;
        }
        #endregion

        #region check Invoice Hardcopy Status
        public JsonResult checkInvoiceByHardcopyStatus(string supplierCode)
        {
            AjaxResult results = new AjaxResult();

            List<Invoice> invoiceList = invoiceCreationRevRepo.getInvoiceByHardcopyStatus(supplierCode);

            SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                CommonConstant.SYSTEM_CD_SUBMITTED_HARDCOPY, CommonConstant.SYSTEM_TYPE_CREATE_INVOICE);

            if (invoiceList != null && sysProp != null)
            {
                int totalInvoice = 0;
                DateTime now = DateTime.Now;
                int dayMax = 0;
                if (sysProp.SYSTEM_VALUE_TEXT != null && sysProp.SYSTEM_VALUE_TEXT.Length > 0)
                {
                    dayMax = int.Parse(sysProp.SYSTEM_VALUE_TEXT);
                }
                else
                {
                    dayMax = sysProp.SYSTEM_VALUE_NUM;
                }

                foreach (Invoice invoice in invoiceList)
                {
                    var diff = (now - ((DateTime)invoice.INVOICE_DATE)).Days;
                    if (diff > dayMax)
                    {
                        totalInvoice++;
                    }
                }

                if (totalInvoice > 0)
                {
                    Message msg = messageRepo.GetMessageById("INVCRE000001");

                    results.Result = AjaxResult.VALUE_ERROR;
                    results.ErrMesgs = new String[] {
                        string.Format(msg.MSG_TEXT, totalInvoice, dayMax)
                    };
                }
                else
                {
                    results.Result = AjaxResult.VALUE_SUCCESS;
                }
            }
            else
            {
                results.Result = AjaxResult.VALUE_SUCCESS;
            }

            // update 24-12-2020 [START]
            int amountStamp = int.Parse(systemPropertyRepo.GetSysPropByCodeAndType(
                            CommonConstant.STAMP, CommonConstant.AMOUNT_STAMP).SYSTEM_VALUE_TEXT);
            int amountInvoice = int.Parse(systemPropertyRepo.GetSysPropByCodeAndType(
                            CommonConstant.STAMP, CommonConstant.AMOUNT_INVOICE).SYSTEM_VALUE_TEXT);

            results.Params = new object[] { amountStamp, amountInvoice };
            // update 24-12-2020 [START]

            return Json(results);
        }
        #endregion

        #region Invoice Attachment
        public ActionResult UploadAttachment(string attachmentType, HttpPostedFileBase file)
        {
            AjaxResult ajaxResult = new AjaxResult();
            string path = null;

            if(file != null)
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

                //character
                SystemProperty FORBIDDEN_INVOICE_FILENAME = systemPropertyRepo.GetSysPropByCodeAndType(
                                "FORBIDDEN_INVOICE_FILENAME", CommonConstant.SYSTEM_TYPE_ATTACHMENT_FORMAT_FILENAME);

                foreach (char c  in FORBIDDEN_INVOICE_FILENAME.SYSTEM_VALUE_TEXT)
                {
                    if(file.FileName.Contains(c))
                    {
                        Message msg = messageRepo.GetMessageById("INVCRE000101");
                        ajaxResult.Result = AjaxResult.VALUE_ERROR;
                        ajaxResult.ErrMesgs = new string[] {
                            string.Format(msg.MSG_TEXT,FORBIDDEN_INVOICE_FILENAME.SYSTEM_VALUE_TEXT)
                        };
                        //"Invalid Character Set In File Name. Replace with Valid File Name ….!"
                        return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);
                    }
                }

                //ajaxResult.Result = AjaxResult.VALUE_ERROR;
                //ajaxResult.ErrMesgs = new string[] {
                //        string.Format("File name sesuai")
                //    };
                //return Content(new JavaScriptSerializer().Serialize(ajaxResult), "text/plain", System.Text.Encoding.UTF8);

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
                    try {
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

                        ajaxResult = invoiceCreationRevRepo.insertInvoiceAttachmentTemp(attachmentType, fileNameOrigin, fileNameServer, NoReg);
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
                ajaxResult = invoiceCreationRevRepo.deleteInvoiceAttachmentTemp(attachmentType, fileName, NoReg);
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

        public FileResult ViewAttachment(string fileName, string fileNameServer)
        {
            SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                        CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
            string path = sysProp.SYSTEM_VALUE_TEXT;

            string fileNameServerFullPath = Path.Combine(path, fileNameServer);
            if (System.IO.File.Exists(fileNameServerFullPath))
            {
                return File(fileNameServerFullPath, MimeExtensionHelper.GetMimeType(fileName));
            }
            else
            {
                return null;
            }
        }

        public ActionResult DeleteAllAttachment(List<InvoiceAttachment> attachments)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (InvoiceAttachment attachment in attachments)
                {
                    SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                            CommonConstant.SYSTEM_CD_UPLOAD_PATH_INVOICE, CommonConstant.SYSTEM_TYPE_UPLOAD_PATH);
                    string path = sysProp.SYSTEM_VALUE_TEXT;

                    string fileNameServerFullPath = Path.Combine(path, attachment.FILE_NAME_SERVER);
                    if (System.IO.File.Exists(fileNameServerFullPath))
                    {
                        System.IO.File.Delete(fileNameServerFullPath);
                    }

                    string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                    ajaxResult = invoiceCreationRevRepo.deleteInvoiceAttachmentTemp(attachment.ATTACHMENT_TYPE, attachment.FILE_NAME, NoReg);
                }
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

        #endregion

        public JsonResult getMessage(string msgID)
        {
            string result = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(msgID))
                {
                    Message msg = messageRepo.GetMessageById(msgID);
                    result = msg.MSG_TEXT;
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return Json(new { msg = result});
        }

        #region TVEST_AP23 - ONLINE_LIV02 Data Appearance (Tax Number) Inquiry
        public JsonResult validateEfakturLookup(string TAX_NO)
        {
            SystemProperty sysProp = (SystemProperty)systemPropertyRepo.GetSysPropByCodeAndType(
                    CommonConstant.SYSTEM_CD_EFAKTUR_EXPIRED, CommonConstant.SYSTEM_TYPE_CREATE_INV);

            string msg = string.Empty;

            try
            {
                msg = eFakturRepo.ValidateEfakturLookup(TAX_NO, sysProp.SYSTEM_VALUE_NUM);
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return Json(new { msg });
        }
        #endregion
    }
}
