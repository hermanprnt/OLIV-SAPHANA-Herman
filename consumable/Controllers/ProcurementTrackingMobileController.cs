using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models;
using consumable.Models.ProcurementTracking;


namespace consumable.Controllers
{
    public class ProcurementTrackingMobileController : PageController
    {

        private ProcurementTrackingRepository procurementTrackingRepo = ProcurementTrackingRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Procurement Tracking";
          //  ViewData["ProcurementTracking"] = (List<ProcurementTracking>)procurementTrackingRepo.GetProcurementTracking(null, 
            //    CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }
    }
}
