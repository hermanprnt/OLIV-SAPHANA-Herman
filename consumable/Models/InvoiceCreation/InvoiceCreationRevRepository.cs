using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using consumable.Common.Data;
using consumable.Controllers;
using System.IO;
using Toyota.Common.Web.Platform;
using Toyota.Common.Database;
using consumable.Models.InvoiceCreation;
using consumable.Commons;
using consumable.Models.SUPPLIER;
using consumable.Models;
using consumable.Commons.Constants;
using consumable.Models.SystemProperty;

namespace consumable.Models.InvoiceCreation
{
    public class InvoiceCreationRevRepository
    {
        private InvoiceCreationRevRepository() { }

        #region Singleton
        private static InvoiceCreationRevRepository instance = null;
        public static InvoiceCreationRevRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InvoiceCreationRevRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countInvoiceCreationList(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, string poTextSearch, string receivingDateSearch, string matDocNoSearch, string dnNoSearch)
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
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                MATDOC_NO = matDocNoSearch,
                RECEIVING_DATE_FROM = receivingDateSearchFrom,
                RECEIVING_DATE_TO = receivingDateSearchTo,
                PO_TEXT = poTextSearch,
                DN_NO = dnNoSearch
            };
            return db.SingleOrDefault<int>("CountInvoiceCreationRev", args);
        }

        public List<GR_IR_DATA> GetInvoiceCreation(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, string poTextSearch, string receivingDateSearch, string matDocNoSearch, string dnNoSearch, int fromNumber, int toNumber)
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
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                MATDOC_NO = matDocNoSearch,
                RECEIVING_DATE_FROM = receivingDateSearchFrom,
                RECEIVING_DATE_TO = receivingDateSearchTo,
                PO_TEXT = poTextSearch,
                DN_NO = dnNoSearch,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<GR_IR_DATA> result = db.Fetch<GR_IR_DATA>("GetInvoiceCreationRev", args);
            db.Close();
            return result;
        }

        public List<String> GetInvoiceCreationSort(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, string poTextSearch, string receivingDateSearch, string matDocNoSearch,
            string dnNoSearch, int fromNumber, int toNumber, string field, string sort)
        {
            List<String> result = new List<String>();
            List<GR_IR_DATA> resultItem = new List<GR_IR_DATA>();
            resultItem = GetInvoiceCreation(poNoSearch, supplierSearch,
                poDateSearch, statusSearch, poTextSearch, receivingDateSearch, matDocNoSearch, dnNoSearch, fromNumber, toNumber);

            List<GR_IR_DATA> returnResult = new List<GR_IR_DATA>();
            switch (field)
            {
                case "PO_NUMBER":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_NUMBER).ToList() : resultItem.OrderByDescending(o => o.PO_NUMBER).ToList());
                    break;
                case "PO_ITEM":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_ITEM).ToList() : resultItem.OrderByDescending(o => o.PO_ITEM).ToList());
                    break;
                case "PO_TEXT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MAT_DESCR).ToList() : resultItem.OrderByDescending(o => o.MAT_DESCR).ToList());
                    break;
                case "PO_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_DATE).ToList() : resultItem.OrderByDescending(o => o.PO_DATE).ToList());
                    break;
                case "MATDOC_DATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_DATE).ToList() : resultItem.OrderByDescending(o => o.MATDOC_DATE).ToList());
                    break;
                case "VEND_CODE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.VEND_CODE).ToList() : resultItem.OrderByDescending(o => o.VEND_CODE).ToList());
                    break;
                case "SUPPLIER_NAME":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.SUPPLIER_NAME).ToList() : resultItem.OrderByDescending(o => o.SUPPLIER_NAME).ToList());
                    break;
                case "MATDOC_NUMBER":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_NUMBER).ToList() : resultItem.OrderByDescending(o => o.MATDOC_NUMBER).ToList());
                    break;
                case "MATDOC_QTY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_QTY).ToList() : resultItem.OrderByDescending(o => o.MATDOC_QTY).ToList());
                    break;
                case "MATDOC_AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_AMOUNT).ToList() : resultItem.OrderByDescending(o => o.MATDOC_AMOUNT).ToList());
                    break;
                case "HEADER_TEXT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.HEADER_TEXT).ToList() : resultItem.OrderByDescending(o => o.HEADER_TEXT).ToList());
                    break;
                case "MATDOC_CURRENCY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.MATDOC_CURRENCY).ToList() : resultItem.OrderByDescending(o => o.MATDOC_CURRENCY).ToList());
                    break;
            }

            foreach (GR_IR_DATA item in returnResult)
            {
                string checkbox = null;

                checkbox = "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" name=\"selectedManifest\" onclick=\"javascript:updateManifest()\" />";


                string MATDOC_AMOUNT;
                if (item.MATDOC_CURRENCY.Equals("IDR"))
                { MATDOC_AMOUNT = item.MATDOC_AMOUNT.FormatCommaSeparator(); }
                else
                { MATDOC_AMOUNT = item.MATDOC_AMOUNT.FormatCommaSeparatorWithDecimal(); }

                result.Add(
                "<tr>" +
                    "<td width=\"20px\" class=\"text-center grid-checkbox-col\">" +
                        checkbox +
                    "</td>" +
                    "<td width=\"80px\" class=\"text-center cursor-link\">" +
                        item.PO_NUMBER +
                    "</td>" +
                    "<td width=\"65px\" class=\"text-center\">" +
                        item.PO_ITEM +
                    "</td>" +
                    "<td width=\"200px\" class=\"text-left ellipsis\" style=\"max-width: 220px;\" title=" + item.MAT_DESCR.Replace("\"", "") + ">" +
                        item.MAT_DESCR +
                    "</td>" +
                    "<td width=\"75px\" class=\"text-center\">" +
                        (item.PO_DATE.HasValue ? item.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                    "</td>" +
                    "<td width=\"105px\" class=\"text-center\">" +
                         (item.MATDOC_DATE.HasValue ? item.MATDOC_DATE.Value.ToString("dd.MM.yyyy") : string.Empty) +
                    "</td>" +
                    "<td width=\"100px\" class=\"text-center\">" +
                        item.VEND_CODE +
                    "</td>" +
                    "<td width=\"150px\" class=\"text-left ellipsis\" title=" + item.SUPPLIER_NAME + " style=\"max-width: 150px;\">" +
                        item.SUPPLIER_NAME +
                    "</td>" +
                    "<td width=\"75px\" class=\"text-right\">" +
                        item.MATDOC_NUMBER +
                    "</td>" +
                    "<td width=\"75px\" class=\"text-right\">" +
                        item.MATDOC_QTY +
                    "</td>" +
                    "<td width=\"75px\" class=\"text-center\">" +
                        MATDOC_AMOUNT +
                    "</td>" +
                    "<td width=\"130px\" class=\"text-right\">" +
                        item.HEADER_TEXT +
                    "</td>" +
                    "<td class=\"text-center\">" +
                        item.MATDOC_CURRENCY +
                    "</td>" +
                "</tr>");

            }

            return result;
        }

        public List<GR_IR_DATA> GetDownloadGR(string dnNoSearch, string poNoSearch, string receivingDateSearch,
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
                //PO_DATE_FROM = poDateSearchFrom,
                //PO_DATE_TO = poDateSearchTo,
                //PO_NO = poNoSearch,
                //SUPPLIER = supplierSearch,
                //STATUS = statusSearch,
                //PO_TEXT = poTextSearch,
                DN_NO = dnNoSearch,
                RECEIVING_DATE_FROM = receivingDateSearchFrom,
                RECEIVING_DATE_TO = receivingDateSearchTo,
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER = supplierSearch,
                MATDOC_NO = matDocNoSearch,
                STATUS = statusSearch
            };

            List<GR_IR_DATA> result = db.Fetch<GR_IR_DATA>("GetGR_IR_DATAExcel1", args);
            db.Close();
            return result;
        }

        public byte[] GenerateDownloadFile(List<GR_IR_DATA> invoiceCreationList)
        {
            byte[] result = null;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                result = CreateFile(invoiceCreationList);
            }
            finally
            {
                db.Close();

            }

            return result;
        }


        private byte[] CreateFile(List<GR_IR_DATA> invoiceCreationList)
        {
            if (invoiceCreationList == null)
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
            sheet = workbook.CreateSheet(NPOIWriter.EscapeSheetName("InvoiceCreation"));
            sheet.FitToPage = false;

            // write header manually
            headers = new Dictionary<string, string>();
            //headers.Add("Process ID", processID);


            WriteDetail(workbook, sheet, startRow, cellStyleHeader, cellStyleData, invoiceCreationList);

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
        List<GR_IR_DATA> invoiceCreationList)
        {
            int rowIdx = startRow;
            int itemCount = 0;

            int colHeader = 0;

            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO No");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Date");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Header Text");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Receiving Date");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Code");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material Doc No");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "IR Doc No");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Status");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Plant");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Movement Type");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Cancel Doc No");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "User ID");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice No");

            //20200812 start
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Item");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "DN No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "DN Item");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Text");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Receiving Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Code");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Name");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material Doc No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material Item");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Qty");
            //NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Unit");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Amount");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Header Text");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Currency");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "System");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material(Goods/Service)");
            //20200812 end

            rowIdx = 1;
            foreach (GR_IR_DATA item in invoiceCreationList)
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

            //NPOIWriter.createCellText(row, cellStyle, col++, item.PO_NUMBER);
            ////NPOIWriter.createCellDate(row, cellStyle, col++, item.PO_DATE);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.PO_DATE.HasValue ? item.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.HEADER_TEXT);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_DATE.HasValue ? item.MATDOC_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);

            //NPOIWriter.createCellText(row, cellStyle, col++, item.VEND_CODE);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_NUMBER);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.IR_NO);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.GR_STATUS);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.PLANT_CODE);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.MOV_TYPE);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.CANCEL_DOC_NO);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.USRID);

            //20200812 start
            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_NUMBER);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_ITEM);
            NPOIWriter.createCellText(row, cellStyle, col++, item.DN_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.DN_NO_ITEM);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MAT_DESCR);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_DATE.HasValue ? item.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);

            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_DATE.HasValue ? item.MATDOC_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);

            NPOIWriter.createCellText(row, cellStyle, col++, item.VEND_CODE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SUPPLIER_NAME);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_NUMBER);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_ITEM);

            NPOIWriter.createCellDouble(row, cellStyle, col++, item.MATDOC_QTY);
            //NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_UNIT);
            NPOIWriter.createCellDouble(row, cellStyle, col++, item.MATDOC_AMOUNT);
            NPOIWriter.createCellText(row, cellStyle, col++, item.HEADER_TEXT);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_CURRENCY);
            NPOIWriter.createCellText(row, cellStyle, col++, item.SYSTEM);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATERIAL);
            //20200812 end

        }

        public void updateUsedFlagEfaktur(IDBContext db, Invoice invoice, string createdBy, string invoiceNo)
        {
            SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
            SystemProperty.SystemProperty system = systemPropertyRepo.GetSysPropByCodeAndType("IP_ADDRESS", "INVOICE_INQUIRY");

            dynamic args2 = new
            {
                TAX_INVOICE_NO = invoice.TAX_INVOICE_NO,
                IS_USED = "1",
                CHANGED_BY = createdBy,
                INVOICE_NO = invoiceNo,
                LinkedServer = (system != null) ? system.SYSTEM_VALUE_TEXT : string.Empty
            };
            db.Execute("UpdateEfaktur", args2);

        }

        public AjaxResult SaveInvoice(IDBContext db, Invoice invoice, List<GR_IR_DATA> items, string createdBy, List<InvoiceAttachment> attachments, string path)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    INVOICE_NO = invoice.INVOICE_NO,
                    INVOICE_DATE = invoice.S_INVOICE_DATE.FormatSQLDate(),
                    TURN_OVER = invoice.TURN_OVER,
                    TAX_BASE = invoice.TAX_BASE,
                    CALCULATE_TAX = invoice.CALCULATE_TAX,
                    CHECKBOX_STAMP = invoice.CHECKBOX_STAMP,
                    STAMP_AMOUNT = invoice.STAMP_AMOUNT,
                    TOTAL_AMOUNT = invoice.TOTAL_AMOUNT,
                    STATUS = "CREATED",
                    SUPPLIER_CD = items[0].VEND_CODE,
                    CURRENCY = items[0].CURRENCY,
                    TAX_INVOICE_NO = invoice.TAX_INVOICE_NO,
                    CERTIFICATE_ID = invoice.CERTIFICATE_ID,
                    TAX_INVOICE_AMOUNT = invoice.TAX_INVOICE_AMOUNT,
                    CREATED_BY = createdBy,
                    WITHOLDING_TAX_CD = invoice.WITHOLDING_TAX_CD //20200605
                };

                //db.Execute("InsertInvoice", args);
                long invoiceId = db.ExecuteScalar<long>("InsertInvoice", args);

                int i = 0;
                //if (invoice.InvoiceAttachmentList != null)
                //{
                //    foreach (InvoiceAttachment invoiceAttachment in invoice.InvoiceAttachmentList)
                //    {
                //        char c = Convert.ToChar(65 + i);

                //        dynamic argsInvoiceAttach = new
                //        {
                //            INVOICE_ID = invoiceId,
                //            FILE_NAME = invoiceAttachment.FILE_NAME,
                //            FILE_NAME_SERVER = invoiceAttachment.FILE_NAME_SERVER,
                //            CREATED_BY = createdBy
                //        };
                //        db.Execute("InsertInvoiceAttachment", argsInvoiceAttach);

                //        i++;
                //    }
                //}

                //insert invoice attachment new
                if (attachments != null)
                {
                    foreach (InvoiceAttachment attachment in attachments)
                    {
                        //get attachment temp
                        dynamic argsAttachmentGet = new
                        {
                            ATTACHMENT_TYPE = attachment.ATTACHMENT_TYPE,
                            FILE_NAME = attachment.FILE_NAME,
                            CREATED_BY = createdBy
                        };
                        InvoiceAttachment attachmentTemp = db.SingleOrDefault<InvoiceAttachment>("GetInvoiceAttachmentTemp", argsAttachmentGet);

                        //convert filename attachment
                        string filenameNew = ConvertFileNameAttachment(attachment.ATTACHMENT_TYPE, attachmentTemp.FILE_NAME, items[0].VEND_CODE, createdBy, invoiceId.ToString());

                        //update 04-01-2021 [START]
                        string[] arrayInvDate = invoice.S_INVOICE_DATE.Split('.');
                        DateTime temp = new DateTime(Int32.Parse(arrayInvDate[2]), Int32.Parse(arrayInvDate[1]), Int32.Parse(arrayInvDate[0]));
                        string tahun = temp.ToString("yyyy");
                        string bulan = temp.ToString("MMMM");

                        //string tahun = invoice.INVOICE_DATE != null ? ((DateTime)invoice.INVOICE_DATE).ToString("yyyy") : DateTime.Now.ToString("yyyy");
                        //string bulan = invoice.INVOICE_DATE != null ? ((DateTime)invoice.INVOICE_DATE).ToString("MMMM") : DateTime.Now.ToString("MMMM");
                        //update 04-01-2021 [END]

                        string supplier = items[0].VEND_CODE;
                        string addPath = Path.Combine(tahun, bulan.ToUpper(), supplier);
                        string newPath = Path.Combine(path, addPath);

                        string sourceFile = Path.Combine(path, attachmentTemp.FILE_NAME_SERVER);
                        string destFile = Path.Combine(newPath, filenameNew);

                        System.IO.Directory.CreateDirectory(newPath);
                        System.IO.File.Move(sourceFile, destFile);

                        //insert attachment
                        dynamic argsAttachmentInsert = new
                        {
                            INVOICE_ID = invoiceId,
                            ATTACHMENT_TYPE = attachmentTemp.ATTACHMENT_TYPE,
                            FILE_NAME = filenameNew,
                            FILE_NAME_SERVER = filenameNew,
                            CREATED_BY = createdBy
                        };
                        db.Execute("InsertInvoiceAttachment", argsAttachmentInsert);

                        //delete attachment temp
                        dynamic argsAttachmentDelete = new
                        {
                            ATTACHMENT_TYPE = attachmentTemp.ATTACHMENT_TYPE,
                            FILE_NAME = attachmentTemp.FILE_NAME,
                            CREATED_BY = createdBy
                        };
                        db.Execute("DeleteInvoiceAttachmentTemp", argsAttachmentDelete);
                    }
                }
                //insert invoice attachment new

                for (i = 0; i < items.Count; i++)
                {
                    dynamic args1 = new
                    {
                        VEND_CODE = items[i].VEND_CODE,
                        GR_IR_ID = items[i].GR_IR_ID,
                        INVOICE_ID = invoiceId,
                        UPDATED_BY = createdBy
                    };
                    db.Execute("UpdateGRIRDataRev", args1);
                }


                /*
                 * create specific function to do update efaktur
                 * 
                dynamic args2 = new
                {
                    TAX_INVOICE_NO = invoice.TAX_INVOICE_NO,
                    IS_USED = "1",
                    CHANGED_BY = createdBy
                };
                db.Execute("UpdateEfaktur", args2);*/

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                ajaxResult.Params = new String[] { invoiceId.ToString(), items[0].VEND_CODE, invoice.CERTIFICATE_ID };
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

        //add by fid ahmad 04.10.2022
        public long CheckDuplicateInvoice(IDBContext db, string InvoiceNo, string SupplierCode)
        {
            AjaxResult ajaxResult = new AjaxResult();
            long CountData = 0;

            try
            {
                dynamic args = new
                {
                    INVOICE_NO = InvoiceNo,
                    SUPPLIER_CD = SupplierCode
                };

                CountData = db.ExecuteScalar<long>("CheckDuplicateInvoice", args);

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] {
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message),
                };
            }

            return CountData;
        }
        public string getNewCertificateSeq(IDBContext db, string vendorCode, string invoiceNo,
            string datenow)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            long newSeq = 0;

            try
            {
                dynamic args = new
                {
                    VENDOR_CD = vendorCode,
                    INVOICE_NO = invoiceNo,
                    DATENOW = datenow
                };

                newSeq = db.ExecuteScalar<long>("GetNewCertificateSeq", args);

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

            return newSeq.ToString();
        }

        public string getToleranceValue()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            string result = "";

            try
            {
                result = db.ExecuteScalar<string>("GetToleranceValue");
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

            return result;
        }

        public string checkGRStatus(string param)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            string result = "Y";
            try
            {
                string[] header = param.Split(';');
                foreach (string h in header)
                {
                    string[] detail = h.Split('|');
                    dynamic args = new
                    {
                        GR_SA_NO = detail[0],
                        GR_SA_ITEM_NO = detail[1],
                        USER_ID = detail[2]
                    };
                    result = db.ExecuteScalar<string>("InvoiceCreationCheckGRStatus", args);
                    if (result != "Y")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { string.Format("{0} = {1}", ex.GetType().FullName, ex.Message) };
            }
            return result;
        }

        #region po category 20200515
        public string getPoCategory(string keys)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            string result = "";
            int countg = 0;
            int counts = 0;

            string[] header = keys.Split(';');
            foreach (string h in header)
            {
                string[] detail = h.Split(',');
                dynamic args = new
                {
                    GR_IR_ID = detail[0]
                };
                result = db.SingleOrDefault<string>("getPoCategory", args);
                if (result == "GOODS")
                {
                    countg = countg + 1;
                }
                if (result == "SERVICE")
                {
                    counts = counts + 1;
                }
            }

            if (countg > counts)
            {
                result = "GOODS";
            }

            if (countg < counts)
            {
                result = "SERVICE";
            }

            if (countg.Equals(counts))
            {
                result = "GOODS";
            }

            db.Close();
            return result;
        }
        #endregion

        #region po category excel 20200717
        public string getPoCategoryExcel(string keys)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            string result = "";
            int countg = 0;
            int counts = 0;

            string[] header = keys.Split(';');
            foreach (string h in header)
            {
                string[] detail = h.Split(',');
                dynamic args = new
                {
                    PO_NUMBER = detail[0],
                    PO_ITEM = detail[1],
                    MATDOC_NUMBER = detail[2],
                    MATDOC_ITEM = detail[3]
                };
                result = db.SingleOrDefault<string>("getPoCategoryExcel", args);
                if (result == "GOODS")
                {
                    countg = countg + 1;
                }
                if (result == "SERVICE")
                {
                    counts = counts + 1;
                }
            }

            if (countg > counts)
            {
                result = "GOODS";
            }

            if (countg < counts)
            {
                result = "SERVICE";
            }

            if (countg.Equals(counts))
            {
                result = "GOODS";
            }

            db.Close();
            return result;
        }
        #endregion

        #region validasi VAT WHT 20200518
        public IEnumerable<Supplier> getValidVATWHT(string suppCode)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            string result = null;
            dynamic args = new
            {
                SUPPLIER_CD = suppCode
            };
            IEnumerable<Supplier> list = db.Fetch<Supplier>("getValidVATWHT", args);
            db.Close();
            return list;
        }
        #endregion

        #region valid wht supplier 20200714
        public string getSuppWHT(string suppCd)
        {
            string result = null;
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SUPPLIER_CD = suppCd
            };
            result = db.SingleOrDefault<string>("getSuppWHT", args);
            db.Close();
            return result;
        }
        #endregion

        #region exist invoice 20200707
        public string existInvo(string kode)
        {

            string result = null;
            int exist = 0;
            int notexist = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            string[] header = kode.Split(';');
            foreach (string h in header)
            {
                string[] detail = h.Split(',');
                dynamic args = new
                {
                    PO_NUMBER = detail[0],
                    PO_ITEM = detail[1],
                    MATDOC_NUMBER = detail[2],
                    MATDOC_ITEM = detail[3]
                };
                result = db.SingleOrDefault<string>("getExistInvoExcel", args);
                if (result == "EXIST")
                {
                    exist = exist + 1;
                }
                if (result == "NOT EXIST")
                {
                    notexist = notexist + 1;
                }
            }

            if (exist > 0)
            {
                result = "EXIST";
            }

            if (notexist > header.Length)
            {
                result = "NOT EXIST";
            }

            db.Close();
            return result;
        }
        #endregion

        #region get gr ir 20200723
        public string getGrIr(string kode)
        {
            string result = "";
            IDBContext db = DatabaseManager.Instance.GetContext();
            List<string> nstr = new List<string>();
            string[] header = kode.Split(';');
            foreach (string h in header)
            {
                string[] detail = h.Split(',');
                dynamic args = new
                {
                    PO_NUMBER = detail[0],
                    PO_ITEM = detail[1],
                    MATDOC_NUMBER = detail[2],
                    MATDOC_ITEM = detail[3]
                };
                result = db.SingleOrDefault<string>("getGrIrExcel", args);
                nstr.Add(result);
            }
            string nresult = string.Join(",", nstr.ToArray());
            return nresult;
        }
        #endregion

        #region checkgrstatus 20200814
        public string checkGrStatusExcel(string keyy)
        {
            string hasil = null;
            string data = null;
            int exist = 0;
            int notexist = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            string[] header = keyy.Split(';');
            foreach (string h in header)
            {
                string[] detail = h.Split(',');
                dynamic args = new
                {
                    PO_NUMBER = detail[0],
                    PO_ITEM = detail[1],
                    MATDOC_NUMBER = detail[2],
                    MATDOC_ITEM = detail[3]
                };
                hasil = db.SingleOrDefault<string>("checkGrStatusxls", args);
                if (hasil == "APPROVED")
                {
                    exist = exist + 1;
                }
                else
                {
                    notexist = notexist + 1;
                }
            }
            if (notexist > 0)
            {
                data = "NOT APPROVED";
            }

            if (exist == header.Length)
            {
                data = "APPROVED";
            }
            return data;
        }
        #endregion

        #region check Invoice Hardcopy Status
        public List<Invoice> getInvoiceByHardcopyStatus(string supplierCd)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                SUPPLIER_CD = supplierCd
            };
            List<Invoice> result = db.Fetch<Invoice>("GetInvoiceByHardcopyStatus", args);
            db.Close();

            return result;
        }
        #endregion

        #region invoice attachment temp
        public AjaxResult insertInvoiceAttachmentTemp(string attachmentType, string filename, string filenameServer, string createdBy)
        {
            AjaxResult ajaxResult = new AjaxResult();

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                ATTACHMENT_TYPE = attachmentType,
                FILE_NAME = filename,
                FILE_NAME_SERVER = filenameServer,
                CREATED_BY = createdBy
            };
            db.Execute("InsertInvoiceAttachmentTemp", args);

            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.Params = new object[] { filename, filenameServer };
            ajaxResult.SuccMesgs = new String[] { "Upload Attachment " + attachmentType.Replace('_', ' ') + " Success" };

            return ajaxResult;
        }

        public AjaxResult deleteInvoiceAttachmentTemp(string attachmentType, string filename, string createdBy)
        {
            AjaxResult ajaxResult = new AjaxResult();

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                ATTACHMENT_TYPE = attachmentType,
                FILE_NAME = filename,
                CREATED_BY = createdBy
            };
            db.Execute("DeleteInvoiceAttachmentTemp", args);

            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.SuccMesgs = new String[] { "Delete Attachment " + attachmentType.Replace('_', ' ') + " Success" };

            return ajaxResult;
        }

        public string ConvertFileNameAttachment(string attachmentType, string fileName, string supplierCode, string createdBy, string invoiceId)
        {
            string result;
            string attachmentCode;

            fileName = fileName.Replace(' ', '_');

            SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
            SystemProperty.SystemProperty system = systemPropertyRepo.GetSysPropByCodeAndType(attachmentType, CommonConstant.SYSTEM_TYPE_ATTACHMENT_FORMAT_FILENAME);

            attachmentCode = system.SYSTEM_VALUE_TEXT;

            // (24-11-2020) update format filename with invoice id
            result = String.Format("{0}_{1}_{2}_{3}", attachmentCode, supplierCode, invoiceId, fileName);

            //if (attachmentType.ToUpper().Equals("INVOICE"))
            //{
            //    IDBContext db = DatabaseManager.Instance.GetContext();
            //    dynamic args = new
            //    {
            //        SUPPLIER_CD = supplierCode,
            //        CREATED_BY = createdBy,
            //        SEQ_TYPE = "INVOICE ATTACHMENT"
            //    };
            //    string runNo = db.SingleOrDefault<string>("GetSeqNoInvoiceBySupplier", args);
            //    db.Close();

            //    result = String.Format("{0}_{1}_{2}_{3}", attachmentCode, supplierCode, runNo.PadLeft(3, '0'), fileName);
            //}
            //else
            //{
            //    result = String.Format("{0}_{1}_{2}", attachmentCode, supplierCode, fileName);
            //}

            return result;
        }
        #endregion

        //get invoice notice list
        public List<Invoice> GetDashboardNotice(string supplierCode, int fromNumber, int toNumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                NOTICE_FLAG = 'Y',
                SUPPLIER_CD = supplierCode,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<Invoice> result = db.Fetch<Invoice>("GetDashboardNotice", args);
            db.Close();

            return result;
        }

        public int CountDashboardNotice(string supplierCode)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                NOTICE_FLAG = 'Y',
                SUPPLIER_CD = supplierCode
            };

            return db.SingleOrDefault<int>("CountDashboardNotice", args);
        }

    }

}
