using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models;
using consumable.Models.VAT_IN_H;
using Newtonsoft.Json;
using System.IO;
using CsvHelper;
using NPOI.HSSF.UserModel;
using Excel;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using consumable.Models.SUPPLIER;

namespace consumable.Controllers
{
    public class VAT_INController : PageController
    {

        private VAT_IN_HRepository vatInRepo = VAT_IN_HRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "e-Faktur Bridge - VAT IN";
            string success = Request.QueryString["success"];
            string name = Request.QueryString["name"];

            ViewData["success"] = success;
            ViewData["name"] = name;
            //ViewData["RecordsPerPage"] = SYSTEMRepository.Instance.GetRecordsPerPage();

            ViewData["VatIn"] = (List<VAT_IN_H>)vatInRepo.GetVatIn();
        }

        public ActionResult LoadTable(string id)
        {
            return PartialView("_VAT_In_Table");
        }

        public ActionResult SearchData(DateTime? TaxInvoiceDtFrm, DateTime? TaxInvoiceDtTo, string TaxInvoiceNoFrm,
            string TaxInvoiceNoTo, DateTime? SapPostDtFrm, DateTime? SapPostDtTo, string SapDocNoFrm, string SapDocNoTo,
            DateTime? ScanDtFrm, DateTime? ScanDtTo, string Name, DateTime? DLDtFrm, DateTime? DLDtTo, string NPWP,
            string Status, int Start, int Length)
        {
            string NPWPsearchFormatted = "";
            string TaxInvNoFromSearch = "";
            string TaxInvNoToSearch = "";
            NPWPsearchFormatted = NPWP.Replace(".", "");
            NPWPsearchFormatted = NPWPsearchFormatted.Replace("-", "");
            TaxInvNoFromSearch = TaxInvoiceNoFrm.Replace("-", "");
            TaxInvNoFromSearch = TaxInvNoFromSearch.Replace(".", "");
            TaxInvNoToSearch = TaxInvoiceNoTo.Replace("-", "");
            TaxInvNoToSearch = TaxInvNoToSearch.Replace(".", "");
            if (TaxInvNoFromSearch.Length == 16)
            {
                TaxInvNoFromSearch = TaxInvNoFromSearch.Substring(3, 13);
            }
            if (TaxInvNoToSearch.Length == 16)
            {
                TaxInvNoToSearch = TaxInvNoToSearch.Substring(3, 13);
            }

            //ViewData["VAT_IN_Count"] = CountIndex(VAT_IN_HRepository.Instance.CountData(TaxInvoiceDtFrm, TaxInvoiceDtTo,
            //    TaxInvNoFromSearch, TaxInvNoToSearch, SapPostDtFrm, SapPostDtTo, SapDocNoFrm, SapDocNoTo, ScanDtFrm, ScanDtTo,
            //    Name, DLDtFrm, DLDtTo, NPWPsearchFormatted, Status), Length);

            var List_VAT_IN = VAT_IN_HRepository.Instance.GetListData(TaxInvoiceDtFrm, TaxInvoiceDtTo, TaxInvNoFromSearch,
                TaxInvNoToSearch, SapPostDtFrm, SapPostDtTo, SapDocNoFrm, SapDocNoTo, ScanDtFrm, ScanDtTo, Name, DLDtFrm, DLDtTo,
                NPWPsearchFormatted, Status, ((Start - 1) * Length + 1), (Start * Length)).ToList();

            ViewData["VAT_IN"] = List_VAT_IN;


            if (List_VAT_IN.Count != 0)
            {
                ViewData["CurrentPage"] = Start;
                return PartialView("_VAT_IN_Table");
            }
            else
            {
                return PartialView("_NoDataFound");
            }
        }

        //private PagingModel CountIndex(int count, int length)
        //{
        //    PagingModel pg = new PagingModel();
        //    List<int> index = new List<int>();
        //    pg.Length = count;
        //    pg.CountData = count;

        //    //double total = Math.Round((double)count / (double)length);
        //    int total = 0;
        //    if (count % length != 0)
        //    {
        //        total = (count / length) + 1;
        //    }
        //    else
        //    {
        //        total = count / length;
        //    }

        //    for (int i = 1; i < total + 1; i++)
        //    {
        //        index.Add(i);
        //    }
        //    pg.IndexList = index;
        //    return pg;
        //}

        public ActionResult ViewEFaktur(string TaxInvoiceNo, string ReplacementFG)
        {
            ViewData["VAT_IN_H_Detail"] = VAT_IN_HRepository.Instance.GetDetail(TaxInvoiceNo, ReplacementFG);
            ViewData["VAT_IN_D_Detail"] = VAT_IN_DRepository.Instance.GetListDetail(TaxInvoiceNo, ReplacementFG);
            ViewData["Supplier_Data"] = SupplierRepository.Instance.GetDataByID(TaxInvoiceNo, ReplacementFG);
            ViewData["TMMIN_Data"] = SystemRepository.Instance.GetTMMIN();
            return PartialView("_ViewEFaktur_VAT_IN");
        }

        public ActionResult EditButtons()
        {
            return PartialView("_EditButtons");
        }

        [HttpPost]
        public ActionResult EditData(List<VAT_IN_Edit> list)
        {
            string TaxInvoiceNoFrm = "";
            string TaxInvoiceNoTo = "";
            int count = 0;
            int length = 0;
            foreach (var item in list)
            {
                if (count == 0)
                {
                    TaxInvoiceNoFrm = item.TAX_INVOICE_NO;
                }
                if (count == list.Count - 1)
                {
                    TaxInvoiceNoTo = item.TAX_INVOICE_NO;
                }
                VAT_IN_HRepository.Instance.EditData(item.TAX_INVOICE_NO, item.REPLACEMENT_FG, item.SAP_DOC_NO, item.SAP_POST_DT, item.STATUS);
                count++;
                length = item.Length;
            }
            return SearchData(null, null, TaxInvoiceNoFrm, TaxInvoiceNoTo, null, null, "", "", null, null, "", null, null, "", "", 1, length);
        }

        public FileResult DownloadCSV(string TaxInvNo, string ReplacementFG, string Status)
        {
            String[] TaxInvoiceNoList = JsonConvert.DeserializeObject<String[]>(TaxInvNo);
            String[] ReplacementFGList = JsonConvert.DeserializeObject<String[]>(ReplacementFG);
            string[] StatusList = JsonConvert.DeserializeObject<string[]>(Status);
            List<VAT_IN_H> TableList = new List<VAT_IN_H>();
            VAT_IN_H TableListTmp = new VAT_IN_H();

            for (int i = 0; i < TaxInvoiceNoList.Length; i++)
            {
                TableListTmp = VAT_IN_HRepository.Instance.GetListCSVDataByID(TaxInvoiceNoList[i], ReplacementFGList[i]);
                TableList.Add(TableListTmp);
                VAT_IN_HRepository.Instance.VAT_INUpdateStatusCSVDownload(TaxInvoiceNoList[i], ReplacementFGList[i], StatusList[i]);
            }

            string FileName = "Download VAT In Report_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            MemoryStream stream = new MemoryStream();
            StreamWriter write = new StreamWriter(stream);
            CsvWriter csw = new CsvWriter(write);
            //csw.Configuration.Quote = '"';
            csw.Configuration.QuoteAllFields = true;

            //Header
            csw.WriteField("FM");
            csw.WriteField("KD_JENIS_TRANSAKSI");
            csw.WriteField("FG_PENGGANTI");
            csw.WriteField("NOMOR_FAKTUR");
            csw.WriteField("MASA_PAJAK");
            csw.WriteField("TAHUN_PAJAK");
            csw.WriteField("TANGGAL_FAKTUR");
            csw.WriteField("NPWP");
            csw.WriteField("NAMA");
            csw.WriteField("ALAMAT_LENGKAP");
            csw.WriteField("JUMLAH_DPP");
            csw.WriteField("JUMLAH_PPN");
            csw.WriteField("JUMLAH_PPNBM");
            csw.WriteField("ID_CREDITABLE");
            csw.NextRecord();

            foreach (var item in TableList)
            {
                csw.WriteField("FM");
                csw.WriteField(@item.TRANS_TYPE_CODE.Trim());
                csw.WriteField(@item.REPLACEMENT_FG.Trim());
                csw.WriteField(@item.TAX_INVOICE_NO.Trim());
                csw.WriteField(@item.TAX_INVOICE_MONTH.Trim());
                csw.WriteField(@item.TAX_INVOICE_YEAR.Trim());
                csw.WriteField(@item.TAX_INVOICE_DT.ToString("dd/MM/yyyy"));
                csw.WriteField(@item.SUPPLIER.NPWP.Trim());
                csw.WriteField(@item.SUPPLIER.NAME.Trim());
                csw.WriteField(@item.SUPPLIER.ADDRESS);
                csw.WriteField(Math.Floor(@item.DPP_PRICE));
                csw.WriteField(Math.Floor(@item.VAT_PRICE));
                csw.WriteField(Math.Floor(@item.LUXURY_TAX_PRICE));
                csw.WriteField(@item.IS_CREDITABLE);
                csw.NextRecord();
            }

            write.Flush();
            write.Close();
            return File(stream.GetBuffer(), "text/csv", FileName);
        }

        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            string server = SystemRepository.Instance.GetServer();
            //string dir = SYSTEMRepository.Instance.GetPathFolderVAT_IN_Upload();
            string dir = Server.MapPath("~/VAT_IN_Upload/");
            string dirsuccess = Server.MapPath("~/VAT_IN_Success");
            string filename = file.FileName.ToString();
            string pathandfile = Path.Combine(dir, filename);
            //file.SaveAs(pathandfile);
            string pathandfilesuccess = Path.Combine(dirsuccess, filename);
            //file.SaveAs(pathandfile);
            FileStream stream;

            if (Directory.Exists(Server.MapPath("~/VAT_IN_Upload")))
            {
                file.SaveAs(pathandfile);
                stream = System.IO.File.Open(pathandfile, FileMode.Open, FileAccess.Read);
            }
            else
            {
                string pathString = System.IO.Path.Combine(Server.MapPath("~"), "VAT_IN_Upload/");
                System.IO.Directory.CreateDirectory(pathString);
                file.SaveAs(Path.Combine(pathString, filename));
                stream = System.IO.File.Open(Path.Combine(pathString, filename), FileMode.Open, FileAccess.Read);
            }
            if (!Directory.Exists(Server.MapPath("~/VAT_IN_Success/")))
            {
                string pathString = System.IO.Path.Combine(Server.MapPath("~"), "VAT_IN_Success");
                System.IO.Directory.CreateDirectory(pathString);
            }

            //INV_POSTED_SAPRepository.Instance.UploadToDatabase(ReadDataExcel(file.InputStream));
            List<INV_POSTED_SAP> ListINVSAP = new List<INV_POSTED_SAP>();
            IExcelDataReader excelReader = null;

            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
            if (HelperRepository.Instance.Right2(filename, 4) == ".xls")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                if (excelReader.IsValid == false)
                {
                    excelReader.Close();
                    if (server == "FID")
                    {
                        return Redirect("/VAT_IN/?success=invalidfile");
                    }
                    else
                    {
                        return Redirect("/" + server + "/VAT_IN/?success=invalidfile");
                    }
                }
            }
            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            else if (HelperRepository.Instance.Right2(filename, 4) == "xlsx")
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                if (excelReader.IsValid == false)
                {
                    excelReader.Close();
                    if (server == "FID")
                    {
                        return Redirect("/VAT_IN/?success=invalidfile");
                    }
                    else
                    {
                        return Redirect("/" + server + "/VAT_IN/?success=invalidfile");
                    }
                }
            }

            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet();

            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet results = excelReader.AsDataSet();

            INV_POSTED_SAP obj;
            int count = 0;
            //5. Data Reader methods
            while (excelReader.Read())
            {
                if (count == 0)
                {
                    if (excelReader.GetString(0) == null || excelReader.GetString(0).IndexOf("No. Vendor /") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }

                    }
                    if (excelReader.GetString(1) == null || excelReader.GetString(1).IndexOf("Customer / Vendor") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(2) == null || excelReader.GetString(2).IndexOf("Reference") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(3) == null || excelReader.GetString(3).IndexOf("NPWP") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(4) == null || excelReader.GetString(4).IndexOf("No. Seri Faktur") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(5) == null || excelReader.GetString(5).IndexOf("Due On") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }

                    if (excelReader.GetString(6) == null || excelReader.GetString(6).IndexOf("Document") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(7) == null || excelReader.GetString(7).IndexOf("PPN") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(8) == null || excelReader.GetString(8).IndexOf("PPN") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(9) == null || excelReader.GetString(9).IndexOf("Document No.") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                    if (excelReader.GetString(10) == null || excelReader.GetString(10).IndexOf("Clearing") == -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=invalidFormat");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=invalidFormat");
                        }
                    }
                }

                if ((excelReader.GetString(4) == null || excelReader.GetString(0).Trim() == "") && count > 2)
                {
                    goto end;
                }


                obj = new INV_POSTED_SAP();
                obj.VENDOR_NO = excelReader.GetString(0);
                obj.VENDOR_NAME = excelReader.GetString(1);
                obj.REFERENCE = excelReader.GetString(2);
                obj.NPWP = excelReader.GetString(3);
                obj.FAKTUR_PAJAK = excelReader.GetString(4);
                if (count > 1 && obj.FAKTUR_PAJAK.Trim() != "")
                {
                    try
                    {
                        obj.DUE_ON = DateTime.ParseExact(excelReader.GetString(5), "dd/MM/yyyy", null);
                    }
                    catch (Exception)
                    {
                        obj.DUE_ON = excelReader.GetDateTime(5);
                    }
                    if (obj.DUE_ON.Value.Year < 1753)
                    {
                        obj.DUE_ON = null;
                    }
                }
                else
                {
                    obj.DUE_ON = null;
                }

                if (obj.DUE_ON != null)
                {
                    if (excelReader.GetString(5) != null && excelReader.GetString(5).IndexOf(".") != -1)
                    {
                        if (server == "FID")
                        {
                            return Redirect("/VAT_IN/?success=validasiLengthFormat&name=DUE_ON");
                        }
                        else
                        {
                            return Redirect("/" + server + "/VAT_IN/?success=validasiLengthFormat&name=DUE_ON");
                        }
                    }
                }

                obj.DOC_CURRENCY = excelReader.GetString(6);
                obj.PPN_IN_DOC_CURR = excelReader.GetString(7);
                obj.PPN_IN_LOCAL_CURR = excelReader.GetString(8);
                obj.DOCUMENT_NO = excelReader.GetString(9);
                obj.CLEARING_DOC_NO = excelReader.GetString(10);
                ListINVSAP.Add(obj);
            end: ;
                obj = null;
                count++;
            }

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
            string p_processID = LOG_HRepository.Instance.GetProcessID();
            LOG_HRepository.Instance.LogHeaderStart(p_processID, "TMMIN01020101", "TMMIN01", "SYSTEM", null, "MTAX00003INB", "Interface VAT IN From SAP Process");
            //Session["processID"] = p_processID;
            INV_POSTED_SAPRepository.Instance.UploadToDatabase(ListINVSAP, p_processID, pathandfile, filename, pathandfilesuccess);
            //System.IO.File.Delete(@pathandfile);

            if (!System.IO.File.Exists(@pathandfilesuccess))
            {
                try
                {
                    System.IO.File.Move(@pathandfile, @pathandfilesuccess);
                }
                catch (Exception)
                {

                }

            }
            else
            {
                try
                {
                    System.IO.File.Delete(@pathandfile);
                }
                catch (Exception)
                {

                }

            }
            //return View("/"+server+"/VAT_IN");
            if (server == "FID")
            {
                return Redirect("/VAT_IN/?success=true");
            }
            else
            {
                return Redirect("/" + server + "/VAT_IN/?success=true");
            }

        }

        public List<INV_POSTED_SAP> ReadDataExcel(Stream str)
        {
            XSSFWorkbook hssfworkbook = new XSSFWorkbook(str);
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            int lastrow = sheet.LastRowNum - 4;
            List<INV_POSTED_SAP> ListINVSAP = new List<INV_POSTED_SAP>();
            if (sheet.SheetName == "Sheet1")
            {
                INV_POSTED_SAP obj;
                for (int i = 3; i <= lastrow; i++)
                {
                    obj = new INV_POSTED_SAP();
                    IRow row = sheet.GetRow(i);
                    obj.VENDOR_NO = row.GetCell(0).NumericCellValue.ToString();
                    obj.VENDOR_NAME = row.GetCell(1).StringCellValue;
                    obj.REFERENCE = row.GetCell(2).NumericCellValue.ToString();
                    obj.NPWP = row.GetCell(3).StringCellValue;
                    obj.FAKTUR_PAJAK = row.GetCell(4).NumericCellValue.ToString();
                    obj.DUE_ON = row.GetCell(5).DateCellValue;
                    obj.DOC_CURRENCY = row.GetCell(6).StringCellValue;
                    //obj.PPN_IN_DOC_CURR = row.GetCell(7).NumericCellValue;
                    //obj.PPN_IN_LOCAL_CURR = row.GetCell(8).NumericCellValue;
                    obj.DOCUMENT_NO = row.GetCell(9).NumericCellValue.ToString();
                    obj.CLEARING_DOC_NO = row.GetCell(10).NumericCellValue.ToString();
                    ListINVSAP.Add(obj);
                    obj = null;
                }
            }
            return ListINVSAP;
        }

        public void ExcelReport(DateTime? TaxInvoiceDtFrm, DateTime? TaxInvoiceDtTo, string TaxInvoiceNoFrm,
            string TaxInvoiceNoTo, DateTime? SapPostDtFrm, DateTime? SapPostDtTo, string SapDocNoFrm, string SapDocNoTo,
            DateTime? ScanDtFrm, DateTime? ScanDtTo, string Name, DateTime? DLDtFrm, DateTime? DLDtTo, string NPWP,
            string Status)
        {
            string NPWPsearchFormatted = "";
            string TaxInvNoFromSearch = "";
            string TaxInvNoToSearch = "";
            NPWPsearchFormatted = NPWP.Replace(".", "");
            NPWPsearchFormatted = NPWPsearchFormatted.Replace("-", "");
            TaxInvNoFromSearch = TaxInvoiceNoFrm.Replace("-", "");
            TaxInvNoFromSearch = TaxInvNoFromSearch.Replace(".", "");
            TaxInvNoToSearch = TaxInvoiceNoTo.Replace("-", "");
            TaxInvNoToSearch = TaxInvNoToSearch.Replace(".", "");
            if (TaxInvNoFromSearch.Length == 16)
            {
                TaxInvNoFromSearch = TaxInvNoFromSearch.Substring(3, 13);
            }
            if (TaxInvNoToSearch.Length == 16)
            {
                TaxInvNoToSearch = TaxInvNoToSearch.Substring(3, 13);
            }
            VAT_IN_EXCELREPORT Data = VAT_IN_HRepository.Instance.GetListDataExportExcel(TaxInvoiceDtFrm, TaxInvoiceDtTo,
                TaxInvNoFromSearch, TaxInvNoToSearch, SapPostDtFrm, SapPostDtTo, SapDocNoFrm, SapDocNoTo, ScanDtFrm, ScanDtTo,
                Name, DLDtFrm, DLDtTo, NPWPsearchFormatted, Status);
            var workbook = new HSSFWorkbook();
            string FileName = "VAT_IN_EXCEL_REPORT_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            if (Status == "")
            {
                Status = "All";
            }
            if (Status == "1")
            {
                Status = "Registerd";
            }
            if (Status == "2")
            {
                Status = "Posted";
            }
            if (Status == "3")
            {
                Status = "Downloaded";
            }

            string TaxInvDtFrom = ": ";
            string TaxInvDtTo = "To ";
            string TaxInvNoFrom = ": ";
            string TaxInvNoTo = "To ";
            string SapPostDtFrom = ": ";
            string SapPostDateTo = "To ";
            string SapDocNoFrom = ": ";
            string SapDocumentNoTo = "To ";
            string ScanDateFrom = ": ";
            string ScanDateTo = "To ";
            string DLDateFrom = ": ";
            string DLDateTo = "To ";
            string SuppName = ": ";
            string SuppNPWP = ": ";
            string Sts = ": ";

            if (TaxInvoiceDtFrm != null)
                TaxInvDtFrom = TaxInvDtFrom + System.Convert.ToDateTime(TaxInvoiceDtFrm).ToString("dd/MM/yyyy");
            else
                TaxInvDtFrom = TaxInvDtFrom + "-";

            if (TaxInvoiceDtTo != null)
                TaxInvDtTo = TaxInvDtTo + System.Convert.ToDateTime(TaxInvoiceDtTo).ToString("dd/MM/yyyy");
            else
                TaxInvDtTo = TaxInvDtTo + "-";

            if (TaxInvoiceNoFrm != null)
                if (TaxInvNoFrom.Length == 13)
                    TaxInvNoFrom = TaxInvNoFrom + HelperRepository.Instance.GenTaxInvoiceFormat(TaxInvoiceNoFrm.ToString());
                else
                    TaxInvNoFrom = TaxInvNoFrom + TaxInvoiceNoFrm.ToString();
            else
                TaxInvNoFrom = TaxInvNoFrom + "-";

            if (TaxInvoiceNoTo != null)
                if (TaxInvNoFrom.Length == 13)
                    TaxInvNoTo = TaxInvNoTo + HelperRepository.Instance.GenTaxInvoiceFormat(TaxInvoiceNoTo.ToString());
                else
                    TaxInvNoTo = TaxInvNoTo + TaxInvoiceNoTo.ToString();
            else
                TaxInvNoTo = TaxInvNoTo + "-";

            if (SapPostDtFrm != null)
                SapPostDtFrom = SapPostDtFrom + System.Convert.ToDateTime(SapPostDtFrm).ToString("dd/MM/yyyy");
            else
                SapPostDtFrom = SapPostDtFrom + "-";

            if (SapPostDtTo != null)
                SapPostDateTo = SapPostDateTo + System.Convert.ToDateTime(SapPostDtTo).ToString("dd/MM/yyyy");
            else
                SapPostDateTo = SapPostDateTo + "-";

            if (SapDocNoFrm != null)
                SapDocNoFrom = SapDocNoFrom + SapDocNoFrm.ToString();
            else
                SapDocNoFrom = SapDocNoFrom + "-";

            if (SapDocNoTo != null)
                SapDocumentNoTo = SapDocumentNoTo + SapDocNoTo.ToString();
            else
                SapDocumentNoTo = SapDocumentNoTo + "-";

            if (ScanDtFrm != null)
                ScanDateFrom = ScanDateFrom + System.Convert.ToDateTime(ScanDtFrm).ToString("dd/MM/yyyy");
            else
                ScanDateFrom = ScanDateFrom + "-";

            if (ScanDtTo != null)
                ScanDateTo = ScanDateTo + System.Convert.ToDateTime(ScanDtTo).ToString("dd/MM/yyyy");
            else
                ScanDateTo = ScanDateTo + "-";

            if (DLDtFrm != null)
                DLDateFrom = DLDateFrom + System.Convert.ToDateTime(DLDtFrm).ToString("dd/MM/yyyy");
            else
                DLDateFrom = DLDateFrom + "-";

            if (DLDtTo != null)
                DLDateTo = DLDateTo + System.Convert.ToDateTime(DLDtTo).ToString("dd/MM/yyyy");
            else
                DLDateTo = DLDateTo + "-";

            if (Name != null)
                SuppName = SuppName + Name.ToString();
            else
                SuppName = SuppName + "-";

            if (NPWP != null)
                if (NPWP.Length == 15)
                    SuppNPWP = SuppNPWP + HelperRepository.Instance.GenNPWPFormat(NPWP.ToString());
                else
                    SuppNPWP = SuppNPWP + NPWP.ToString();
            else
                SuppNPWP = SuppNPWP + "-";

            if (Status != "")
                Sts = ": " + Status;

            List<string[]> ListArr = new List<string[]>();
            string[] header1 = new string[1] { "VAT IN REPORT" };
            ListArr.Add(header1);
            string[] header2 = new string[1] { "PT. TOYOTA MANUFACTURING INDONESIA" };
            ListArr.Add(header2);
            string[] enter = new string[1] { "" };
            ListArr.Add(enter);
            string[] Criteria = new string[10] { "Tax Invoice Date ", "", TaxInvDtFrom, TaxInvDtTo, "", "", "", "Tax Invoice No ", TaxInvNoFrom, TaxInvNoTo };
            ListArr.Add(Criteria);
            string[] Criteria2 = new string[10] { "SAP Posting Date ", "", SapPostDtFrom, SapPostDateTo, "", "", "", "SAP Doc No ", SapDocNoFrom, SapDocumentNoTo };
            ListArr.Add(Criteria2);
            string[] Criteria3 = new string[9] { "Scan Date ", "", ScanDateFrom, ScanDateTo, "", "", "", "Supplier Name ", SuppName };
            ListArr.Add(Criteria3);
            string[] Criteria4 = new string[9] { "Downloaded Date ", "", DLDateFrom, DLDateTo, "", "", "", "Supplier NPWP ", SuppNPWP };
            ListArr.Add(Criteria4);
            string[] Criteria5 = new string[9] { "", "", "", "", "", "", "", "Status ", Sts };
            ListArr.Add(Criteria5);
            ListArr.Add(enter);

            string[] header3 = new string[] { "No", "Supplier Code", "Supplier Name", "Invoice No", "Supplier NPWP", "Tax Invoice No", 
                "Tax Invoice Date", "Tax Based", "VAT", "Luxury Tax", "SAP Doc No", "SAP Posting Date", "Status", "Scan Date", "Download Date", "Scan By" };
            ListArr.Add(header3);

            int i = 1;
            foreach (VAT_IN_H obj in Data.VAT_IN)
            {
                string SapPostDt = "";
                string DownloadDt = "";
                string TaxInvNoFormatted = "";
                string SuppNPWPFromatted = "";
                if (obj.TAX_INVOICE_NO.Length == 13)
                {
                    TaxInvNoFormatted = "01" + obj.REPLACEMENT_FG + "." + HelperRepository.Instance.GenTaxInvoiceFormat(obj.TAX_INVOICE_NO);
                }
                else
                {
                    TaxInvNoFormatted = obj.TAX_INVOICE_NO;
                }
                if (obj.SUPPLIER.NPWP.Length == 15)
                {
                    SuppNPWPFromatted = HelperRepository.Instance.GenNPWPFormat(obj.SUPPLIER.NPWP);
                }
                else
                {
                    SuppNPWPFromatted = obj.SUPPLIER.NPWP;
                }
                if (obj.SAP_POST_DT != null)
                {
                    SapPostDt = Convert.ToDateTime(obj.SAP_POST_DT).ToString("dd/MM/yyyy");
                }
                if (obj.DOWNLOAD_DT != null)
                {
                    DownloadDt = Convert.ToDateTime(obj.DOWNLOAD_DT).ToString("dd/MM/yyyy");
                }
                //string SupplierNPWP = HelperRepository.Instance.GenNPWPFormat(obj.SUPPLIER.NPWP);
                string[] myArr = new string[] {i.ToString(), obj.SUPPLIER.SUPPLIER_CODE, obj.SUPPLIER.NAME, obj.INVOICE_NO, SuppNPWPFromatted, 
                    TaxInvNoFormatted, obj.TAX_INVOICE_DT.ToString("dd/MM/yyyy"), obj.DPP_PRICE.ToString("#,0")+".00", obj.VAT_PRICE.ToString("#,0")+".00", 
                    obj.LUXURY_TAX_PRICE.ToString("#,0")+".00",obj.SAP_DOC_NO, SapPostDt, obj.SYSTEM.SYSTEM_VALUE_TEXT.ToString(), 
                    Convert.ToDateTime(obj.SCAN_DT).ToString("dd/MM/yyyy"), DownloadDt, obj.SCAN_BY };
                ListArr.Add(myArr);
                i++;
            }
            string[] Total = new string[] { "", "", "", "", "", "", "Total", Data.TotalDPP.ToString("#,0")+".00", Data.TotalVAT.ToString("#,0")+".00", 
                Data.TotalPPnBM.ToString("#,0")+".00", "", "", "", "", "", "" };
            ListArr.Add(Total);
            //workbook = CommonDownload.Instance.CreateObjectToSheet(ListArr, new VAT_IN_EXCELREPORT(), 9, 6, 1); //call function execute report
            using (var exportData = new MemoryStream()) //binding to stream reader
            {
                workbook.Write(exportData);
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.GetBuffer());
                Response.Flush();
                Response.End();
            }
        }
    }
}
