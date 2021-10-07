using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using consumable.Controllers;
using System.IO;
using consumable.Commons;

namespace consumable.Models.ProcurementTracking
{
    public class ProcurementTrackingRepository
    {
        private ProcurementTrackingRepository() { }

        #region Singleton
        private static ProcurementTrackingRepository instance = null;
        public static ProcurementTrackingRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ProcurementTrackingRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countProcurement(string receivedDate, string supplier, string sapDocNo, string planPaymentDate, 
            string verifiedStatus, string paymentStatus, string taxInvoiceNo, string invoiceNo, 
            string actualPaymentDate, string typeOfTransaction)
        {
            string receivedDateFrom = "";
            string receivedDateTo = "";
            string planPaymentDateFrom = "";
            string planPaymentDateTo = "";
            string actualPaymentDateFrom = "";
            string actualPaymentDateTo = "";
            
            if (receivedDate != null && !"".Equals(receivedDate))
            {
                string[] receivedDateArray = receivedDate.Split('-');
                receivedDateFrom = receivedDateArray[0].Trim();
                receivedDateTo = receivedDateArray[1].Trim();
            }

            if (planPaymentDate != null && !"".Equals(planPaymentDate))
            {
                string[] planPaymentDateArray = planPaymentDate.Split('-');
                planPaymentDateFrom = planPaymentDateArray[0].Trim();
                planPaymentDateTo = planPaymentDateArray[1].Trim();
            }

            if (actualPaymentDate != null && !"".Equals(actualPaymentDate))
            {
                string[] actualPaymentDateArray = actualPaymentDate.Split('-');
                actualPaymentDateFrom = actualPaymentDateArray[0].Trim();
                actualPaymentDateTo = actualPaymentDateArray[1].Trim();
            }

            if (supplier != null && !"".Equals(supplier))
            {
                string[] supplierArray = supplier.Split(';');

                for (int i = 0; i < supplierArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplier = "'" + supplierArray[i] + "'";
                    }
                    else
                    {
                        supplier = supplier + ",'" + supplierArray[i] + "'";
                    }
                }
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                RECEIVED_DATE_FROM = receivedDateFrom,
                RECEIVED_DATE_TO = receivedDateTo,
                SUPPLIER = supplier,
                AP_DOC_NO = sapDocNo,
                PLAN_PAYMENT_DATE_FROM = planPaymentDateFrom,
                PLAN_PAYMENT_DATE_TO = planPaymentDateTo,
                VERIFY_STATUS = verifiedStatus,
                PAYMENT_STATUS = paymentStatus,
                TAX_INVOICE_NO = taxInvoiceNo,
                INVOICE_NO = invoiceNo,
                ACTUAL_PAYMENT_DATE_FROM = actualPaymentDateFrom,
                ACTUAL_PAYMENT_DATE_TO = actualPaymentDateTo,
                TRANS_TYPE = typeOfTransaction
            };
            return db.SingleOrDefault<int>("CountProcurementTracking", args);
        }

        public List<ProcurementTracking> GetProcurementTracking(string receivedDate, string supplier, string sapDocNo, 
            string planPaymentDate, string verifiedStatus, string paymentStatus, string taxInvoiceNo, string invoiceNo, 
            string actualPaymentDate, string typeOfTransaction, int fromNumber, int toNumber)
        {    
            string receivedDateFrom = "";
            string receivedDateTo = "";
            string planPaymentDateFrom = "";
            string planPaymentDateTo = "";
            string actualPaymentDateFrom = "";
            string actualPaymentDateTo = "";

            if (receivedDate != null && !"".Equals(receivedDate))
            {
                string[] receivedDateArray = receivedDate.Split('-');
                receivedDateFrom = receivedDateArray[0].Trim();
                receivedDateTo = receivedDateArray[1].Trim();
            }

            if (planPaymentDate != null && !"".Equals(planPaymentDate))
            {
                string[] planPaymentDateArray = planPaymentDate.Split('-');
                planPaymentDateFrom = planPaymentDateArray[0].Trim();
                planPaymentDateTo = planPaymentDateArray[1].Trim();
            }

            if (actualPaymentDate != null && !"".Equals(actualPaymentDate))
            {
                string[] actualPaymentDateArray = actualPaymentDate.Split('-');
                actualPaymentDateFrom = actualPaymentDateArray[0].Trim();
                actualPaymentDateTo = actualPaymentDateArray[1].Trim();
            }

            if (supplier != null && !"".Equals(supplier))
            {
                string[] supplierArray = supplier.Split(';');

                for (int i = 0; i < supplierArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplier = "'" + supplierArray[i] + "'";
                    }
                    else
                    {
                        supplier = supplier + ",'" + supplierArray[i] + "'";
                    }
                }
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                RECEIVED_DATE_FROM = receivedDateFrom,
                RECEIVED_DATE_TO = receivedDateTo,
                SUPPLIER = supplier,
                AP_DOC_NO= sapDocNo,
                PLAN_PAYMENT_DATE_FROM = planPaymentDateFrom,
                PLAN_PAYMENT_DATE_TO = planPaymentDateTo,
                VERIFY_STATUS = verifiedStatus,
                PAYMENT_STATUS = paymentStatus,
                TAX_INVOICE_NO = taxInvoiceNo,
                INVOICE_NO = invoiceNo,
                ACTUAL_PAYMENT_DATE_FROM = actualPaymentDateFrom,
                ACTUAL_PAYMENT_DATE_TO = actualPaymentDateTo,
                TRANS_TYPE = typeOfTransaction,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<ProcurementTracking> result = db.Fetch<ProcurementTracking>("GetProcurementTracking", args);
            db.Close();

            return result;

        }

        public List<String> GetProcurementTrackingSort(string receivedDate, string supplier, string sapDocNo, 
            string planPaymentDate, string verifiedStatus, string paymentStatus, string taxInvoiceNo, string invoiceNo, 
            string actualPaymentDate, string typeOfTransaction, int fromNumber, int toNumber, string field, string sort)
        {
            List<String> result = new List<String>();
            List<ProcurementTracking> resultItem = new List<ProcurementTracking>();
            resultItem = GetProcurementTracking(receivedDate, supplier, sapDocNo, 
                planPaymentDate, verifiedStatus, paymentStatus, taxInvoiceNo, invoiceNo, 
                actualPaymentDate, typeOfTransaction, fromNumber, toNumber);

            List<ProcurementTracking> returnResult = new List<ProcurementTracking>();
            switch (field)
            {
                case "INVOICE_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INVOICE_NO).ToList() : resultItem.OrderByDescending(o => o.INVOICE_NO).ToList());
                    break;
                case "VENDOR_CODE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.VENDOR_CODE).ToList() : resultItem.OrderByDescending(o => o.VENDOR_CODE).ToList());
                    break;
                case "VENDOR_NAME":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.VENDOR_NAME).ToList() : resultItem.OrderByDescending(o => o.VENDOR_NAME).ToList());
                    break;
                case "INVOICE_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INVOICE_AMOUNT).ToList() : resultItem.OrderByDescending(o => o.INVOICE_AMOUNT).ToList());
                    break;
                case "TAX_INVOICE_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TAX_INVOICE_NO).ToList() : resultItem.OrderByDescending(o => o.TAX_INVOICE_NO).ToList());
                    break;
                case "INV_DOC_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INV_DOC_DATE).ToList() : resultItem.OrderByDescending(o => o.INV_DOC_DATE).ToList());
                    break;
                case "RECEIVED_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.RECEIVED_DATE).ToList() : resultItem.OrderByDescending(o => o.RECEIVED_DATE).ToList());
                    break;
                case "AP_DOC_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.AP_DOC_NO).ToList() : resultItem.OrderByDescending(o => o.AP_DOC_NO).ToList());
                    break;
                case "TRANS_TYPE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TRANS_TYPE).ToList() : resultItem.OrderByDescending(o => o.TRANS_TYPE).ToList());
                    break;
                case "INV_VERIFICATION_PLAN_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INV_VERIFICATION_PLAN_DATE).ToList() : resultItem.OrderByDescending(o => o.INV_VERIFICATION_PLAN_DATE).ToList());
                    break;
                case "INV_VERIFICATION_ACTUAL_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INV_VERIFICATION_ACTUAL_DATE).ToList() : resultItem.OrderByDescending(o => o.INV_VERIFICATION_ACTUAL_DATE).ToList());
                    break;
                case "INV_VERIFICATION_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INV_VERIFICATION_BY).ToList() : resultItem.OrderByDescending(o => o.INV_VERIFICATION_BY).ToList());
                    break;
                case "VERIFICATION_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.VERIFICATION_STATUS).ToList() : resultItem.OrderByDescending(o => o.VERIFICATION_STATUS).ToList());
                    break;
                case "INV_PAYMENT_PLAN_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INV_PAYMENT_PLAN_DATE).ToList() : resultItem.OrderByDescending(o => o.INV_PAYMENT_PLAN_DATE).ToList());
                    break;
                case "INV_PAYMENT_ACTUAL_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INV_PAYMENT_ACTUAL_DATE).ToList() : resultItem.OrderByDescending(o => o.INV_PAYMENT_ACTUAL_DATE).ToList());
                    break;
                case "CLEARING_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CLEARING_NO).ToList() : resultItem.OrderByDescending(o => o.CLEARING_NO).ToList());
                    break;
                case "PAYMENT_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PAYMENT_STATUS).ToList() : resultItem.OrderByDescending(o => o.PAYMENT_STATUS).ToList());
                    break;
                case "NOTICE_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.NOTICE_BY).ToList() : resultItem.OrderByDescending(o => o.NOTICE_BY).ToList());
                    break;
                case "REMARKS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.REMARKS).ToList() : resultItem.OrderByDescending(o => o.REMARKS).ToList());
                    break;
            }

            if (returnResult != null && returnResult.Count > 0)
            {
                int i = 0;
                foreach (ProcurementTracking pt in returnResult)
                {
                    result.Add(
                    "<tr>" +
                        "<td class=\"text-center\" width=\"30px\">" +
                            "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" id="+i+" name=\"selectedProcTrack\" value="+pt.SOURCE_DATA+" />" +
                        "</td>" +
                        "<td class=\"text-center\" width=\"150px\">" +
                            pt.INVOICE_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            pt.VENDOR_CODE +
                        "</td>" +
                        "<td class=\"text-center ellipsis\" width=\"180px\" style=\"max-width: 180px;\" title=\"pt.VENDOR_NAME\">" +
                            pt.VENDOR_NAME +
                        "</td>" +
                        "<td class=\"text-right\" width=\"100px\">" +
                            pt.INVOICE_AMOUNT.ToString("N0") +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.TAX_INVOICE_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            (pt.INV_DOC_DATE.HasValue ? pt.INV_DOC_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            (pt.RECEIVED_DATE.HasValue ? pt.RECEIVED_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            (pt.INV_VERIFICATION_PLAN_DATE.HasValue ? pt.INV_VERIFICATION_PLAN_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            (pt.INV_VERIFICATION_ACTUAL_DATE.HasValue ? pt.INV_VERIFICATION_ACTUAL_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.INV_VERIFICATION_BY +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.VERIFICATION_STATUS +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.AP_DOC_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            (pt.INV_PAYMENT_PLAN_DATE.HasValue ? pt.INV_PAYMENT_PLAN_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            (pt.INV_PAYMENT_ACTUAL_DATE.HasValue ? pt.INV_PAYMENT_ACTUAL_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.CLEARING_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.PAYMENT_STATUS +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.NOTICE_BY +
                        "</td>" +
                        "<td class=\"text-center\" width=\"100px\">" +
                            pt.REMARKS +
                        "</td>" +
                        "<td class=\"text-center\">" +
                            pt.TRANS_TYPE +
                        "</td>" +
                    "</tr>");

                    i++;
                }
            }
            else
            {
                result.Add(
                    "<tr>" +
                    "<td colspan=\"20\" class=\"text-center\">" +
                    "No data retrieved." +
                    "</td>" +
                    "</tr>");
            }

            return result;
        }

        public AjaxResult SaveDataInputOtherInvoice(IDBContext db, ProcurementTracking procTrack, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    RECEIVED_DATE = procTrack.RECEIVED_DATE,
                    VENDOR_CODE = procTrack.VENDOR_CODE,
                    INVOICE_NO = procTrack.INVOICE_NO,
                    INV_DOC_DATE = procTrack.INV_DOC_DATE,
                    INVOICE_AMOUNT = procTrack.INVOICE_AMOUNT,
                    TRANS_TYPE = procTrack.TRANS_TYPE,
                    SOURCE_DATA = "TBL",
                    CREATED_BY = "User01"
                };

                db.Execute("InputProcTrack", args);

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

        public AjaxResult DeleteData(IDBContext db, List<ProcurementTracking> procTrackList)
        {
            Console.Write("Delete Proc Track");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (ProcurementTracking item in procTrackList)
                {
                    dynamic args = new
                    {
                        TaxInvoiceNo = item.TAX_INVOICE_NO
                    };

                    result = db.Execute("DeleteProcTrack", args);

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

        public AjaxResult SubmitNotice(IDBContext db, List<ProcurementTracking> procTrackList)
        {
            Console.Write("Submit Notice Proc Track");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (ProcurementTracking item in procTrackList)
                {
                    dynamic args = new
                    {
                        TaxInvoiceNo = item.TAX_INVOICE_NO,
                        NoticeDate = item.NOTICE_DATE,
                        Remarks = item.REMARKS,
                        NoticeBy = "temp.mr1",
                        ChangeBy = "temp.mr1"
                    };

                    result = db.Execute("NoticeProcTrack", args);

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

        //Excel
        public List<ProcurementTracking> GetProcurementTrackingExcel(string receivedDate, 
           string supplier, string sapDocNo, string planPaymentDate, string verifiedStatus, 
            string paymentStatus, string taxInvoiceNo, string invoiceNo,
            string actualPaymentDate, string typeOfTransaction)
        {
            string receivedDateFrom = "";
            string receivedDateTo = "";
            string planPaymentDateFrom = "";
            string planPaymentDateTo = "";
            string actualPaymentDateFrom = "";
            string actualPaymentDateTo = "";

            if (receivedDate != null && !"".Equals(receivedDate))
            {
                string[] receivedDateArray = receivedDate.Split('-');
                receivedDateFrom = receivedDateArray[0].Trim();
                receivedDateTo = receivedDateArray[1].Trim();
            }

            if (planPaymentDate != null && !"".Equals(planPaymentDate))
            {
                string[] planPaymentDateArray = planPaymentDate.Split('-');
                planPaymentDateFrom = planPaymentDateArray[0].Trim();
                planPaymentDateTo = planPaymentDateArray[1].Trim();
            }

            if (actualPaymentDate != null && !"".Equals(actualPaymentDate))
            {
                string[] actualPaymentDateArray = actualPaymentDate.Split('-');
                actualPaymentDateFrom = actualPaymentDateArray[0].Trim();
                actualPaymentDateTo = actualPaymentDateArray[1].Trim();
            }

            if (supplier != null && !"".Equals(supplier))
            {
                string[] supplierArray = supplier.Split(';');

                for (int i = 0; i < supplierArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplier = "'" + supplierArray[i] + "'";
                    }
                    else
                    {
                        supplier = supplier + ",'" + supplierArray[i] + "'";
                    }
                }

            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                RECEIVED_DATE_FROM = receivedDateFrom,
                RECEIVED_DATE_TO = receivedDateTo,
                SUPPLIER = supplier,
                AP_DOC_NO = sapDocNo,
                PLAN_PAYMENT_DATE_FROM = planPaymentDateFrom,
                PLAN_PAYMENT_DATE_TO = planPaymentDateTo,
                VERIFY_STATUS = verifiedStatus,
                PAYMENT_STATUS = paymentStatus,
                TAX_INVOICE_NO = taxInvoiceNo,
                INVOICE_NO = invoiceNo,
                ACTUAL_PAYMENT_DATE_FROM = actualPaymentDateFrom,
                ACTUAL_PAYMENT_DATE_TO = actualPaymentDateTo,
                TRANS_TYPE = typeOfTransaction
            };

            List<ProcurementTracking> result = db.Fetch<ProcurementTracking>("GetProcurementTrackingExcel", args);
            db.Close();

            return result;

        }


        public byte[] GenerateDownloadFile(List<ProcurementTracking> procurementTrackingList)
        {
            byte[] result = null;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                result = CreateFile(procurementTrackingList);
            }
            finally
            {
                db.Close();

            }

            return result;
        }

        private byte[] CreateFile(List<ProcurementTracking> procurementTrackingList)
        {
            if (procurementTrackingList == null)
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
            sheet = workbook.CreateSheet(NPOIWriter.EscapeSheetName("Procurement Tracking"));
            sheet.FitToPage = false;

            // write header manually
            headers = new Dictionary<string, string>();
            //headers.Add("Process ID", processID);


            WriteDetail(workbook, sheet, startRow, cellStyleHeader, cellStyleData, procurementTrackingList);

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
        
        //public ICellStyle createHolidayCellStyleData(HSSFWorkbook wb)
        //{
        //    ICellStyle headerStyle = wb.CreateCellStyle();
        //    headerStyle.BorderBottom = BorderStyle.THIN;
        //    headerStyle.BorderLeft = BorderStyle.THIN;
        //    headerStyle.BorderRight = BorderStyle.THIN;
        //    headerStyle.BorderTop = BorderStyle.THIN;
        //    headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
        //    headerStyle.FillPattern = FillPatternType.THIN_HORZ_BANDS;
        //    headerStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
        //    return headerStyle;
        //}


        private void WriteDetail(
        HSSFWorkbook wb,
        ISheet sheet,
        int startRow,
        ICellStyle cellStyleHeader,
        ICellStyle cellStyleData,
        List<ProcurementTracking> procurementTrackingList)
        {
            int rowIdx = startRow;
            int itemCount = 0;
           
            int colHeader = 0;

            //fixed col
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Invoice No");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Supplier Code");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Supplier Name");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Invoice Amount");         

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Tax Invoice No");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Document Date");
            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Received Date");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 3, cellStyleHeader, "Invoice Verify");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Plan Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Actual Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Entry By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Status");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "AP Doc No");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 3, cellStyleHeader, "Invoice Payment");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Plan Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Actual Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Clearing");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Status");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 0, colHeader, colHeader + 1, cellStyleHeader, "Notice");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 1, colHeader++, cellStyleHeader, "Remarks");

            NPOIWriter.CreateMergedColHeader(wb, sheet, 0, 1, colHeader, colHeader++, cellStyleHeader, "Type Of Transaction");

            rowIdx = 2;
            foreach (ProcurementTracking procurementTracking in procurementTrackingList)
            {
                WriteDetailSingleData(wb, cellStyleData, procurementTracking, sheet, ++itemCount, rowIdx++);
            }
        }


        private void WriteDetailSingleData(
          HSSFWorkbook wb,
          ICellStyle cellStyle,
          ProcurementTracking item,
          ISheet sheet,
          int rowCount,
          int rowIndex)
        {
            IRow row = sheet.CreateRow(rowIndex);
            int startDtlColumnIdx = 11;
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, item.INVOICE_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.VENDOR_CODE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.VENDOR_NAME);
            NPOIWriter.createCellDouble(row, cellStyle, col++, item.INVOICE_AMOUNT);
            NPOIWriter.createCellText(row, cellStyle, col++, item.TAX_INVOICE_NO);
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.INV_DOC_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.INV_DOC_DATE.HasValue? item.INV_DOC_DATE.Value.ToString("dd.MM.yyyy"): "");
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.RECEIVED_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.RECEIVED_DATE.HasValue? item.RECEIVED_DATE.Value.ToString("dd.MM.yyyy"):"");
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.INV_VERIFICATION_PLAN_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.INV_VERIFICATION_PLAN_DATE.HasValue ? item.INV_VERIFICATION_PLAN_DATE.Value.ToString("dd.MM.yyyy") : "");
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.INV_VERIFICATION_ACTUAL_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.INV_VERIFICATION_ACTUAL_DATE.HasValue ? item.INV_VERIFICATION_ACTUAL_DATE.Value.ToString("dd.MM.yyyy"):"");
            NPOIWriter.createCellText(row, cellStyle, col++, item.INV_VERIFICATION_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.VERIFICATION_STATUS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.AP_DOC_NO);
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.INV_PAYMENT_PLAN_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.INV_PAYMENT_PLAN_DATE.HasValue ? item.INV_PAYMENT_PLAN_DATE.Value.ToString("dd.MM.yyyy"):"");
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.INV_PAYMENT_ACTUAL_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.INV_PAYMENT_ACTUAL_DATE.HasValue ? item.INV_PAYMENT_ACTUAL_DATE.Value.ToString("dd.MM.yyyy"):"");
            NPOIWriter.createCellText(row, cellStyle, col++, item.CLEARING_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PAYMENT_STATUS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.REMARKS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.TRANS_TYPE);
        }
    }
}