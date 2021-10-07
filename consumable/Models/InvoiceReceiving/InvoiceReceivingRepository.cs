using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using consumable.Controllers;
using System.IO;
using consumable.Commons;

namespace consumable.Models.InvoiceReceiving
{
    public class InvoiceReceivingRepository
    {
        private InvoiceReceivingRepository() { }

        #region Singleton
        private static InvoiceReceivingRepository instance = null;
        public static InvoiceReceivingRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InvoiceReceivingRepository();
                }
                return instance;
            }
        }
        #endregion


        public int countInvoiceReceiving()
        {
           
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                STATUS = ""
            };
            return db.SingleOrDefault<int>("CountInvoiceReceiving", args);
        }

        public List<InvoiceReceiving> GetInvoiceReceiving(int fromNumber, int toNumber)
        {           
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<InvoiceReceiving> result = db.Fetch<InvoiceReceiving>("GetInvoiceReceiving", args);
            db.Close();
            return result;
        }

        public InvoiceReceiving findInvoiceReceiving(string certificateId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CERTIFICATE_ID = certificateId
            };

            InvoiceReceiving result = db.SingleOrDefault<InvoiceReceiving>("FindInvoiceReceiving", args);
            db.Close();
            return result;
        }

        public List<InvoiceReceiving> GetInvoiceReceivingListExcel()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
               
            };

            List<InvoiceReceiving> result = db.Fetch<InvoiceReceiving>("GetInvoiceReceivingExcel", args);
            db.Close();
            return result;
        }

        public AjaxResult process(IDBContext db, string certificateId, string processType, string noticeDate, 
            string noticeRemark, string noticeBy)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            DateTime start = DateTime.Today.AddDays(7);
            DayOfWeek day = DayOfWeek.Wednesday;

            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            DateTime nextWednesday = start.AddDays(daysToAdd);
            DateTime paymentPlanDate = nextWednesday;
            
            try
            {
                //int suppAutoPostFlag = db.SingleOrDefault<int>("GetSuppAutoPostFlag", new { CERTIFICATE_ID = certificateId });

                dynamic args = new
                {
                    CERTIFICATE_ID = certificateId,
                    RECEIVED_STATUS =  processType,
                    NOTICE_DATE = noticeDate,
                    PAYMENT_PLAN_DATE = paymentPlanDate.ToString("yyyy-MM-dd"),
                    NOTICE_REMARK = noticeRemark,
                    NOTICE_BY = noticeBy,
                    UPDATED_BY = noticeBy
                };

                db.Execute("ProcessInvoiceReceiving", args);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                
                //20200818 start
                int suppAutoPostFlag = db.SingleOrDefault<int>("GetSuppAutoPostFlag", new { CERTIFICATE_ID = certificateId });
                //20200818 end

                if (suppAutoPostFlag == 1)
                {
                    //ajaxResult.SuccMesgs = new string[] { "Invoice Auto Posting" };
                } else {
                    ajaxResult.SuccMesgs = new string[] { "Invoice Manual Posting" };
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


        public byte[] GenerateDownloadFile(List<InvoiceReceiving> invoiceReceivingList)
        {
            byte[] result = null;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                result = CreateFile(invoiceReceivingList);
            }
            finally
            {
                db.Close();

            }

            return result;
        }


        private byte[] CreateFile(List<InvoiceReceiving> invoiceReceivingList)
        {
            if (invoiceReceivingList == null)
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
            sheet = workbook.CreateSheet(NPOIWriter.EscapeSheetName("Invoice Receiving List"));
            sheet.FitToPage = false;

            // write header manually
            headers = new Dictionary<string, string>();
            //headers.Add("Process ID", processID);


            WriteDetail(workbook, sheet, startRow, cellStyleHeader, cellStyleData, invoiceReceivingList);

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
        List<InvoiceReceiving> invoiceReceivingList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            int colHeader = 0;

            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Certificate ID");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Code");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Name");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Currency");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice Amount");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice Tax No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice Tax Amount");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Submit Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Submit By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Received Status");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Notice By");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Notice Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Notice Remarks");
            
            rowIdx = 1;
            foreach (InvoiceReceiving item in invoiceReceivingList)
            {
                WriteDetailSingleData(wb, cellStyleData, item, sheet, ++itemCount, rowIdx++);
            }
        }


        private void WriteDetailSingleData(
          HSSFWorkbook wb,
          ICellStyle cellStyle,
          InvoiceReceiving item,
          ISheet sheet,
          int rowCount,
          int rowIndex)
        {
            IRow row = sheet.CreateRow(rowIndex);
            int startDtlColumnIdx = 11;
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, item.CERTIFICATE_ID);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUPPLIER_CD);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUPPLIER_NAME);
           
            NPOIWriter.createCellText(row, cellStyle, col++, item.INVOICE_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.CURRENCY);
            NPOIWriter.createCellDouble(row, cellStyle, col++, item.TOTAL_AMOUNT);

            NPOIWriter.createCellText(row, cellStyle, col++, item.TAX_INVOICE_NO);
            NPOIWriter.createCellDouble(row, cellStyle, col++, item.TAX_INVOICE_AMOUNT);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUBMIT_DT.HasValue ? item.SUBMIT_DT.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUBMIT_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.RECEIVED_STATUS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_BY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_DATE.HasValue ? item.NOTICE_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.NOTICE_REMARK);
           

        }

        public List<string> GetInvoiceReceivingSort(int fromNumber, int toNumber, string field, string sort)
        {
            List<String> result = new List<String>();
            List<InvoiceReceiving> resultItem = new List<InvoiceReceiving>();
            resultItem = GetInvoiceReceiving(fromNumber, toNumber);

            List<InvoiceReceiving> returnResult = new List<InvoiceReceiving>();
            switch (field)
            {
                case "CERTIFICATE_ID":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CERTIFICATE_ID).ToList() : resultItem.OrderByDescending(o => o.CERTIFICATE_ID).ToList());
                    break;
                case "SUPPLIER_CD":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUPPLIER_CD).ToList() : resultItem.OrderByDescending(o => o.SUPPLIER_CD).ToList());
                    break;
                case "SUPPLIER_NAME":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUPPLIER_NAME).ToList() : resultItem.OrderByDescending(o => o.SUPPLIER_NAME).ToList());
                    break;
                case "INVOICE_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.INVOICE_NO).ToList() : resultItem.OrderByDescending(o => o.INVOICE_NO).ToList());
                    break;
                case "CURRENCY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CURRENCY).ToList() : resultItem.OrderByDescending(o => o.CURRENCY).ToList());
                    break;
                case "TOTAL_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TOTAL_AMOUNT).ToList() : resultItem.OrderByDescending(o => o.TOTAL_AMOUNT).ToList());
                    break;
                case "TAX_INVOICE_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TAX_INVOICE_NO).ToList() : resultItem.OrderByDescending(o => o.TAX_INVOICE_NO).ToList());
                    break;
                case "TAX_INVOICE_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TAX_INVOICE_AMOUNT).ToList() : resultItem.OrderByDescending(o => o.TAX_INVOICE_AMOUNT).ToList());
                    break;
                case "SUBMIT_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUBMIT_DT).ToList() : resultItem.OrderByDescending(o => o.SUBMIT_DT).ToList());
                    break;
                case "SUBMIT_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUBMIT_BY).ToList() : resultItem.OrderByDescending(o => o.SUBMIT_BY).ToList());
                    break;
                case "RECEIVED_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.RECEIVED_STATUS).ToList() : resultItem.OrderByDescending(o => o.RECEIVED_STATUS).ToList());
                    break;
            }

            if (returnResult != null && returnResult.Count > 0)
            {
                foreach (InvoiceReceiving ir in returnResult)
                {
                    string TOTAL_AMOUNT;
                    if (ir.CURRENCY.Equals("IDR"))
                    { TOTAL_AMOUNT = ir.TOTAL_AMOUNT.FormatCommaSeparator(); }
                    else
                    { TOTAL_AMOUNT = ir.TOTAL_AMOUNT.FormatCommaSeparatorWithDecimal(); }

                    string submitDate = ir.SUBMIT_DT.HasValue ? ir.SUBMIT_DT.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string tdReceivedStatus = null;
                    if ("ACCEPT".Equals(ir.RECEIVED_STATUS))
                    {
                        tdReceivedStatus = "<td width=\"80px\" class=\"text-center\" style=\"background-color: #66FF66\">" + ir.RECEIVED_STATUS + "</td>";
                    }
                    else
                    {
                        tdReceivedStatus = "<td width=\"80px\" class=\"text-center\" style=\"background-color: #FF4D4D\">" + ir.RECEIVED_STATUS + "</td>";

                    }

                    string tdNotice = null;
                    if (!ir.NOTICE_DATE.HasValue)
                    {
                        tdNotice = "<td></td>";
                    }
                    else
                    {
                        tdNotice = "<td class=\"text-center cursor-link\"><i class=\"fa fa-envelope\" onclick=\"openPopupNotice('" + ir.NOTICE_BY + "', '" + ir.NOTICE_DATE.Value.ToString("dd.MM.yyyy") + "' , '" + ir.NOTICE_REMARK + "')\"></i></td>";
                    }

                    result.Add("<tr>" +
                                "<td width=\"210px\" class=\"text-left\">" + ir.CERTIFICATE_ID + "" +
                                "</td>" +
                                "<td width=\"110px\" class=\"text-center\">" + ir.SUPPLIER_CD + "" +
                                "</td>" +
                                "<td width=\"200px\" class=\"text-left ellipsis\" style=\"max-width: 200px;\">" + ir.SUPPLIER_NAME + "" +
                                "</td>" +
                                "<td width=\"120px\" class=\"text-center\">" + ir.INVOICE_NO + "" +
                                "</td>" +
                                "<td width=\"75px\" class=\"text-center\">" + ir.CURRENCY + "" +
                                "</td>" +
                                "<td width=\"110px\" class=\"text-right\">" + TOTAL_AMOUNT + "" +
                                "</td>" +
                                "<td width=\"120px\" class=\"text-center\">" + ir.TAX_INVOICE_NO + "" +
                                "</td>" +
                                "<td width=\"110px\" class=\"text-right\">" + ir.TAX_INVOICE_AMOUNT.ToString("N0") + "" +
                                "</td>" +
                                "<td width=\"75px\" class=\"text-center\">" + submitDate + "" +
                                "</td>" +
                                "<td width=\"120px\" class=\"text-left\" class=\"text-left ellipsis\" style=\"max-width: 120px;\">" + ir.SUBMIT_BY + "" +
                                "</td>" +
                                tdReceivedStatus +
                                tdNotice +
                            "</tr>");
                }
            }
            else
            {
                result.Add(
                    "<tr>" +
                    "<td colspan=\"15\" class=\"text-center\">" +
                    "No data retrieved." +
                    "</td>" +
                    "</tr>");
            }

            return result;
        }
    }
}