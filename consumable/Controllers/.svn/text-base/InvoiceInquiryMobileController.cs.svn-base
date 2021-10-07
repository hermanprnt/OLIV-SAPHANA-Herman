using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.InvoiceInquiry;

namespace consumable.Controllers
{
    public class InvoiceInquiryMobileController : PageController
    {

        private InvoiceInquiryRepository invoiceInquiryRepo = InvoiceInquiryRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Invoice Inquiry";
            //ViewData["InvoiceInquiry"] = (List<InvoiceInquiry>)invoiceInquiryRepo.GetInvoiceInquiry();
        }
    }
}
