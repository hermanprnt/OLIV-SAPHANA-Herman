using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models
{
    public class VAT_OUT_EXCELRepository
    {
        private VAT_OUT_EXCELRepository() { }
        private static VAT_OUT_EXCELRepository instance = null;
        public static VAT_OUT_EXCELRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VAT_OUT_EXCELRepository();
                }

                return instance;
            }
        }

        public VAT_OUT_EXCELREPORT GetListData(DateTime? TaxInvoiceDateFrom, DateTime? TaxInvoiceDateTo, DateTime? InterfaceDateFrom,
                     DateTime? InterfaceDateTo, DateTime? DownloadDateFrom, DateTime? DownloadDateTo, DateTime? CompleteDateFrom,
                     DateTime? CompleteDateTo, String TaxInvoiceNoFrom, String TaxInvoiceNoTo, String CustomerName,
                     String CustomerNPWP, String Status)
        {
            IEnumerable<VAT_OUT_H> ListVAT_OUT_H = VAT_OUT_HRepository.Instance.GetAllData(TaxInvoiceDateFrom, TaxInvoiceDateTo, InterfaceDateFrom, InterfaceDateTo, DownloadDateFrom, DownloadDateTo, CompleteDateFrom, CompleteDateTo, TaxInvoiceNoFrom, TaxInvoiceNoTo, CustomerName, CustomerNPWP, Status);
            VAT_OUT_EXCELREPORT result = new VAT_OUT_EXCELREPORT();
            result.VAT_OUT = ListVAT_OUT_H;
            decimal TotalDPPTemp = 0;
            decimal TotalVATTemp = 0;
            decimal TotalPPnBMTemp = 0;
            foreach (var item in result.VAT_OUT)
            {
                TotalDPPTemp = TotalDPPTemp + item.DPP_PRICE;
                TotalVATTemp = TotalVATTemp + item.VAT_PRICE;
                TotalPPnBMTemp = TotalPPnBMTemp + item.LUXURY_TAX_PRICE;
            }

            result.TotalDPP = TotalDPPTemp;
            result.TotalVAT = TotalVATTemp;
            result.TotalPPnBM = TotalPPnBMTemp;

            return result;
        }
    }
}