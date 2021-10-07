using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.RequistionForm;
using consumable.Models.EmployeeProfile;
using consumable.Models.Organization;
using consumable.Models.Paging;
using Toyota.Common.Database;
using consumable.Commons.Constants;
using consumable.Models;
using System.Net;
using System.Text;
using System.IO;
using Toyota.Common.Database;

namespace consumable.Controllers
{
    public class GpsUrlController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "GPS";
            ViewData["GpsUrl"] = "http://localhost:19729/";

        }
    }
}
