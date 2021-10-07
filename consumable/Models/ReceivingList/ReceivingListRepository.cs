using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using consumable.Common.Data;
using consumable.Controllers;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.IO;
using consumable.Commons;

namespace consumable.Models.ReceivingList
{
    public class ReceivingListRepository
    {
        private ReceivingListRepository() { }

        #region Singleton
        private static ReceivingListRepository instance = null;
        public static ReceivingListRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReceivingListRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countReceivingList(string poNoSearch, string supplierSearch, string poDateSearch, string statusSearch)
        {
            //string receivingDateSearchFrom = "";
            //string receivingDateSearchTo = "";
            //if (receivingDateSearch != null && !"".Equals(receivingDateSearch))
            //{
            //    string[] receivingDateSearchArray = receivingDateSearch.Split('-');
            //    receivingDateSearchFrom = receivingDateSearchArray[0].Trim();
            //    receivingDateSearchTo = receivingDateSearchArray[1].Trim();
            //}

            string poDateSearchFrom = "";
            string poDateSearchTo = "";
            if (poDateSearch != null && !"".Equals(poDateSearch))
            {
                string[] poDateSearchArray = poDateSearch.Split('-');
                poDateSearchFrom = poDateSearchArray[0].Trim();
                poDateSearchTo = poDateSearchArray[1].Trim();
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
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch
            };
            return db.SingleOrDefault<int>("CountReceivingList", args);
        }

        public List<ReceivingList> GetReceivingList(string poNoSearch, string supplierSearch, string poDateSearch, 
            string statusSearch, int fromNumber, int toNumber)
        {
            //string receivingDateSearchFrom = "";
            //string receivingDateSearchTo = "";
            //if (receivingDateSearch != null && !"".Equals(receivingDateSearch))
            //{
            //    string[] receivingDateSearchArray = receivingDateSearch.Split('-');
            //    receivingDateSearchFrom = receivingDateSearchArray[0].Trim();
            //    receivingDateSearchTo = receivingDateSearchArray[1].Trim();
            //}

            string poDateSearchFrom = "";
            string poDateSearchTo = "";
            if (poDateSearch != null && !"".Equals(poDateSearch))
            {
                string[] poDateSearchArray = poDateSearch.Split('-');
                poDateSearchFrom = poDateSearchArray[0].Trim();
                poDateSearchTo = poDateSearchArray[1].Trim();
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
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<ReceivingList> result = db.Fetch<ReceivingList>("GetReceivingList", args);
            db.Close();
            return result;
        }

        public List<String> GetReceivingListSort(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, int fromNumber, int toNumber, string field, string sort)
        {
            List<String> result = new List<String>();
            List<ReceivingList> resultItem = new List<ReceivingList>();
            resultItem = GetReceivingList(poNoSearch, supplierSearch, poDateSearch, statusSearch, 
                fromNumber, toNumber);

            List<ReceivingList> returnResult = new List<ReceivingList>();
            switch (field)
            {
                case "PO_NUMBER":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_NUMBER).ToList() : resultItem.OrderByDescending(o => o.PO_NUMBER).ToList());
                    break;
                case "PO_ITEM":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_ITEM).ToList() : resultItem.OrderByDescending(o => o.PO_ITEM).ToList());
                    break;
                case "PO_TEXT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_TEXT).ToList() : resultItem.OrderByDescending(o => o.PO_TEXT).ToList());
                    break;
                case "PO_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_DATE).ToList() : resultItem.OrderByDescending(o => o.PO_DATE).ToList());
                    break;
                case "VEND_CODE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.VEND_CODE).ToList() : resultItem.OrderByDescending(o => o.VEND_CODE).ToList());
                    break;
                case "SUPPLIER_NAME":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUPPLIER_NAME).ToList() : resultItem.OrderByDescending(o => o.SUPPLIER_NAME).ToList());
                    break;
                case "TOTAL_QTY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TOTAL_QTY).ToList() : resultItem.OrderByDescending(o => o.TOTAL_QTY).ToList());
                    break;
                case "MATDOC_UNIT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_UNIT).ToList() : resultItem.OrderByDescending(o => o.MATDOC_UNIT).ToList());
                    break;
                case "TOTAL_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.TOTAL_AMOUNT).ToList() : resultItem.OrderByDescending(o => o.TOTAL_AMOUNT).ToList());
                    break;
                case "MATDOC_CURRENCY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_CURRENCY).ToList() : resultItem.OrderByDescending(o => o.MATDOC_CURRENCY).ToList());
                    break;
                case "GR_STATUS":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.GR_STATUS).ToList() : resultItem.OrderByDescending(o => o.GR_STATUS).ToList());
                    break;
            }

            if (returnResult != null && returnResult.Count > 0)
            {
                foreach (ReceivingList rl in returnResult)
                {
                    string checkbox = null;
                    if (rl.GR_STATUS != "RECEIVED" || rl.TOTAL_QTY <= 0)
                    {
                        checkbox = "<input type=\"checkbox\" name=\"selectedGR\" class=\"check\" disabled />";
                    }
                    else
                    {
                        checkbox = "<input type=\"checkbox\" name=\"selectedGR\" class=\"check\" />";
                    }

                    string invoiceId = null;
                    if (rl.INVOICE_ID != null)
                        invoiceId = rl.INVOICE_ID;

                    string poDate = rl.PO_DATE.HasValue ? rl.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty;

                    string TOTAL_AMOUNT;
                    if (rl.MATDOC_CURRENCY.Equals("IDR"))
                    { TOTAL_AMOUNT = rl.TOTAL_AMOUNT.FormatCommaSeparator(); }
                    else
                    { TOTAL_AMOUNT = rl.TOTAL_AMOUNT.FormatCommaSeparatorWithDecimal(); }

                    result.Add(
                    "<tr>" +
                        "<td width=\"25px\" class=\"text-center grid-checkbox-col\">" +
                            checkbox +
                        "</td>" +
                        "<td width=\"90px\" class=\"text-center cursor-link\">" +
                            "<a onclick=\"openPopupMaterial('" + rl.PO_NUMBER + "', '" + rl.PO_ITEM + "' , '" + rl.PO_DATE.Value.ToString("dd.MM.yyyy") + "', " +
                                "'" + rl.VEND_CODE + "', '" + rl.GR_STATUS + "', '" + rl.PO_TEXT.Replace("\"", "") + "', '" + invoiceId + "')\">" + rl.PO_NUMBER + "</a>" +
                        "</td>" +
                        "<td width=\"80px\" class=\"text-center\">" +
                            rl.PO_ITEM +
                        "</td>" +
                        "<td width=\"220px\" style=\"max-width: 220px;\" class=\"text-left ellipsis\" title=" + rl.PO_TEXT.Replace("\"", "") + ">" +
                            rl.PO_TEXT +
                        "</td>" +
                        "<td width=\"75px\" class=\"text-center\">" +
                            poDate +
                        "</td>" +
                        "<td width=\"100px\" class=\"text-center\">" +
                            rl.VEND_CODE +
                        "</td>" +
                        "<td width=\"150px\" style=\"max-width: 150px;\" class=\"text-left ellipsis\" title=" + rl.SUPPLIER_NAME + ">" +
                            rl.SUPPLIER_NAME +
                        "</td>" +
                        "<td width=\"75px\" class=\"text-right\">" +
                            rl.TOTAL_QTY +
                        "</td>" +
                        "<td width=\"75px\" class=\"text-center\">" +
                            rl.MATDOC_UNIT +
                        "</td>" +
                        "<td width=\"150px\" class=\"text-right\">" +
                            TOTAL_AMOUNT +
                        "</td>" +
                        "<td width=\"75px\" class=\"text-center\">" +
                            rl.MATDOC_CURRENCY +
                        "</td>" +
                        "<td class=\"text-center\">" +
                            rl.GR_STATUS +
                        "</td>" +
                        "<td class=\"hidden\">" +
                            rl.INVOICE_ID +
                        "</td>" +
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

        public List<ReceivingListDetail> GetGR_IR_DATA_Detail(string matDocNoSearch)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                MATDOC_NO = matDocNoSearch
            };

            List<ReceivingListDetail> result = db.Fetch<ReceivingListDetail>("GetGR_IR_DATA_DETAIL", args);
            db.Close();
            return result;
        }


        public List<ReceivingListDetail> GetReceivingListDetail(string poNumber, string poItem, 
            string poDate, string vendCode, string grStatus, string invoiceId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                PO_NUMBER = poNumber,
                PO_ITEM = poItem,
                PO_DATE = poDate,
                VEND_CODE = vendCode,
                GR_STATUS = grStatus,
                INVOICE_ID = invoiceId
            };

            List<ReceivingListDetail> result = db.Fetch<ReceivingListDetail>("GetReceivingListDetail", args);
            db.Close();
            return result;
        }

        public AjaxResult ApproveGR(IDBContext db, string poNumber, string poItem,
            string poDate, string vendCode, string grStatus, string invoiceId, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            if (invoiceId == null || "".Equals(invoiceId.Trim()) )
            {
                invoiceId = "";
            }

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = poNumber,
                    PO_ITEM = poItem,
                    PO_DATE = poDate,
                    VEND_CODE = vendCode,
                    GR_STATUS = grStatus,
                    INVOICE_ID = invoiceId,
                    UPDATED_BY = NoReg,
                    UPDATED_DT = DateTime.Now
                };

                db.Execute("ApproveGR", args);

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


        //public List<string> GetInvoiceReceivingSort(string field, string sort)
        //{
        //    List<String> result = new List<String>();

        //    List<InvoiceReceiving> returnResult = new List<InvoiceReceiving>();
        //    switch (field)
        //    {
        //        case "CERTIFICATE_ID":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.CERTIFICAT_ID).ToList() : tempIR.OrderByDescending(o => o.CERTIFICAT_ID).ToList());
        //            break;
        //        case "SUPPLIER_CODE":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUPPLIER_CODE).ToList() : tempIR.OrderByDescending(o => o.SUPPLIER_CODE).ToList());
        //            break;
        //        case "SUPPLIER_NAME":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUPPLIER_NAME).ToList() : tempIR.OrderByDescending(o => o.SUPPLIER_NAME).ToList());
        //            break;
        //        case "INVOICE_NO":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_NO).ToList() : tempIR.OrderByDescending(o => o.INVOICE_NO).ToList());
        //            break;
        //        case "CURRENCY":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.CURRENCY).ToList() : tempIR.OrderByDescending(o => o.CURRENCY).ToList());
        //            break;
        //        case "INVOICE_AMOUNT":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_AMOUNT).ToList() : tempIR.OrderByDescending(o => o.INVOICE_AMOUNT).ToList());
        //            break;
        //        case "INVOICE_TAX_NO":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_TAX_NO).ToList() : tempIR.OrderByDescending(o => o.INVOICE_TAX_NO).ToList());
        //            break;
        //        case "INVOICE_TAX_AMOUNT":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.INVOICE_TAX_AMOUNT).ToList() : tempIR.OrderByDescending(o => o.INVOICE_TAX_AMOUNT).ToList());
        //            break;
        //        case "SUBMIT_DATE":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUBMIT_DATE).ToList() : tempIR.OrderByDescending(o => o.SUBMIT_DATE).ToList());
        //            break;
        //        case "SUBMIT_BY":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.SUBMIT_BY).ToList() : tempIR.OrderByDescending(o => o.SUBMIT_BY).ToList());
        //            break;
        //        case "RECEIVE_STATUS":
        //            returnResult = ((sort == "asc" || sort == "none") ? tempIR.OrderBy(o => o.RECEIVE_STATUS).ToList() : tempIR.OrderByDescending(o => o.RECEIVE_STATUS).ToList());
        //            break;
        //    }

        //    foreach (InvoiceReceiving item in returnResult)
        //    {
        //        result.Add("<tr>" +
        //                    "<td width=\"160px\" class=\"text-left\">" + item.CERTIFICAT_ID + "" +
        //                    "</td>" +
        //                    "<td width=\"80px\" class=\"text-center\">" + item.SUPPLIER_CODE + "" +
        //                    "</td>" +
        //                    "<td width=\"200px\" class=\"text-left ellipsis\" style=\"max-width: 201px;\">" + item.SUPPLIER_NAME + "" +
        //                    "</td>" +
        //                    "<td width=\"100px\" class=\"text-center\">" + item.INVOICE_NO + "" +
        //                    "</td>" +
        //                    "<td width=\"75px\" class=\"text-center\">" + item.CURRENCY + "" +
        //                    "</td>" +
        //                    "<td width=\"110px\" class=\"text-right\">" + item.INVOICE_AMOUNT.ToString("N0") + "" +
        //                    "</td>" +
        //                    "<td width=\"100px\" class=\"text-center\">" + item.INVOICE_TAX_NO + "" +
        //                    "</td>" +
        //                    "<td width=\"100px\" class=\"text-right\">" + item.INVOICE_TAX_AMOUNT.ToString("N0") + "" +
        //                    "</td>" +
        //                    "<td width=\"100px\" class=\"text-center\">" + (item.SUBMIT_DATE != DateTime.MinValue ? item.SUBMIT_DATE.ToString("dd.MM.yyyy") : "") + "" +
        //                    "</td>" +
        //                    "<td width=\"100px\" class=\"text-center\">" + item.SUBMIT_BY + "" +
        //                    "</td>" +
        //                    "<td width=\"100px\" class=\"text-left\">" + item.RECEIVE_STATUS + "" +
        //                    "</td>" +
        //                    "<td class=\"text-center\">" + item.NOTICE + "" +
        //                    "</td>" +
        //                "</tr>");
        //    }

        //    return result;
        //}


        public List<GR_IR_DATA> GetReceivingListExcel(string poNoSearch, string receivingDateSearch,
         string supplierSearch, string poDateSearch, string matDocNoSearch, string statusSearch)
        {
            string receivingDateSearchFrom = "";
            string receivingDateSearchTo = "";
            if (receivingDateSearch != null && !"".Equals(receivingDateSearch))
            {
                string[] receivingDateSearchArray = receivingDateSearch.Split('-');
                receivingDateSearchFrom = receivingDateSearchArray[0].Trim();
                receivingDateSearchTo = receivingDateSearchArray[1].Trim();
            }

            string poDateSearchFrom = "";
            string poDateSearchTo = "";
            if (poDateSearch != null && !"".Equals(poDateSearch))
            {
                string[] poDateSearchArray = poDateSearch.Split('-');
                poDateSearchFrom = poDateSearchArray[0].Trim();
                poDateSearchTo = poDateSearchArray[1].Trim();
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
                RECEIVING_DATE_FROM = receivingDateSearchFrom,
                RECEIVING_DATE_TO = receivingDateSearchTo,
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER = supplierSearch,
                MATDOC_NO = matDocNoSearch,
                STATUS = statusSearch
            };

            List<GR_IR_DATA> result = db.Fetch<GR_IR_DATA>("GetGR_IR_DATAExcel", args);
            db.Close();
            return result;
        }


        public byte[] GenerateDownloadFile(List<GR_IR_DATA> receivingList)
        {
            byte[] result = null;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                result = CreateFile(receivingList);
            }
            finally
            {
                db.Close();

            }

            return result;
        }


        private byte[] CreateFile(List<GR_IR_DATA> receivingList)
        {
            if (receivingList == null)
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
            sheet = workbook.CreateSheet(NPOIWriter.EscapeSheetName("Receiving List"));
            sheet.FitToPage = false;

            // write header manually
            headers = new Dictionary<string, string>();
            //headers.Add("Process ID", processID);


            WriteDetail(workbook, sheet, startRow, cellStyleHeader, cellStyleData, receivingList);

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
        List<GR_IR_DATA> receivingList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            int colHeader = 0;

            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Item");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Text");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material Doc No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material Item");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Receiving Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Code");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Name");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Qty");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Unit");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Amount");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Currency");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Status");
           
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Cancel Doc No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "User ID ");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice ID");

            rowIdx = 1;
            foreach (GR_IR_DATA item in receivingList)
            {
                WriteDetailSingleData(wb, cellStyleData, item, sheet, ++itemCount, rowIdx++);
            }
        }


        private void WriteDetailSingleData(
          HSSFWorkbook wb,
          ICellStyle cellStyle,
          GR_IR_DATA item,
          ISheet sheet,
          int rowCount,
          int rowIndex)
        {
            IRow row = sheet.CreateRow(rowIndex);
            int startDtlColumnIdx = 11;
            int col = 0;

            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_NUMBER);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_ITEM);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MAT_DESCR);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_DATE.HasValue ? item.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_NUMBER);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_ITEM);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_DATE.HasValue ? item.MATDOC_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);

            NPOIWriter.createCellText(row, cellStyle, col++, item.VEND_CODE);          
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUPPLIER_NAME);         

            NPOIWriter.createCellDouble(row, cellStyle, col++, item.MATDOC_QTY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_UNIT);
            NPOIWriter.createCellDouble(row, cellStyle, col++, item.MATDOC_AMOUNT);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_CURRENCY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.GR_STATUS);
           
            NPOIWriter.createCellText(row, cellStyle, col++, item.CANCEL_DOC_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.USRID);
            NPOIWriter.createCellText(row, cellStyle, col++, item.INVOICE_ID);

        }

    }
}