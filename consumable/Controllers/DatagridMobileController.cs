﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class DatagridMobileController : PageController
    {
        protected override void Startup()
        {
            Settings.Title = "Datagrid";
        }
    }
}
