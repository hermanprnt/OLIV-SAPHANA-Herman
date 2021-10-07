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

namespace consumable.Models.InvoiceCreation
{
    public class InvoiceCreationRepository
    {
        private InvoiceCreationRepository() { }

        #region Singleton
        private static InvoiceCreationRepository instance = null;
        public static InvoiceCreationRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new InvoiceCreationRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countInvoiceCreationList(string poNoSearch, string supplierSearch, string poDateSearch, 
            string statusSearch, string poTextSearch, string receivingDateSearch, string matDocNoSearch)
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
                PO_TEXT = poTextSearch
            };
            return db.SingleOrDefault<int>("CountInvoiceCreation", args);
        }

        public List<InvoiceCreation> GetInvoiceCreation(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, string poTextSearch, string receivingDateSearch, string matDocNoSearch, int fromNumber, int toNumber)
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
                PO_TEXT = poTextSearch,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<InvoiceCreation> result = db.Fetch<InvoiceCreation>("GetInvoiceCreation", args);
            db.Close();
            return result;
        }

        public List<String> GetInvoiceCreationSort(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, string poTextSearch, string receivingDateSearch, string matDocNoSearch, 
            int fromNumber, int toNumber, string field, string sort)
        {
            List<String> result = new List<String>();
            List<InvoiceCreation> resultItem = new List<InvoiceCreation>();
            resultItem = GetInvoiceCreation(poNoSearch, supplierSearch, poDateSearch, statusSearch,
                poTextSearch, receivingDateSearch, matDocNoSearch, fromNumber, toNumber);

            List<InvoiceCreation> returnResult = new List<InvoiceCreation>();
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
            }

            foreach (InvoiceCreation ic in returnResult)
            {
                string checkbox = null;
                if (ic.TOTAL_QTY <= 0)
                {							         
                    checkbox = "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" name=\"selectedManifest\" onclick=\"javascript:updateManifest()\"  disabled />";                  
                }
                else
                {
                    checkbox = "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" name=\"selectedManifest\" onclick=\"javascript:updateManifest()\" />";
                }

                string invoiceId = null;
                if (ic.INVOICE_ID != null)
                    invoiceId = ic.INVOICE_ID;

                string poDate = ic.PO_DATE.HasValue ? ic.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty;

                string TOTAL_AMOUNT;
                if (ic.MATDOC_CURRENCY.Equals("IDR"))
                { TOTAL_AMOUNT = ic.TOTAL_AMOUNT.FormatCommaSeparator(); }
                else
                { TOTAL_AMOUNT = ic.TOTAL_AMOUNT.FormatCommaSeparatorWithDecimal(); }

                result.Add(
                "<tr>" +
                    "<td width=\"20px\" class=\"text-center grid-checkbox-col\">" +
                        checkbox +
                    "</td>" +
                    "<td width=\"100px\" class=\"text-center cursor-link\">" +
                        "<a onclick=\"openPopupMaterial('" + ic.PO_NUMBER + "', '" + ic.PO_ITEM + "' , '" + ic.PO_DATE.Value.ToString("dd.MM.yyyy") + "', " +
                            "'" + ic.VEND_CODE + "', '" + ic.GR_STATUS + "', '" + ic.PO_TEXT.Replace("\"", "") + "', '" + invoiceId + "')\">" + ic.PO_NUMBER + "</a>" +
                    "</td>" +
                    "<td width=\"80px\" class=\"text-center\">" +
                        ic.PO_ITEM + 
                    "</td>" +
                    "<td width=\"220px\" class=\"text-left ellipsis\" style=\"max-width: 220px;\" title=" + ic.PO_TEXT.Replace("\"", "") + ">" +
                        ic.PO_TEXT + 
                    "</td>" +
                    "<td width=\"75px\" class=\"text-center\">" +
                        poDate + 
                    "</td>" +
                    "<td width=\"100px\" class=\"text-center\">" +
                        ic.VEND_CODE + 
                    "</td>" +
                    "<td width=\"150px\" class=\"text-left ellipsis\" title="+ic.SUPPLIER_NAME+" style=\"max-width: 150px;\">" +
                        ic.SUPPLIER_NAME + 
                    "</td>" +
                    "<td width=\"75px\" class=\"text-right\">" +
                        ic.TOTAL_QTY + 
                    "</td>" +
                    "<td width=\"75px\" class=\"text-center\">" +
                        ic.MATDOC_UNIT + 
                    "</td>" +
                    "<td width=\"130px\" class=\"text-right\">" +
                        TOTAL_AMOUNT + 
                    "</td>" +
                    "<td class=\"text-center\">" +
                        ic.MATDOC_CURRENCY +
                    "</td>" +
                "</tr>");

            }

            return result;
        }

        public List<GR_IR_DATA> GetDownloadGR(string poNoSearch, string supplierSearch, string poDateSearch, 
            string statusSearch, string poTextSearch)
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
                PO_TEXT = poTextSearch
            };

            List<GR_IR_DATA> result = db.Fetch<GR_IR_DATA>("GetGR_IR_DATAExcel", args);
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

            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "PO Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Header Text");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Receiving Date");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Supplier Code");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Material Doc No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "IR Doc No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Status");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Plant");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Movement Type");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Cancel Doc No");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "User ID");
            NPOIWriter.CreateSingleColHeader(wb, sheet, 0, colHeader++, cellStyleHeader, "Invoice No");

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

            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_NUMBER);
            //NPOIWriter.createCellDate(row, cellStyle, col++, item.PO_DATE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PO_DATE.HasValue ? item.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
            NPOIWriter.createCellText(row, cellStyle, col++, item.HEADER_TEXT);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_DATE.HasValue ? item.MATDOC_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);

            NPOIWriter.createCellText(row, cellStyle, col++, item.VEND_CODE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MATDOC_NUMBER);
            NPOIWriter.createCellText(row, cellStyle, col++, item.IR_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.GR_STATUS);
            NPOIWriter.createCellText(row, cellStyle, col++, item.PLANT_CODE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.MOV_TYPE);
            NPOIWriter.createCellText(row, cellStyle, col++, item.CANCEL_DOC_NO);
            NPOIWriter.createCellText(row, cellStyle, col++, item.USRID);

        }

        public AjaxResult SaveInvoice(IDBContext db, Invoice invoice, List<GR_IR_DATA> items, string createdBy)
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
                    CREATED_BY = createdBy
                };

                //db.Execute("InsertInvoice", args);
                long regId = db.ExecuteScalar<long>("InsertInvoice", args);

                int i = 0;
                if (invoice.InvoiceAttachmentList != null)
                {
                    foreach (InvoiceAttachment invoiceAttachment in invoice.InvoiceAttachmentList)
                    {
                        char c = Convert.ToChar(65 + i);

                        dynamic argsInvoiceAttach = new
                        {
                            INVOICE_ID = regId,
                            FILE_NAME = invoiceAttachment.FILE_NAME,
                            FILE_NAME_SERVER = invoiceAttachment.FILE_NAME_SERVER,
                            CREATED_BY = createdBy
                        };
                        db.Execute("InsertInvoiceAttachment", argsInvoiceAttach);

                        i++;
                    }
                }

                for (i = 0; i < items.Count; i++)
                {
                    dynamic args1 = new
                    {
                        PO_NUMBER = items[i].PO_NUMBER,
                        PO_ITEM = items[i].PO_ITEM,
                        VEND_CODE = items[i].VEND_CODE,
                        GR_STATUS = "APPROVED",
                        INVOICE_ID = regId,
                        UPDATED_BY = createdBy
                    };
                    db.Execute("UpdateGRIRData", args1);
                }

                dynamic args2 = new
                {
                    TAX_INVOICE_NO = invoice.TAX_INVOICE_NO,
                    IS_USED = "1",
                    CHANGED_BY = createdBy
                };
                db.Execute("UpdateEfaktur", args2);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
		        ajaxResult.Params = new String[] { regId.ToString(), items[0].VEND_CODE, invoice.CERTIFICATE_ID };
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


    }

}
