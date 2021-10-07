using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Data;
using NPOI.XSSF.UserModel;

namespace consumable.Commons
{
    public partial class NPOIWriter : IExcelWriter
    {
        private const int MaxNumberOfRowPerSheet = 65500;
        private const int MaximumSheetNameLength = 25;

        private HSSFWorkbook Workbook { get; set; }
        protected HSSFWorkbook XlsxWorkbook { get; set; }

        public NPOIWriter()
        {
            this.Workbook = new HSSFWorkbook();
            this.XlsxWorkbook = new HSSFWorkbook();
        }

        /// <summary>
        /// Create Table Header
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        protected ISheet CreateColumnHeader<T>(IEnumerable<T> dataSource, string sheetName)
        {
            ISheet sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));

            /* styling cell */
            ICellStyle headerStyle = this.Workbook.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderTop = BorderStyle.THIN;

            headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE.index;
            headerStyle.FillPattern = FillPatternType.THIN_HORZ_BANDS;
            headerStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE.index;

            headerStyle.WrapText = false;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.Alignment = HorizontalAlignment.CENTER;

            IFont headerLabelFont = this.Workbook.CreateFont();
            headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerStyle.SetFont(headerLabelFont);

            IRow row = sheet.CreateRow(0);

            string cellValue = string.Empty;

            object obj = new object();
            if (dataSource.Any())
                obj = dataSource.ElementAt(0);

            Type t = obj.GetType();
            PropertyInfo[] pis = t.GetProperties();

            int i = 0;
            foreach (PropertyInfo pi in pis)
            {
                ICell cell = row.CreateCell(i);

                cellValue = pi.Name.ToString().Contains("_") ? pi.Name.ToString().Replace("_", " ") : pi.Name.ToString();
                cell.SetCellValue(cellValue);

                if (headerStyle != null)
                    cell.CellStyle = headerStyle;

                i++;
            }

            return sheet;
        }

        protected ISheet CreateColumnHeader(DataTable dataSource, string sheetName)
        {
            ISheet sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));

            /* styling cell */
            ICellStyle headerStyle = this.Workbook.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderTop = BorderStyle.THIN;

            headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE.index;
            headerStyle.FillPattern = FillPatternType.THIN_HORZ_BANDS;
            headerStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE.index;

            headerStyle.WrapText = true;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.Alignment = HorizontalAlignment.CENTER;

            IFont headerLabelFont = this.Workbook.CreateFont();
            headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerStyle.SetFont(headerLabelFont);

            IRow row = sheet.CreateRow(0);

            for (int colIndex = 0; colIndex < dataSource.Columns.Count; colIndex++)
            {
                ICell cell = row.CreateCell(colIndex);
                cell.SetCellValue(dataSource.Columns[colIndex].ColumnName);

                if (headerStyle != null)
                    cell.CellStyle = headerStyle;
            }

            return sheet;
        }

        protected ISheet CreateColumnFooter(ISheet sheet, string[] footerText)
        {
            /* styling cell */
            ICellStyle footerStyle = this.Workbook.CreateCellStyle();
            //footerStyle.BorderBottom = BorderStyle.THIN;
            //footerStyle.BorderLeft = BorderStyle.THIN;
            //footerStyle.BorderRight = BorderStyle.THIN;
            //footerStyle.BorderTop = BorderStyle.THIN;

            footerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
            footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            //footerStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.WHITE.index;

            footerStyle.Indention = 1;
            footerStyle.WrapText = false;
            footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle.Alignment = HorizontalAlignment.LEFT;

            IFont footerLabelFont = this.Workbook.CreateFont();
            footerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            footerLabelFont.Color = NPOI.HSSF.Util.HSSFColor.BLACK.index;
            footerStyle.SetFont(footerLabelFont);

            int lastRowIndex = sheet.LastRowNum;
            int footerRowIndex = 0;
            int counter = 2;
            string cellValue = string.Empty;

            foreach (string footer in footerText)
            {
                footerRowIndex = lastRowIndex + counter;
                IRow row = sheet.CreateRow(footerRowIndex);

                ICell cell = row.CreateCell(0);

                cellValue = footer;
                cell.SetCellValue(cellValue);
                cell.CellStyle = footerStyle;

                CellRangeAddress cellRange = new CellRangeAddress(footerRowIndex, footerRowIndex, 0, 10);
                sheet.AddMergedRegion(cellRange);

                counter++;
            }

            return sheet;
        }

        public static string EscapeSheetName(string sheetName)
        {
            string escapedSheetName = sheetName
                                        .Replace("/", "-")
                                        .Replace("\\", " ")
                                        .Replace("?", string.Empty)
                                        .Replace("*", string.Empty)
                                        .Replace("[", string.Empty)
                                        .Replace("]", string.Empty)
                                        .Replace(":", string.Empty);

            if (escapedSheetName.Length > MaximumSheetNameLength)
                escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

            return escapedSheetName;
        }


        /// <summary>
        /// Create Content Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public byte[] Write<T>(IEnumerable<T> dataSource, string sheetName)
        {
            if (this.Workbook == null)
                this.Workbook = new HSSFWorkbook();

            byte[] result;
            if (dataSource == null)
                throw new ArgumentNullException("Data source cannot be null!!!");
            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException("Sheet Name, cann0t be null!!");

            ISheet sheet = CreateColumnHeader(dataSource, sheetName);

            int rowIndex = 1;
            int i = 0;
            object value;
            if (dataSource.Any())
            {
                Type t = dataSource.ElementAt(0).GetType();
                PropertyInfo[] pis = t.GetProperties();

                ICellStyle headerStyle = this.Workbook.CreateCellStyle();
                headerStyle.BorderBottom = BorderStyle.THIN;
                headerStyle.BorderLeft = BorderStyle.THIN;
                headerStyle.BorderRight = BorderStyle.THIN;
                headerStyle.BorderTop = BorderStyle.THIN;

                ICell cell;
                foreach (object obj in dataSource)
                {
                    IRow row = sheet.CreateRow(rowIndex++);
                    i = 0;
                    foreach (PropertyInfo pi in pis)
                    {
                        value = pi.GetValue(obj, null);
                        cell = row.CreateCell(i);

                        if (value != null)
                            cell.SetCellValue(value.ToString());

                        cell.CellStyle = headerStyle;
                        i++;
                    }
                }
            }

            using (MemoryStream buffer = new MemoryStream())
            {
                this.Workbook.Write(buffer);
                result = buffer.GetBuffer();
            }

            this.Workbook = null;
            return result;
        }

        /// <summary>
        /// Create Content Table With Footer Section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="sheetName"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public byte[] Write<T>(IEnumerable<T> dataSource, string sheetName, string[] footer)
        {
            if (this.Workbook == null)
                this.Workbook = new HSSFWorkbook();

            byte[] result;
            if (dataSource == null)
                throw new ArgumentNullException("Data source cannot be null!!!");
            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException("Sheet Name, cann0t be null!!");

            ISheet sheet = CreateColumnHeader(dataSource, sheetName);

            int rowIndex = 1;
            int i = 0;
            object value;
            if (dataSource.Any())
            {
                Type t = dataSource.ElementAt(0).GetType();
                PropertyInfo[] pis = t.GetProperties();

                ICellStyle headerStyle = this.Workbook.CreateCellStyle();
                headerStyle.BorderBottom = BorderStyle.THIN;
                headerStyle.BorderLeft = BorderStyle.THIN;
                headerStyle.BorderRight = BorderStyle.THIN;
                headerStyle.BorderTop = BorderStyle.THIN;

                ICell cell;
                foreach (object obj in dataSource)
                {
                    IRow row = sheet.CreateRow(rowIndex++);
                    i = 0;
                    foreach (PropertyInfo pi in pis)
                    {
                        value = pi.GetValue(obj, null);
                        cell = row.CreateCell(i);

                        if (value != null)
                            cell.SetCellValue(value.ToString());

                        cell.CellStyle = headerStyle;

                        i++;
                    }
                }

                CreateColumnFooter(sheet, footer);
            }

            using (MemoryStream buffer = new MemoryStream())
            {
                this.Workbook.Write(buffer);
                result = buffer.GetBuffer();
            }

            this.Workbook = null;
            return result;
        }

        public byte[] Write(DataTable dataSource, string sheetName)
        {
            if (this.Workbook == null)
                this.Workbook = new HSSFWorkbook();

            byte[] result;
            if (dataSource == null)
                throw new ArgumentNullException("Data source cannot be null!!!");
            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException("Sheet Name, cannot be null!!");

            ISheet sheet = CreateColumnHeader(dataSource, sheetName);

            if (dataSource != null)
            {
                ICellStyle headerStyle = this.Workbook.CreateCellStyle();
                headerStyle.BorderBottom = BorderStyle.THIN;
                headerStyle.BorderLeft = BorderStyle.THIN;
                headerStyle.BorderRight = BorderStyle.THIN;
                headerStyle.BorderTop = BorderStyle.THIN;

                int currentNPOIRowIndex = 1;
                int sheetCount = 1;

                for (int rowIndex = 0; rowIndex < dataSource.Rows.Count; rowIndex++)
                {
                    if (currentNPOIRowIndex >= MaxNumberOfRowPerSheet)
                    {
                        sheetCount++;
                        currentNPOIRowIndex = 1;

                        sheet = CreateColumnHeader(dataSource, sheetName + " - " + sheetCount);
                    }

                    IRow row = sheet.CreateRow(currentNPOIRowIndex++);

                    for (int colIndex = 0; colIndex < dataSource.Columns.Count; colIndex++)
                    {
                        ICell cell = row.CreateCell(colIndex);
                        cell.SetCellValue(dataSource.Rows[rowIndex][colIndex].ToString());
                    }
                }
            }

            using (MemoryStream buffer = new MemoryStream())
            {
                this.Workbook.Write(buffer);
                result = buffer.GetBuffer();
            }

            this.Workbook = null;
            return result;
        }

        public byte[] WriteXLSx(DataTable dataSource, string sheetName)
        {

            if (this.XlsxWorkbook == null)
                this.XlsxWorkbook = new HSSFWorkbook();

            byte[] result;
            if (dataSource == null)
                throw new ArgumentNullException("Data source cannot be null!!!");
            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException("Sheet Name, cannot be null!!");

            ISheet sheet = CreateColumnHeaderXlsx(dataSource, sheetName);

            if (dataSource != null)
            {
                ICellStyle headerStyle = this.XlsxWorkbook.CreateCellStyle();
                headerStyle.BorderBottom = BorderStyle.THIN;
                headerStyle.BorderLeft = BorderStyle.THIN;
                headerStyle.BorderRight = BorderStyle.THIN;
                headerStyle.BorderTop = BorderStyle.THIN;

                int currentNPOIRowIndex = 1;
                int sheetCount = 1;

                for (int rowIndex = 0; rowIndex < dataSource.Rows.Count; rowIndex++)
                {
                    if (currentNPOIRowIndex >= MaxNumberOfRowPerSheet)
                    {
                        sheetCount++;
                        currentNPOIRowIndex = 1;

                        sheet = CreateColumnHeader(dataSource, sheetName + " - " + sheetCount);
                    }

                    IRow row = sheet.CreateRow(currentNPOIRowIndex++);

                    for (int colIndex = 0; colIndex < dataSource.Columns.Count; colIndex++)
                    {
                        ICell cell = row.CreateCell(colIndex);
                        cell.SetCellValue(dataSource.Rows[rowIndex][colIndex].ToString());
                    }
                }
            }

            using (MemoryStream buffer = new MemoryStream())
            {
                this.XlsxWorkbook.Write(buffer);
                result = buffer.GetBuffer();
            }

            this.XlsxWorkbook = null;
            return result;
        }

        protected ISheet CreateColumnHeaderXlsx(DataTable dataSource, string sheetName)
        {
            ISheet sheet = this.XlsxWorkbook.CreateSheet(EscapeSheetName(sheetName));

            /* styling cell */
            ICellStyle headerStyle = this.XlsxWorkbook.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderTop = BorderStyle.THIN;

            headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE.index;
            headerStyle.FillPattern = FillPatternType.THIN_HORZ_BANDS;
            headerStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_ORANGE.index;

            headerStyle.WrapText = true;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.Alignment = HorizontalAlignment.CENTER;

            IFont headerLabelFont = this.XlsxWorkbook.CreateFont();
            headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerStyle.SetFont(headerLabelFont);

            IRow row = sheet.CreateRow(0);

            for (int colIndex = 0; colIndex < dataSource.Columns.Count; colIndex++)
            {
                ICell cell = row.CreateCell(colIndex);
                cell.SetCellValue(dataSource.Columns[colIndex].ColumnName);

                if (headerStyle != null)
                    cell.CellStyle = headerStyle;
            }

            return sheet;
        }

        public static ICellStyle createCellStyleData(HSSFWorkbook wb)
        {
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderTop = BorderStyle.THIN;
            return headerStyle;
        }

        public static ICellStyle createCellStyleColumnHeader(HSSFWorkbook wb)
        {
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderTop = BorderStyle.THIN;

            headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_BLUE.index;
            headerStyle.FillPattern = FillPatternType.THIN_HORZ_BANDS;
            headerStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_BLUE.index;

            headerStyle.WrapText = true;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.Alignment = HorizontalAlignment.CENTER;

            IFont headerLabelFont = wb.CreateFont();
            headerLabelFont.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
            headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
            headerStyle.SetFont(headerLabelFont);
            return headerStyle;
        }

        public static ICellStyle createCellStyleDataDate(HSSFWorkbook wb, short dataFormat)
        {
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderTop = BorderStyle.THIN;
            headerStyle.DataFormat = dataFormat;

            return headerStyle;
        }

        public static ICellStyle createCellStyleColumnHeader(HSSFWorkbook wb, short format)
        {
            ICellStyle headerStyle = createCellStyleColumnHeader(wb);
            headerStyle.DataFormat = format;

            return headerStyle;
        }

        public static void createCellText(
            IRow row,
            ICellStyle cellStyle,
            int colIdx, string txt)
        {
            ICell cell = row.CreateCell(colIdx);

            if (txt != null)
            {
                cell.SetCellValue(txt);
                cell.SetCellType(CellType.STRING);
            }
            else
            {
                cell.SetCellType(CellType.BLANK);
            }

            if (cellStyle != null)
            {
                cell.CellStyle = cellStyle;
            }
        }

        public static void createCellDate(
            IRow row,
            ICellStyle cellStyle,
            int colIdx, DateTime? val)
        {
            ICell cell = row.CreateCell(colIdx);

            if (val != null)
            {
                cell.SetCellValue((DateTime)val);
                cell.SetCellType(CellType.NUMERIC);
            }
            else
            {
                cell.SetCellType(CellType.BLANK);
            }

            if (cellStyle != null)
            {
                cell.CellStyle = cellStyle;
            }
        }

        public static void writeColHeader(
            HSSFWorkbook wb, ISheet sheet, int startRow,
            ICellStyle colStyleHeader,
            string[] colHeaders)
        {
            IRow row =
                sheet.CreateRow(startRow);

            for (int i = 0; i < colHeaders.Length; i++)
            {
                ICell cell = row.CreateCell(i);

                cell.SetCellValue(colHeaders[i]);

                if (colStyleHeader != null)
                {
                    cell.CellStyle = colStyleHeader;
                }
            }
        }

        public static void createCellDouble(
            IRow row,
            ICellStyle cellStyle,
            int colIdx, double? val)
        {
            ICell cell = row.CreateCell(colIdx);

            if (val != null)
            {
                cell.SetCellValue((double)val);
                cell.SetCellType(CellType.NUMERIC);
            }
            else
            {
                cell.SetCellType(CellType.BLANK);
            }

            if (cellStyle != null)
            {
                cell.CellStyle = cellStyle;
            }
        }

        public static void CreateMergedColHeader(HSSFWorkbook wb, ISheet sheet, int startRow, int endRow, int startCol,
            int endCol, ICellStyle colStyleHeader, string cellValue)
        {
            IRow row = null;

            for (var i = startRow; i <= endRow; i++)
            {
                row = sheet.GetRow(i) ?? sheet.CreateRow(i);
                for (var j = startCol; j <= endCol; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = colStyleHeader;
                    cell.SetCellValue(cellValue);
                }
            }

            var cellRange = new CellRangeAddress(startRow, endRow, startCol, endCol);
            sheet.AddMergedRegion(cellRange);
        }

        public static void CreateSingleColHeader(HSSFWorkbook wb, ISheet sheet, int rows, int col, ICellStyle colStyleHeader, string cellValue)
        {
            IRow row = sheet.GetRow(rows) ?? sheet.CreateRow(rows);

            ICell cell = row.CreateCell(col);
            cell.CellStyle = colStyleHeader;
            cell.SetCellValue(cellValue);
        }
    }
}
