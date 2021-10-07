using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.PRInquiry;

namespace consumable.Controllers
{
    public class DatagridFixedController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Fixed Datagrid";
            ViewData["PRInquiry"] = PRInquiryRepository.Instance.GetPRInquiry();
        }

        [HttpGet]
        public ContentResult GetPRInquirySort(string field, string sort)
        {
            List<String> result = new List<String>();
            result = PRInquiryRepository.Instance.GetPRInquirySort(field, sort);

            return Content(String.Join("", result.ToArray()));
        }
    }
}
