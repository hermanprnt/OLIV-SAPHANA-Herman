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
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class InvoiceCreationController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private ReceivingListRepository receivingListRepo = ReceivingListRepository.Instance;
        private InvoiceCreationRepository invoiceCreationRepo = InvoiceCreationRepository.Instance;
        private EFakturRepository eFakturRepo = EFakturRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private GR_IRRepository grIrRepo = GR_IRRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;

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

            getInvoiceCreationList("", vendCodeLogin, "", "", "" , "",
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
        }

        public ActionResult search(string poNoSearch, string supplierSearch, string poDateSearch,
            string poText, string receivingDateSearch, string matDocNoSearch, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getInvoiceCreationList(poNoSearch, supplierSearch, poDateSearch, poText, receivingDateSearch, matDocNoSearch,
                page, size);

            return PartialView("_GridView");
        }

        private void getInvoiceCreationList(string poNoSearch, string supplierSearch, string poDateSearch,
            string poText, string receivingDateSearch, string matDocNoSearch, int page, int size)
        {
            int countdata = 0;
            countdata = invoiceCreationRepo.countInvoiceCreationList(poNoSearch, supplierSearch, poDateSearch, 
                CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            ViewData["InvoiceCreation"] = invoiceCreationRepo.GetInvoiceCreation(poNoSearch, supplierSearch,
                poDateSearch, CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch, pg.StartData, pg.EndData);
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
            string poText, string receivingDateSearch, string matDocNoSearch, int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = invoiceCreationRepo.countInvoiceCreationList(poNoSearch, supplierSearch, poDateSearch, 
                CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<String> result = new List<String>();
            result = invoiceCreationRepo.GetInvoiceCreationSort(poNoSearch, supplierSearch, poDateSearch, 
                CommonConstant.STATUS_GR_APPROVED, poText, receivingDateSearch, matDocNoSearch, pg.StartData, pg.EndData, 
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

        public ActionResult onLookupEfaktur(string Parameter, string VendCode, string PartialViewSearchAndInput, int Page)
        {
            if (PartialViewSearchAndInput.Equals("_LookupTableEfaktur"))
            {
                Paging pg = new Paging(eFakturRepo.countEfaktur(Parameter, VendCode), Page, CommonFunction.Instance.DefaultSize());

                ViewData["LookupPagingEfaktur"] = pg;

                ViewData["eFakturList"] = (List<EFaktur>)eFakturRepo.GetData(Parameter, VendCode, pg.StartData, pg.EndData);
            }

            return PartialView(PartialViewSearchAndInput);
        }

        public JsonResult SaveInvoice(Invoice invoice, List<GR_IR_DATA> items, List<HttpPostedFileBase> file)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                string seqCertificate = invoiceCreationRepo.getNewCertificateSeq(db, items[0].VEND_CODE, invoice.INVOICE_NO,
                    DateTime.Now.ToString("dd.MM.yyyy"));

                string certificateId = items[0].VEND_CODE + invoice.INVOICE_NO + DateTime.Now.ToString("yyyyMMdd") + seqCertificate;
                invoice.CERTIFICATE_ID = certificateId;

                results = invoiceCreationRepo.SaveInvoice(db, invoice, items, NoReg);

                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else if (AjaxResult.VALUE_SUCCESS.Equals(results.Result))
                {
                    db.CommitTransaction();

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

        //public void DownloadGR(string poNoSearch, string receivingDateSearch,
        //    string supplierSearch, string poDateSearch, string matDocNoSearch, string poTextSearch)
        //{
        //    string fileName = string.Format("InvoiceCreation{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //    // Get User Id
        //    string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;
        //    List<GR_IR_DATA> invoiceCreationList = invoiceCreationRepo.GetDownloadGR(poNoSearch, receivingDateSearch,
        //        supplierSearch, poDateSearch, matDocNoSearch, "APPROVED", poTextSearch);

        //    byte[] result = invoiceCreationRepo.GenerateDownloadFile(invoiceCreationList);
        //    SendToClientBrowser(fileName, result);
        //}

        //protected void SendToClientBrowser(string fileName, byte[] hasil)
        //{
        //    Response.Clear();
        //    //Response.ContentType = result.MimeType;
        //    Response.Cache.SetCacheability(HttpCacheability.Private);
        //    Response.Expires = -1;
        //    Response.Buffer = true;

        //    Response.ContentType = "application/octet-stream";
        //    Response.AddHeader("Content-Length", Convert.ToString(hasil.Length));

        //    Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", fileName));
        //    Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

        //    Response.BinaryWrite(hasil);
        //    Response.End();
        //}

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
    }
}
