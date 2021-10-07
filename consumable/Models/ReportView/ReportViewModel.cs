using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WebForms;


namespace consumable.Models.ReportView
{
    public class ReportViewModel
    {
        public enum ReportFormat { PDF = 1, Word = 2, Excel = 3 }
        public ReportViewModel()
        {
            //initation for the data set holder
            ReportDataSets = new List<ReportDataSet>();
            ReportParamDic = new Dictionary<string, string>();
            PageWidth = "8.5in";
            PageHeight = "11in";
            MarginTop = "0in";
            MarginLeft = "0in";
            MarginRight = "0in";
            MarginBottom = "0in";
        }

        //Device info
        public string PageWidth { get; set; } //8.5in
        public string PageHeight { get; set; } //11in
        public string MarginTop { get; set; } //0n
        public string MarginLeft { get; set; } //0n
        public string MarginRight { get; set; } //0n
        public string MarginBottom { get; set; } //0n

        //Reference to the RDLC file that contain the report definition
        public string FileName { get; set; }

        //The report name for the reprt
        public string ReportName { get; set; }

        //dataset holder
        public List<ReportDataSet> ReportDataSets { get; set; }

        //report format needed
        public ReportFormat Format { get; set; }
        public bool ViewAsAttachment { get; set; }

        //Parameter holder
        public Dictionary<string, string> ReportParamDic { get; set; }

        //an helper class to store the data for each report data set
        public class ReportDataSet
        {
            public string DatasetName { get; set; }
            public List<object> DataSetData { get; set; }
        }

        public string ReportExportFileName
        {
            get
            {
                return string.Format("attachment; filename={0}.{1}", this.ReportName, this.ReportExportExtension);
            }
        }
        public string ReportExportExtension
        {
            get
            {
                switch (this.Format)
                {
                    case ReportViewModel.ReportFormat.Word: return ".doc";
                    case ReportViewModel.ReportFormat.Excel: return ".xls";
                    default:
                        return ".pdf";
                }
            }
        }

        public string LastMimeType
        {
            get
            {
                return mimeType;
            }
        }
        private string mimeType;

        public byte[] RenderReport()
        {
            //geting repot data from the business object

            //creating a new report and setting its path
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(this.FileName);

            //adding the reort datasets with there names
            if (ReportDataSets != null)
            {
                foreach (var dataset in this.ReportDataSets)
                {
                    ReportDataSource reportDataSource = new ReportDataSource(dataset.DatasetName, dataset.DataSetData);
                    localReport.DataSources.Add(reportDataSource);
                }
            }

            //setting the partameters for the report
            if (ReportParamDic != null)
            {
                foreach (KeyValuePair<string, string> item in ReportParamDic)
                {
                    localReport.SetParameters(new ReportParameter(item.Key, item.Value));
                }
            }
            //localReport.SetParameters(new ReportParameter("RightMainTitle", this.RightMainTitle));
            //localReport.SetParameters(new ReportParameter("RightSubTitle", this.RightSubTitle));
            //localReport.SetParameters(new ReportParameter("LeftMainTitle", this.LeftMainTitle));
            //localReport.SetParameters(new ReportParameter("LeftSubTitle", this.LeftSubTitle));
            //localReport.SetParameters(new ReportParameter("ReportTitle", this.ReportTitle));
            //localReport.SetParameters(new ReportParameter("ReportLogo", System.Web.HttpContext.Current.Server.MapPath(this.ReportLogo)));
            //localReport.SetParameters(new ReportParameter("ReportDate", this.ReportDate.ToShortDateString()));
            //localReport.SetParameters(new ReportParameter("UserNamPrinting", this.UserNamPrinting));

            //enabling external images
            localReport.EnableExternalImages = true;

            //preparing to render the report

            string reportType = this.Format.ToString();

            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + this.Format.ToString() + "</OutputFormat>" +
                /*
                "  <PageWidth>" + this.PageWidth + "</PageWidth>" +
                "  <PageHeight>" + this.PageHeight + "</PageHeight>" +
                "  <MarginTop>" + this.MarginTop + "</MarginTop>" +
                "  <MarginLeft>" + this.MarginLeft + "</MarginLeft>" +
                "  <MarginRight>" + this.MarginRight + "</MarginRight>" +
                "  <MarginBottom>" + this.MarginBottom + "</MarginBottom>" +
                */
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return renderedBytes;
        }
    }
}
