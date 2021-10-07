using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using consumable.Common.Data;
using consumable.Models;
using consumable.Models.InvoiceInquiry;
using consumable.Models.ReportView;
using consumable.Models.SystemProperty;
using GenCode128;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Commons
{
    public class PrintReport
    {
        public const string SCREEN_MODE_ADD = "ADD";
        public const string SCREEN_MODE_EDIT = "EDIT";

        public const string TABLEAU_IP = "10.33.1.108";

        private static PrintReport instance = null;
        public static PrintReport Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PrintReport();
                }
                return instance;
            }
        }


        public ReportViewModel GetReportViewModelInvoice(string invoiceId, string supplierCode, string certificateId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            List<BarcodeData> barcodeCertificateIdDatas = new List<BarcodeData>();
            //List<BarcodeData> logoDatas = new List<BarcodeData>();

            ReportViewModel reportViewModel = new ReportViewModel()
            {
                FileName = "~/Reports/Certificate.rdlc",
                ReportName = "Certificate",
                PageWidth = "7.973cm",
                PageHeight = "5.5cm",
                Format = ReportViewModel.ReportFormat.PDF,
                ViewAsAttachment = false,
            };

            SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
            SystemProperty system = systemPropertyRepo.GetSysPropByCodeAndType("IP_ADDRESS", "INVOICE_INQUIRY");

            dynamic args = new
            {
                INVOICE_ID = invoiceId,
                SUPPLIER_CD = supplierCode,
                CERTIFICATE_ID = certificateId,
                LinkedServer = system.SYSTEM_VALUE_TEXT
            };

            InvoiceInquiry invoiceInquiryData = db.SingleOrDefault<InvoiceInquiry>("GetInvoiceInquiryUnique", args);

            if (invoiceInquiryData == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(invoiceInquiryData.CERTIFICATE_ID))
            {
                Image myimgBarcode = Code128Rendering.MakeBarcodeImage(invoiceInquiryData.CERTIFICATE_ID,
                                        int.Parse("1"), true);
                MemoryStream msBarcode = new MemoryStream();
                myimgBarcode.Save(msBarcode, System.Drawing.Imaging.ImageFormat.Gif);

                barcodeCertificateIdDatas.Add(new BarcodeData()
                {
                    Value = invoiceInquiryData.CERTIFICATE_ID,
                    Image = msBarcode.ToArray()
                });

                Bitmap bitmapLogo = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/toyota-logo.png"));
                MemoryStream msLogo = new MemoryStream();
                bitmapLogo.Save(msLogo, System.Drawing.Imaging.ImageFormat.Png);

                barcodeCertificateIdDatas.Add(new BarcodeData()
                {
                    Value = "",
                    Image = msLogo.ToArray()
                });
            }

            //adding parameter to report
            reportViewModel.ReportParamDic.Add("InvoiceNo", invoiceInquiryData.INVOICE_NO ?? "-");
            reportViewModel.ReportParamDic.Add("Supplier", invoiceInquiryData.SUPPLIER_CD + " - " + invoiceInquiryData.SUPPLIER_NAME);
            reportViewModel.ReportParamDic.Add("InvoiceDate", invoiceInquiryData.INVOICE_DATE.HasValue ? invoiceInquiryData.INVOICE_DATE.Value.ToString("dd MMM yyyy") : "-");
            reportViewModel.ReportParamDic.Add("TurnOver", invoiceInquiryData.CURRENCY + " " + invoiceInquiryData.TURN_OVER.FormatCommaSeparator());
            reportViewModel.ReportParamDic.Add("TotalAmount", invoiceInquiryData.CURRENCY + " " + invoiceInquiryData.TOTAL_AMOUNT.FormatCommaSeparator());
            reportViewModel.ReportParamDic.Add("InvoiceTaxNo", invoiceInquiryData.TAX_INVOICE_NO ?? "-");
            reportViewModel.ReportParamDic.Add("InvoiceTaxDate", invoiceInquiryData.TAX_INVOICE_DT.HasValue ? invoiceInquiryData.TAX_INVOICE_DT.Value.ToString("dd MMM yyyy") : "-");
            reportViewModel.ReportParamDic.Add("InvoiceTaxAmount", invoiceInquiryData.CURRENCY + " " + invoiceInquiryData.TAX_INVOICE_AMOUNT.FormatCommaSeparator());

            reportViewModel.ReportParamDic.Add("CertificateId", invoiceInquiryData.CERTIFICATE_ID);

            //adding the dataset information to the report view model object
            reportViewModel.ReportDataSets.Add(new ReportViewModel.ReportDataSet()
            {
                DataSetData = barcodeCertificateIdDatas.ToList<object>(),
                DatasetName = "DataSetBarcodeCertificateId"
            });

            return reportViewModel;
        }



        public ReportViewModel GetReportViewModelPo(string poNumber, string vendCode, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            SEND_PO_HEADER poHeader = null;
            List<SEND_PO_ITEM> poItemDatas = new List<SEND_PO_ITEM>();
            List<BarcodeData> logoDatas = new List<BarcodeData>();

            ReportViewModel reportViewModel = new ReportViewModel()
            {
                FileName = "~/Reports/PO.rdlc",
                ReportName = "PO",
                PageWidth = "5.5cm",
                PageHeight = "7.973cm",
                Format = ReportViewModel.ReportFormat.PDF,
                ViewAsAttachment = false,
            };

            dynamic args = new
            {
                PO_NUMBER = poNumber,
                VEND_CODE = vendCode,
                NoReg = NoReg
            };

            poHeader = db.SingleOrDefault<SEND_PO_HEADER>("GetPoUnique", args);


            if (poHeader != null)
            {
                dynamic args2 = new
                {
                    PO_NUMBER = poNumber,
                    VEND_CODE = vendCode,
                    NoReg = NoReg
                };

                poItemDatas = db.Fetch<SEND_PO_ITEM>("GetPoItemByHeader", args2);

                //adding parameter to report
                reportViewModel.ReportParamDic.Add("PoNumber", poHeader.PO_NUMBER);
                reportViewModel.ReportParamDic.Add("PoDate", poHeader.PO_DATE.HasValue ? poHeader.PO_DATE.Value.ToString("dd.MM.yyyy") : string.Empty);
                reportViewModel.ReportParamDic.Add("VendorCode", poHeader.VEND_CODE);
                reportViewModel.ReportParamDic.Add("VendorName", poHeader.VEND_NAME);
                reportViewModel.ReportParamDic.Add("VendorAddress", poHeader.ADDRESS);
                reportViewModel.ReportParamDic.Add("VendorPostalCode", poHeader.POST_CODE);
                reportViewModel.ReportParamDic.Add("VendorCity", poHeader.CITY);
                reportViewModel.ReportParamDic.Add("VendorAttention", poHeader.ATTENTION);
                reportViewModel.ReportParamDic.Add("VendorPhoneNumber", poHeader.TEL_NUMBER);
                reportViewModel.ReportParamDic.Add("VendorFaxNumber", poHeader.FAX_NUMBER);

                reportViewModel.ReportParamDic.Add("DeliveryName", poHeader.DELIV_NAME);
                reportViewModel.ReportParamDic.Add("DeliveryAddress", poHeader.DELIV_ADDR);
                reportViewModel.ReportParamDic.Add("DeliveryPostalCode", poHeader.DELIV_POST);
                reportViewModel.ReportParamDic.Add("DeliveryCity", poHeader.DELIV_CITY);

                //Double totalAmountItem = poHeader.QTY * poHeader.PRICE;
                //reportViewModel.ReportParamDic.Add("Qty", poHeader.QTY.FormatCommaSeparator());
                //reportViewModel.ReportParamDic.Add("Description", poHeader.DESCRIPTION);
                //reportViewModel.ReportParamDic.Add("Unit", poHeader.UNIT);
                //reportViewModel.ReportParamDic.Add("Item", poHeader.ITEM);
                //reportViewModel.ReportParamDic.Add("Price", poHeader.PRICE.FormatCommaSeparator());
                //reportViewModel.ReportParamDic.Add("TotalAmountItem", totalAmountItem.FormatCommaSeparator());

                reportViewModel.ReportParamDic.Add("SubTotal", poHeader.PO_AMOUNT_TEMP.FormatCommaSeparator());
                reportViewModel.ReportParamDic.Add("Total", poHeader.PO_AMOUNT_TEMP.FormatCommaSeparator());

                reportViewModel.ReportParamDic.Add("ShName", poHeader.SH_NAME);
                if (poHeader.SH_NAME != null)
                {
                    reportViewModel.ReportParamDic.Add("ShSigned", "DIGITAL SIGNED");
                    reportViewModel.ReportParamDic.Add("ShDate", poHeader.SH_DATE.HasValue ? poHeader.SH_DATE.Value.ToString("dd MMM yyyy") : string.Empty);
                }

                reportViewModel.ReportParamDic.Add("DphName", poHeader.DPH_NAME);
                if (poHeader.DPH_NAME != null)
                {
                    reportViewModel.ReportParamDic.Add("DphSigned", "DIGITAL SIGNED");
                    reportViewModel.ReportParamDic.Add("DphDate", poHeader.DPH_DATE.HasValue ? poHeader.DPH_DATE.Value.ToString("dd MMM yyyy") : string.Empty);
                }

                reportViewModel.ReportParamDic.Add("DhName", poHeader.DH_NAME);
                if (poHeader.DH_NAME != null)
                {
                    reportViewModel.ReportParamDic.Add("DhSigned", "DIGITAL SIGNED");
                    reportViewModel.ReportParamDic.Add("DhDate", poHeader.DH_DATE.HasValue ? poHeader.DH_DATE.Value.ToString("dd MMM yyyy") : string.Empty);
                }
                //MemoryStream msBarcode = new MemoryStream();

                //Bitmap bitmapLogo = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/toyota-logo.png"));
                //MemoryStream msLogo = new MemoryStream();
                //bitmapLogo.Save(msLogo, System.Drawing.Imaging.ImageFormat.Png);

                //logoDatas.Add(new BarcodeData()
                //{
                //    Value = "",
                //    Image = msLogo.ToArray()
                //});

                //adding the dataset information to the report view model object
                //reportViewModel.ReportDataSets.Add(new ReportViewModel.ReportDataSet()
                //{
                //    DataSetData = logoDatas.ToList<object>(),
                //    DatasetName = "DataSetBarcodeCertificateId"
                //});

                reportViewModel.ReportDataSets.Add(new ReportViewModel.ReportDataSet()
                {
                    DataSetData = poItemDatas.ToList<object>(),
                    DatasetName = "DataSetPoItem"
                });
            }
            else
            {
                reportViewModel = null;
            }

            return reportViewModel;
        }

    }
}
