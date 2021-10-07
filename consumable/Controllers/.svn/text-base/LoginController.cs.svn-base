using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using System.Web.Security;
using consumable.Commons;

namespace consumable.Controllers
{
    public class LoginController : LoginPageController
    {
        //
        // GET: /Login/
        protected override void Startup()
        {
            Settings.Title = "Masuk";
        }

        public ActionResult Ldap(string username, string password)
        {
            ILoginValidator lo = LDAPValidator.Instance;
            bool ret = lo.ValidateUser(username, password);
            if (ret)
            {
                FormsAuthentication.SetAuthCookie(username, false);
                // get, and save user info to session variables 
            }
            return Content(ret.ToString());
        }
    }
}
