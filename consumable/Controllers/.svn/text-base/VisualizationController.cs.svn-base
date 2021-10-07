using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class VisualizationController : PageController
    {
        protected override void Startup()
        {
            //ApplicationSettings.Instance.Menu.Enabled = false;
            Settings.Title = "Visualization";
        }

        public ActionResult SearchData()
        {
            return PartialView("_GridView");
        }
    }
}
