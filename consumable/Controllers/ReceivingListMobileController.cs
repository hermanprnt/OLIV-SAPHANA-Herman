using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.ReceivingList;

namespace consumable.Controllers
{
    public class ReceivingListMobileController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Receiving List";
            //ViewData["ReceivingListInquiry"] = ReceivingListRepository.Instance.GetReceivingListInquiry();
        }
    }
}
