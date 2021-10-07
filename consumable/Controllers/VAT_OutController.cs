using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models;
using System.IO;
using CsvHelper;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Data.OleDb;
using Excel;
using NPOI.SS.UserModel;
using System.Web.UI.WebControls;
using System.Text;
using Toyota.Common.Credential;
using consumable.Models.VAT_OUT;

//using GemBox.Spreadsheet;

namespace consumable.Controllers
{
    public class VAT_OutController : PageController
    {
        //
        // GET: /VAT_Out/

        protected override void Startup()
        {
            Settings.Title = "VAT OUT";
            string success = Request.QueryString["success"];
            string name = Request.QueryString["name"];

            ViewData["name"] = name;
            ViewData["success"] = success;
            ViewData["VAT_OUT_List"] = VAT_OUTRepository.Instance.GetVAT_OUT();
            //ViewData["RecordsPerPage"] = SystemRepository.Instance.GetRecordsPerPage();
        }

        public ActionResult SearchVATOut(DateTime? TaxInvoiceDateFrom, DateTime? TaxInvoiceDateTo, DateTime? InterfaceDateFrom,
                     DateTime? InterfaceDateTo, DateTime? DownloadDateFrom, DateTime? DownloadDateTo, DateTime? CompleteDateFrom,
                     DateTime? CompleteDateTo, String TaxInvoiceNoFrom, String TaxInvoiceNoTo, String CustomerName,
                     String CustomerNPWP, String Status, int Start, int Length)
        {
            //ViewData["VAT_OUT_List"] = VAT_OUT_HRepository.Instance.GetListData(TaxInvoiceDateFrom, TaxInvoiceDateTo, InterfaceDateFrom, InterfaceDateTo, DownloadDateFrom, DownloadDateTo, CompleteDateFrom, CompleteDateTo, TaxInvoiceNoFrom, TaxInvoiceNoTo, CustomerName, CustomerNPWP, Status, Start, Length);
            ViewData["VAT_OUT_List"] = VAT_OUT_HRepository.Instance.GetListData(TaxInvoiceDateFrom, TaxInvoiceDateTo, InterfaceDateFrom, InterfaceDateTo, DownloadDateFrom, DownloadDateTo, CompleteDateFrom, CompleteDateTo, TaxInvoiceNoFrom, TaxInvoiceNoTo, CustomerName, CustomerNPWP, Status, ((Start - 1) * Length + 1), (Start * Length));
            //ViewData["VAT_OUTCount"] = CountIndex(VAT_OUT_HRepository.Instance.CountData(TaxInvoiceDateFrom, TaxInvoiceDateTo, InterfaceDateFrom, InterfaceDateTo, DownloadDateFrom, DownloadDateTo, CompleteDateFrom, CompleteDateTo, TaxInvoiceNoFrom, TaxInvoiceNoTo, CustomerName, CustomerNPWP, Status), Length);
            ViewData["CurrentPage"] = Start;
            return PartialView("_ViewTable");
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

        //Added By Yayan--
        public ActionResult ViewEFaktur(string TaxInvoiceNo, string DebitAdviceNo, string ReplacementFG, string CustomerNPWP, string CustomerName)
        {
            ViewData["VAT_OUT_H_Detail"] = VAT_OUT_HRepository.Instance.GetDetail(TaxInvoiceNo, DebitAdviceNo, ReplacementFG);
            ViewData["VAT_OUT_D_Detail"] = VAT_OUT_DRepository.Instance.GetListDataByID(TaxInvoiceNo, DebitAdviceNo, ReplacementFG);
            ViewData["Customer_Data"] = CUSTOMERRepository.Instance.GetDataByID(TaxInvoiceNo, ReplacementFG, DebitAdviceNo);
            ViewData["TMMIN_Data"] = SystemRepository.Instance.GetTMMIN();
            return PartialView("_ViewEFaktur_VAT_OUT");
        }
        //Added By Yayan--

        public FileResult DownloadCSV(String TaxInvoiceNo, String ReplacementFG, String DebitAdvNo, String Status)
        {
            List<VAT_OUT_CSV> ListVAT_OUT = new List<VAT_OUT_CSV>();


            //List<string[]> data = JsonConvert.DeserializeObject<List<string[]>>(TaxInvoiceNo);

            String[] TaxInvoiceNoList = JsonConvert.DeserializeObject<String[]>(TaxInvoiceNo);
            String[] ReplacementFGList = JsonConvert.DeserializeObject<String[]>(ReplacementFG);
            String[] DebitAdvNoList = JsonConvert.DeserializeObject<String[]>(DebitAdvNo);
            String[] StatusList = JsonConvert.DeserializeObject<String[]>(Status);

            var row2 = SystemRepository.Instance.GetVAT_OUT_H_CSV();
            List<VAT_OUT_CSV> row3 = new List<VAT_OUT_CSV>();
            VAT_OUT_CSV VAT_OUT_CSVTemp = new VAT_OUT_CSV();
            for (int i = 0; i < TaxInvoiceNoList.Count(); i++)
            {
                VAT_OUT_CSVTemp = VAT_OUT_CSVRepository.Instance.GetListData(TaxInvoiceNoList[i], ReplacementFGList[i], DebitAdvNoList[i]);
                row3.Add(VAT_OUT_CSVTemp);
                VAT_OUT_HRepository.Instance.VAT_OUTUpdateStatusCSVDownload(TaxInvoiceNoList[i], ReplacementFGList[i], DebitAdvNoList[i], StatusList[i]);
            }

            string FileName = "Download VAT Out Report_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            MemoryStream stream = new MemoryStream();
            StreamWriter write = new StreamWriter(stream);
            CsvWriter csw = new CsvWriter(write);
            //csw.Configuration.Quote = '"';
            csw.Configuration.QuoteAllFields = true;

            //---------------HEADER 1----------------------
            csw.WriteField("FK");
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
            csw.WriteField("ID_KETERANGAN_TAMBAHAN");
            csw.WriteField("FG_UANG_MUKA");
            csw.WriteField("UANG_MUKA_DPP");
            csw.WriteField("UANG_MUKA_PPN");
            csw.WriteField("UANG_MUKA_PPNBM");
            csw.WriteField("REFERENSI");
            csw.NextRecord();

            //---------------HEADER 2 ----------------
            csw.WriteField("LT");
            csw.WriteField("NPWP");
            csw.WriteField("NAMA");
            csw.WriteField("JALAN");
            csw.WriteField("BLOK");
            csw.WriteField("NOMOR");
            csw.WriteField("RT");
            csw.WriteField("RW");
            csw.WriteField("KECAMATAN");
            csw.WriteField("KELURAHAN");
            csw.WriteField("KABUPATEN");
            csw.WriteField("PROPINSI");
            csw.WriteField("KODE_POS");
            csw.WriteField("NOMOR_TELEPON");
            csw.NextRecord();

            //---------------HEADER 3 ----------------
            csw.WriteField("OF");
            csw.WriteField("KODE_OBJEK");
            csw.WriteField("NAMA");
            csw.WriteField("HARGA_SATUAN");
            csw.WriteField("JUMLAH_BARANG");
            csw.WriteField("HARGA_TOTAL");
            csw.WriteField("DISKON");
            csw.WriteField("DPP");
            csw.WriteField("PPN");
            csw.WriteField("TARIF_PPNBM");
            csw.WriteField("PPNBM");
            csw.NextRecord();

            //----------TABLE LIST------------
            foreach (var item in row3)
            {
                //TABLE LIST ROW 1
                string downpayment_dpp = "";
                string downpayment_vat = "";
                string downpayment_lt = "";

                if (Convert.ToDouble(item.DOWN_PAYMENT_DPP) == 0.00)
                {
                    downpayment_dpp = "0";
                }
                else
                {
                    downpayment_dpp = item.DOWN_PAYMENT_DPP.ToString();
                }
                if (Convert.ToDouble(item.DOWN_PAYMENT_VAT) == 0.00)
                {
                    downpayment_vat = "0";
                }
                else
                {
                    downpayment_vat = item.DOWN_PAYMENT_VAT.ToString();
                }
                if (Convert.ToDouble(item.DOWN_PAYMENT_LT) == 0.00)
                {
                    downpayment_lt = "0";
                }
                else
                {
                    downpayment_lt = item.DOWN_PAYMENT_LT.ToString();
                }
                //TABLE LIST ROW 1
                csw.WriteField("FK");
                csw.WriteField(item.TRANS_TYPE_CODE);
                csw.WriteField(item.REPLACEMENT_FG);
                csw.WriteField(item.TAX_INVOICE_NO);
                csw.WriteField(item.TAX_INVOICE_MONTH);
                csw.WriteField(item.TAX_INVOICE_YEAR);
                csw.WriteField(item.TAX_INVOICE_DATE.ToString("dd/MM/yyyy"));
                csw.WriteField(item.NPWP);
                csw.WriteField(item.NAME);
                csw.WriteField(item.ADDRESS);
                csw.WriteField(Math.Floor(item.DPP_PRICE));
                csw.WriteField(Math.Floor(item.VAT_PRICE));
                csw.WriteField(Math.Floor(item.LUXURY_TAX_PRICE));
                csw.WriteField(item.ADD_REMARK_ID);
                csw.WriteField(Math.Floor(item.DOWN_PAYMENT_FG));
                csw.WriteField(downpayment_dpp);
                csw.WriteField(downpayment_vat);
                csw.WriteField(downpayment_lt);
                csw.WriteField(item.REFERENCE);
                csw.NextRecord();


                //TABLE LIST ROW 2
                csw.WriteField("LT");
                int count = 0;
                foreach (var itemRow2 in row2)
                {
                    if(count == 0)
                    {
                        string npwp = itemRow2.SYSTEM_VALUE_TEXT.Replace(".","");
                        npwp = npwp.Replace("-","");
                        csw.WriteField(npwp);
                    }
                    else
                    {
                        csw.WriteField(itemRow2.SYSTEM_VALUE_TEXT);
                    }
                    count++;
                }
                csw.NextRecord();

                foreach (var itemDetail in item.VAT_OUT_DETAIL)
                {
                    csw.WriteField("OF");
                    csw.WriteField(itemDetail.PRODUCT_CODE);
                    csw.WriteField(itemDetail.PRODUCT_NM);
                    csw.WriteField(itemDetail.UNIT_PRICE);
                    csw.WriteField(itemDetail.PRODUCT_QTY);
                    csw.WriteField(itemDetail.TOTAL_PRICE);
                    csw.WriteField(itemDetail.DISC_PRICE);
                    csw.WriteField(itemDetail.DPP_PRICE);
                    csw.WriteField(itemDetail.VAT_PRICE);
                    csw.WriteField(itemDetail.LUXURY_TAX_PERC);
                    csw.WriteField(itemDetail.LUXURY_TAX_PRICE);
                    csw.NextRecord();
                }
                //csw.NextRecord();
            }
            write.Flush();
            write.Close();
            return File(stream.GetBuffer(), "text/csv", FileName);
        }

        public void ExcelReport(DateTime? TaxInvoiceDateFrom, DateTime? TaxInvoiceDateTo, DateTime? InterfaceDateFrom,
                     DateTime? InterfaceDateTo, DateTime? DownloadDateFrom, DateTime? DownloadDateTo, DateTime? CompleteDateFrom,
                     DateTime? CompleteDateTo, String TaxInvoiceNoFrom, String TaxInvoiceNoTo, String CustomerName,
                     String CustomerNPWP, String Status)
        {
            VAT_OUT_EXCELREPORT Data = VAT_OUT_EXCELRepository.Instance.GetListData(TaxInvoiceDateFrom, TaxInvoiceDateTo, InterfaceDateFrom, InterfaceDateTo, DownloadDateFrom, DownloadDateTo, CompleteDateFrom, CompleteDateTo, TaxInvoiceNoFrom, TaxInvoiceNoTo, CustomerName, CustomerNPWP, Status);
            var workbook = new HSSFWorkbook();
            StringBuilder filename = new StringBuilder();
            filename.Append("VAT_Out_Excel_Report_").Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
            string FileName = filename.ToString();
            string TaxInvDtFrom = ": ";
            string TaxInvDtTo = "To ";
            string InterfaceDtFrom = ": ";
            string InterfaceDtTo = "To ";
            string DownloadDtFrom = ": ";
            string DownloadDtTo = "To ";
            string CompleteDtFrom = ": ";
            string CompleteDtTo = "To ";
            string TaxInvNoFrom = ": ";
            string TaxInvNoTo = "To ";
            string CustomerNM = ": -";
            string CustNPWP = ": -";
            string Sts = ": All";

            if (TaxInvoiceDateFrom != null)
                TaxInvDtFrom = TaxInvDtFrom + System.Convert.ToDateTime(TaxInvoiceDateFrom).ToString("dd/MM/yyyy");
            else
                TaxInvDtFrom = TaxInvDtFrom + "-";

            if (TaxInvoiceDateTo != null)
                TaxInvDtTo = TaxInvDtTo + System.Convert.ToDateTime(TaxInvoiceDateTo).ToString("dd/MM/yyyy");
            else
                TaxInvDtTo = TaxInvDtTo + "-";

            if (InterfaceDateFrom != null)
                InterfaceDtFrom = InterfaceDtFrom + System.Convert.ToDateTime(InterfaceDateFrom).ToString("dd/MM/yyyy");
            else
                InterfaceDtFrom = InterfaceDtFrom + "-";

            if (InterfaceDateTo != null)
                InterfaceDtTo = InterfaceDtTo + System.Convert.ToDateTime(InterfaceDateTo).ToString("dd/MM/yyyy");
            else
                InterfaceDtTo = InterfaceDtTo + "-";

            if (DownloadDateFrom != null)
                DownloadDtFrom = DownloadDtFrom + System.Convert.ToDateTime(DownloadDateFrom).ToString("dd/MM/yyyy");
            else
                DownloadDtFrom = DownloadDtFrom + "-";

            if (DownloadDateTo != null)
                DownloadDtTo = DownloadDtTo + System.Convert.ToDateTime(DownloadDateTo).ToString("dd/MM/yyyy");
            else
                DownloadDtTo = DownloadDtTo + "-";

            if (CompleteDateFrom != null)
                CompleteDtFrom = CompleteDtFrom + System.Convert.ToDateTime(CompleteDateFrom).ToString("dd/MM/yyyy");
            else
                CompleteDtFrom = CompleteDtFrom + "-";

            if (CompleteDateTo != null)
                CompleteDtTo = CompleteDtTo + System.Convert.ToDateTime(CompleteDateTo).ToString("dd/MM/yyyy");
            else
                CompleteDtTo = CompleteDtTo + "-";

            if (TaxInvoiceNoFrom != "")
                TaxInvNoFrom = TaxInvNoFrom + TaxInvoiceNoFrom.ToString();
            else
                TaxInvNoFrom = TaxInvNoFrom + "-";

            if (TaxInvoiceNoTo != "")
                TaxInvNoTo = TaxInvNoTo + TaxInvoiceNoTo.ToString();
            else
                TaxInvNoTo = TaxInvNoTo + "-";

            if (CustomerName != "")
                CustomerNM = ": " + CustomerName;

            if (CustomerNPWP != "")
                CustNPWP = ": " + CustomerNPWP;
            if (Status == "1")
                Status = "Registered";
            if (Status == "2")
                Status = "Downloaded";
            if (Status == "3")
                Status = "Completed";
            if (Status != "")
                Sts = ": " + Status;


            List<string[]> ListArr = new List<string[]>();
            string[] header1 = new string[1] { "VAT OUT REPORT" };
            ListArr.Add(header1);
            string[] header2 = new string[1] { "PT. TOYOTA MANUFACTURING INDONESIA" };
            ListArr.Add(header2);
            string[] enter = new string[1] { "" };
            ListArr.Add(enter);
            string[] Criteria = new string[13] { "Tax Invoice Date ", "", TaxInvDtFrom, TaxInvDtTo, "", "", "", "", "", "Tax Invoice No ", "", TaxInvNoFrom, TaxInvNoTo };
            ListArr.Add(Criteria);
            string[] Criteria2 = new string[12] { "Interface Date ", "", InterfaceDtFrom, InterfaceDtTo, "", "", "", "", "", "Customer NPWP ", "", CustNPWP };
            ListArr.Add(Criteria2);
            string[] Criteria3 = new string[12] { "Download Date ", "", DownloadDtFrom, DownloadDtTo, "", "", "", "", "", "Customer Name ", "", CustomerNM };
            ListArr.Add(Criteria3);
            string[] Criteria4 = new string[12] { "Complete Date ", "", CompleteDtFrom, CompleteDtTo, "", "", "", "", "", "Status ", "", Sts };
            ListArr.Add(Criteria4);
            ListArr.Add(enter);
            string[] header3 = new string[15] { "No", "Customer NPWP", "Customer Name", "Tax Invoice No", "Tax Invoice Date", "Tax Based", "VAT", "Luxury Tax", "Business Unit", 
                "SAP Doc No", "Debit Advice No", "Status", "Interface Date", "Download Date", "Complete Date" };
            ListArr.Add(header3);
            int i = 1;
            foreach (VAT_OUT_H obj in Data.VAT_OUT)
            {
                string DownloadDt = "";
                string CompleteDt = "";
                if(obj.DOWNLOAD_DT!=null)
                {
                    DownloadDt = Convert.ToDateTime(obj.DOWNLOAD_DT).ToString("dd/MM/yyyy");
                }
                if(obj.COMPLETE_DT!=null)
                {
                    CompleteDt = Convert.ToDateTime(obj.DOWNLOAD_DT).ToString("dd/MM/yyyy");
                }
                string[] myArr = new string[15] {i.ToString(), obj.CUSTOMER.NPWP, obj.CUSTOMER.NAME, obj.TAX_INVOICE_NO, obj.TAX_INVOICE_DATE.ToString("dd/MM/yyyy"), obj.DPP_PRICE.ToString("#,0"), obj.VAT_PRICE.ToString("#,0"), obj.LUXURY_TAX_PRICE.ToString("#,0"),
                obj.BUSINESS_UNIT, obj.SAP_DOC_NO, obj.DEBIT_ADVICE_NO, obj.STATUS_NAME.SYSTEM_VALUE_TEXT, Convert.ToDateTime(obj.INTERFACE_DT).ToString("dd/MM/yyyy"), DownloadDt, CompleteDt};
                ListArr.Add(myArr);
                i++;
            }
            string[] Total = new string[15] { "", "", "", "", "Total", Data.TotalDPP.ToString("#,0"), Data.TotalVAT.ToString("#,0"), Data.TotalPPnBM.ToString("#,0"), "", "", "", "", "", "", "" };
            ListArr.Add(Total);
            //workbook = CommonDownload.Instance.CreateObjectToSheet(ListArr, new VAT_OUT_EXCELREPORT(), 8, 4, 2); //call function execute report
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


        public ActionResult UploadTaxInvFromSAP(HttpPostedFileBase fileUpload, FormCollection form)
        {
            var fileName = Path.GetFileName(fileUpload.FileName);
            var uplFileDir = Path.Combine(Server.MapPath("~/TaxInvoiceDir_Upload"), fileName);
            var successUplDir = Path.Combine(Server.MapPath("~/TaxInvoiceDir_Success"));
            var failedUplDir = Path.Combine(Server.MapPath("~/TaxInvoiceDir_Failed"));

            if (Directory.Exists(Server.MapPath("~/TaxInvoiceDir_Upload")))
            {
                fileUpload.SaveAs(uplFileDir);
            }
            else
            {
                string pathString = System.IO.Path.Combine(Server.MapPath("~"), "TaxInvoiceDir_Upload");
                System.IO.Directory.CreateDirectory(pathString);
                fileUpload.SaveAs(uplFileDir);
            }



            return RedirectToAction("");
        }

        [HttpPost]
        public ActionResult SaveTempData(HttpPostedFileBase fileUpload)
        {
            string server = SystemRepository.Instance.GetServer();
            DataTable dtTemp = new DataTable();
            DataTable dtTaxInv = new DataTable();
            User u = Lookup.Get<User>();
            string username = u.Name;

            if (fileUpload != null)
            {
                string p_processID = LOG_HRepository.Instance.GetProcessID();
                VAT_OUT_HRepository.Instance.DeleteTempData();

                var fileName = Path.GetFileName(fileUpload.FileName);
                var uplFileDir = Path.Combine(Server.MapPath("~/TaxInvoiceDir_Upload"));
                var successUplDir = Path.Combine(Server.MapPath("~/TaxInvoiceDir_Success"));
                var failedUplDir = Path.Combine(Server.MapPath("~/TaxInvoiceDir_Failed"));

                string pathandfile = Path.Combine(uplFileDir, fileName);
                string pathandfilesuccess = Path.Combine(successUplDir, fileName);
                DataRow rowTemp = dtTemp.NewRow();

                List<VAT_OUT_SAP_Model> VAT_OUT_SAPList = new List<VAT_OUT_SAP_Model>();
                FileStream stream;

                /*Upload Directory*/
                if (Directory.Exists(Server.MapPath("~/TaxInvoiceDir_Upload")))
                {
                    //fileUpload.SaveAs(uplFileDir);
                    fileUpload.SaveAs(pathandfile);
                    //stream = System.IO.File.Open(uplFileDir, FileMode.Open, FileAccess.Read);
                    stream = System.IO.File.Open((Path.Combine(uplFileDir, fileName)), FileMode.Open, FileAccess.Read);
                }
                else
                {
                    uplFileDir = System.IO.Path.Combine(Server.MapPath("~"), "TaxInvoiceDir_Upload");
                    System.IO.Directory.CreateDirectory(uplFileDir);
                    //fileUpload.SaveAs(Path.Combine(uplFileDir, fileName));
                    //fileUpload.SaveAs(uplFileDir);
                    fileUpload.SaveAs(pathandfile);

                    stream = System.IO.File.Open((Path.Combine(uplFileDir, fileName)), FileMode.Open, FileAccess.Read);
                }

                /*Success Directory*/
                if (!Directory.Exists(Server.MapPath("~/TaxInvoiceDir_Success/")))
                {
                    string pathString = System.IO.Path.Combine(Server.MapPath("~"), "TaxInvoiceDir_Success");
                    System.IO.Directory.CreateDirectory(pathString);
                }

                IExcelDataReader excelReader = null;

                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                if (HelperRepository.Instance.Right2(fileName, 4) == ".xls")
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                else if (HelperRepository.Instance.Right2(fileName, 4) == "xlsx")
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                DataSet result = excelReader.AsDataSet();

                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet results = excelReader.AsDataSet();

                VAT_OUT_SAP_Model obj;

                int indexRow = 0;


                while (excelReader.Read())
                {
                    obj = new VAT_OUT_SAP_Model();

                    try
                    {

                        if (indexRow == 0)
                        {
                            //if (excelReader.GetString(0) != "TAX_INVOICE_NO") throw new System.InvalidOperationException("Cell " + excelReader.GetString(0) + "uncorrect");
                            if (excelReader.GetString(0) == null || excelReader.GetString(0).IndexOf("TAX_INVOICE_NO") == -1) 
                            { 
                                if(server=="FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(1) == null || excelReader.GetString(1).IndexOf("REPLACEMENT_FG") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(2) == null || excelReader.GetString(2).IndexOf("DEBIT_ADVICE_NO") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(3) == null || excelReader.GetString(3).IndexOf("TRANS_TYPE_CODE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(4) == null || excelReader.GetString(4).IndexOf("INVOICE_MONTH") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(5) == null || excelReader.GetString(5).IndexOf("INVOICE_YEAR") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(6) == null || excelReader.GetString(6).IndexOf("INVOICE_DATE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(7) == null || excelReader.GetString(7).IndexOf("DPP_PRICE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(8) == null || excelReader.GetString(8).IndexOf("VAT_PRICE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(9) == null || excelReader.GetString(9).IndexOf("LUXURY_TAX_PRICE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(10) == null || excelReader.GetString(10).IndexOf("REFERENCE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(11) == null || excelReader.GetString(11).IndexOf("ADD_REMARK_ID") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(12) == null || excelReader.GetString(12).IndexOf("BUSINESS_UNIT") == -1)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                            }
                            if (excelReader.GetString(13) == null || excelReader.GetString(13).IndexOf("SAP_DOC_NO") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(14) == null || excelReader.GetString(14).IndexOf("PRODUCT_CODE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(15) == null || excelReader.GetString(15).IndexOf("PRODUCT_NM") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(16) == null || excelReader.GetString(16).IndexOf("UNIT_PRICE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(17) == null || excelReader.GetString(17).IndexOf("PRODUCT_QTY") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(18) == null || excelReader.GetString(18).IndexOf("TOTAL_PRICE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(19) == null || excelReader.GetString(19).IndexOf("DISC_PRICE") == -1)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                            }
                            if (excelReader.GetString(20) == null || excelReader.GetString(20).IndexOf("DPP_PRICE") == -1)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                            }
                            if (excelReader.GetString(21) == null || excelReader.GetString(21).IndexOf("VAT_PRICE") == -1)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                            }
                            if (excelReader.GetString(22) == null || excelReader.GetString(22).IndexOf("LUXURY_TAX_PERC") == -1)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                            }
                            if (excelReader.GetString(23) == null || excelReader.GetString(23).IndexOf("LUXURY_TAX_PRICE") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(24) == null || excelReader.GetString(24).IndexOf("NAME") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(25) == null || excelReader.GetString(25).IndexOf("NPWP") == -1)
                            {
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                            }
                            if (excelReader.GetString(26) == null || excelReader.GetString(26).IndexOf("ADDRESS") == -1)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=invalidFormat");
                                }
                                else
                                {
                                    return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                                }
                                //return Redirect("/"+server+"/VAT_Out/?success=invalidFormat");
                            }

                        }
                        else if (excelReader.GetString(0) == null && indexRow > 0)
                        {
                            goto end;
                        }
                        else
                        {
                            if (excelReader.GetString(0).Length > 10)
                            {
                                if(server=="FID")
                                {
                                    return Redirect("/VAT_Out/?success=validasiLengthFormat&name=TAX_INVOICE_NO");
                                }
                                else
                                {
                                    return Redirect("/" + server + "/VAT_Out/?success=validasiLengthFormat&name=TAX_INVOICE_NO");
                                }
                                
                            }
                            obj.TAX_INVOICE_NO = excelReader.GetString(0);
                            obj.REPLACEMENT_FG = excelReader.GetString(1);
                            obj.DEBIT_ADVICE_NO = excelReader.GetString(2);

                            if (excelReader.GetString(3).Length > 2)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=validasiLengthFormat&name=TRANS_TYPE_CODE");
                                }
                                else
                                {
                                    return Redirect("/" + server + "/VAT_Out/?success=validasiLengthFormat&name=TRANS_TYPE_CODE");
                                }

                            }

                            obj.TRANS_TYPE_CODE = excelReader.GetString(3);
                            if (excelReader.GetString(4).Length > 2)
                            {
                                if (server == "FID")
                                {
                                    return Redirect("/VAT_Out/?success=validasiLengthFormat&name=INVOICE_MONTH");
                                }
                                else
                                {
                                    return Redirect("/" + server + "/VAT_Out/?success=validasiLengthFormat&name=INVOICE_MONTH");
                                }

                            }
                            obj.INVOICE_MONTH = excelReader.GetString(4);
                            obj.INVOICE_YEAR = excelReader.GetString(5);
                            
                            if (obj.TAX_INVOICE_NO.Trim() != "")
                            {
                                try
                                {
                                    obj.INVOICE_DATE = DateTime.ParseExact(excelReader.GetString(6), "dd/MM/yyyy", null);
                                }
                                catch (Exception)
                                {
                                    obj.INVOICE_DATE = excelReader.GetDateTime(6);
                                }

                                if (obj.INVOICE_DATE.Value.Year < 1753)
                                {
                                    obj.INVOICE_DATE = null;
                                }
                            }
                            else
                            {
                                obj.INVOICE_DATE = null;
                            }

                            if (obj.INVOICE_DATE != null)
                            {
                                if (excelReader.GetString(6).Contains(".") || excelReader.GetString(6).Contains("-"))
                                {
                                    if (server == "FID")
                                    {
                                        return Redirect("/VAT_Out/?success=validasiLengthFormat&name=INVOICE_DATE");
                                    }
                                    else
                                    {
                                        return Redirect("/" + server + "/VAT_Out/?success=validasiLengthFormat&name=INVOICE_DATE");
                                    }
                                }
                            }
                            //if (excelReader.GetString(6) == "INVOICE_DATE") obj.INVOICE_DATE = null; else obj.INVOICE_DATE = excelReader.GetDateTime(6);
                            if (excelReader.GetString(7) == "DPP_PRICE") obj.DPP_PRICE = 0; else obj.DPP_PRICE = excelReader.GetDecimal(7);
                            if (excelReader.GetString(8) == "VAT_PRICE") obj.VAT_PRICE = 0; else obj.VAT_PRICE = excelReader.GetDecimal(8);
                            if (excelReader.GetString(9) == "LUXURY_TAX_PRICE") obj.LUXURY_TAX_PRICE = 0; else obj.LUXURY_TAX_PRICE = excelReader.GetDecimal(9);


                            obj.REFERENCE = excelReader.GetString(10);


                            if (excelReader.GetString(11) == "ADD_REMARK_ID")
                            {
                                obj.ADD_REMARK_ID = 0;
                            }
                            else if (excelReader.GetString(11) == "null" || excelReader.GetString(11) == "null".ToUpper() || excelReader.GetString(11) == "" || obj.ADD_REMARK_ID == 0)
                            {
                                obj.ADD_REMARK_ID = 0;
                            }
                            else
                            {
                                obj.ADD_REMARK_ID = excelReader.GetDecimal(11);
                            }


                            obj.BUSINESS_UNIT = excelReader.GetString(12);
                            obj.SAP_DOC_NO = excelReader.GetString(13);
                            obj.PRODUCT_CODE = excelReader.GetString(14);
                            obj.PRODUCT_NM = excelReader.GetString(15);

                            if (excelReader.GetString(16) == "UNIT_PRICE") obj.UNIT_PRICE = 0; else obj.UNIT_PRICE = excelReader.GetDecimal(16);
                            if (excelReader.GetString(17) == "PRODUCT_QTY") obj.PRODUCT_QTY = 0; else obj.PRODUCT_QTY = excelReader.GetInt64(17);
                            if (excelReader.GetString(18) == "TOTAL_PRICE") obj.TOTAL_PRICE = 0; else obj.TOTAL_PRICE = excelReader.GetDecimal(18);
                            if (excelReader.GetString(19) == "DISC_PRICE") obj.DISC_PRICE = 0; else obj.DISC_PRICE = excelReader.GetDecimal(19);
                            if (excelReader.GetString(20) == "DPP_PRICE") obj.DPP_PRICE_D = 0; else obj.DPP_PRICE_D = excelReader.GetDecimal(20);
                            if (excelReader.GetString(21) == "VAT_PRICE") obj.VAT_PRICE_D = 0; else obj.VAT_PRICE_D = excelReader.GetDecimal(21);
                            if (excelReader.GetString(22) == "LUXURY_TAX_PERC") obj.LUXURY_TAX_PERC_D = 0; else obj.LUXURY_TAX_PERC_D = excelReader.GetDecimal(22);
                            if (excelReader.GetString(23) == "LUXURY_TAX_PRICE") obj.LUXURY_TAX_PRICE = 0; else obj.LUXURY_TAX_PRICE_D = excelReader.GetDecimal(23);

                            obj.NAME = excelReader.GetString(24);
                            obj.NPWP = excelReader.GetString(25);
                            obj.ADDRESS = excelReader.GetString(26);

                        }
                        VAT_OUT_SAPList.Add(obj);
                    end: ;
                        obj = null;
                        indexRow++;
                    }
                    catch (System.InvalidOperationException e)
                    {
                        Response.Write("<script>alert('" + e.Message + "');</script>");
                        if(server=="FID")
                        {
                            return Redirect("/VAT_Out/?success=invalidformat");
                        }
                        else
                        {
                            return Redirect("/"+server+"/VAT_Out/?success=invalidformat");
                        }
                        
                    }

                }
                excelReader.Close();
                LOG_HRepository.Instance.LogHeaderStart(p_processID, "TMMIN02020101", "TMMIN02", "SYSTEM", null, "MTAX00003INB", "Interface Tax Invoice Data from SAP Process");

                VAT_OUT_SAP_Repository.Instance.UploadToDatabase(VAT_OUT_SAPList, p_processID, uplFileDir, fileName, username);

                if (!System.IO.File.Exists(@pathandfilesuccess))
                {
                    System.IO.File.Move(@pathandfile, @pathandfilesuccess);
                }
                else
                {
                    System.IO.File.Delete(@pathandfile);
                }
            }
            if(server=="FID")
            {
                return Redirect("/VAT_Out/?success=true");
            }
            else
            {
                return Redirect("/"+server+"/VAT_Out/?success=true");
            }
            
        }


        public void DownloadTemplate()
        {
            string filename = "Excel_VAT_Out_Upload_Template.xls";
            string filepath = Server.MapPath(Url.Content("~/VAT_Out_ExcelTemplate/" + filename));
            if(System.IO.File.Exists(filepath))
            {
                byte[] filebyte = System.IO.File.ReadAllBytes(filepath);
                Response.Clear();
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.Expires = -1;
                Response.Buffer = true;
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Length", Convert.ToString(filebyte.Length));
                Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", filename));
                Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");
                Response.BinaryWrite(filebyte);
                Response.End();
            }
            else
            {
                Response.Write("<script>alert('File Not Found');</script>");
            }
        }


        //Added BY Andy
        public void DownloadPDF(string TaxInvoiceNo)
        {
            string filename = VAT_OUT_HRepository.Instance.GetFileName(TaxInvoiceNo);
            string filepath = Server.MapPath(Url.Content("~/UploadApproved/Extract/" + filename));
            if (System.IO.File.Exists(filepath) == true)
            {
                byte[] filebyte = System.IO.File.ReadAllBytes(filepath);
                Response.Clear();
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.Expires = -1;
                Response.Buffer = true;
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Length", Convert.ToString(filebyte.Length));
                Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", filename));
                Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");
                Response.BinaryWrite(filebyte);
                Response.End();
            }
            else
            {
                Response.Write("<script>alert('File Not Found');</script>");
            }
        }
    }

}