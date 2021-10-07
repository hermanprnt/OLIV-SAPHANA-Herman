using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.InvoiceCreation;

namespace consumable.Controllers
{
    public class InvoiceCreationMobileController : PageController
    {

        private InvoiceCreationRepository invoiceCreationRepo = InvoiceCreationRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Invoice Creation";
            //ViewData["InvoiceCreation"] = (List<InvoiceCreation>)invoiceCreationRepo.GetInvoiceCreation();
        }

        //[HttpGet]
        //public ContentResult GetInvoiceCreationSort(string field, string sort)
        //{
        //    List<String> result = new List<String>();
        //    result = InvoiceCreationRepository.Instance.GetInvoiceCreationSort(field, sort);

        //    return Content(String.Join("", result.ToArray()));
        //}
    }
}
