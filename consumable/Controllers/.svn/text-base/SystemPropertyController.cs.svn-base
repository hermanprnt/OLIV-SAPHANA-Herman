using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using consumable.Models.Paging;
using Toyota.Common.Database;
using consumable.Commons.Constants;
using consumable.Models;
using System.Net;
using System.Text;
using System.IO;
using Toyota.Common.Web.Platform;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;

namespace consumable.Controllers
{
    public class SystemPropertyController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "System Property";

            getListSystemProperty(null, null, null, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }


        public ActionResult search(string systemPropertyCode, string systemPropertyType, string systemPropertyValueText, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListSystemProperty(systemPropertyCode, systemPropertyType, systemPropertyValueText, page, size);

            return PartialView("_GridView");
        }

        private void getListSystemProperty(string systemPropertyCode, string systemPropertyType, string systemPropertyValueText, int page, int size)
        {
            Paging pg = new Paging(systemPropertyRepo.countSystemProperty(systemPropertyCode, systemPropertyType, systemPropertyValueText), page, size);
            ViewData["Paging"] = pg;
            List<SystemProperty> systemPropertyList = systemPropertyRepo.GetSystemProperty(systemPropertyCode, systemPropertyType, systemPropertyValueText, pg.StartData, pg.EndData);
            ViewData["systemPropertyList"] = systemPropertyList;
        }

        public ActionResult InitAddSystemProperty()
        {
            SystemProperty systemProperty = new SystemProperty();

            ViewData["systemProperty"] = systemProperty;

            return PartialView("_SystemPropertyAddPopUp");
        }

        public JsonResult SaveInput(string screenMode, SystemProperty systemProperty)
        {
            IDBContext db = databaseManager.GetContext();

            AjaxResult results = new AjaxResult();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                {
                    results = systemPropertyRepo.SaveData(db, systemProperty, NoReg);
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    results = systemPropertyRepo.EditData(db, systemProperty, NoReg);
                }

                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else
                {
                    db.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            finally
            {
                db.Close();
            }

            return Json(results);
        }

        public ActionResult GetByKey(string systemPropertyId)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            SystemProperty systemProperty = systemPropertyRepo.GetBySystemPropertyId(systemPropertyId);

            ViewData["systemProperty"] = systemProperty;

            return PartialView("_SystemPropertyAddPopUp");
        }

        public ActionResult DeleteSystemProperty(List<SystemProperty> systemPropertyList)
        {
            IDBContext db = databaseManager.GetContext();

            AjaxResult results = null;

            try
            {
                results = systemPropertyRepo.DeleteData(db, systemPropertyList);

                if (AjaxResult.VALUE_ERROR.Equals(results.Result))
                {
                    db.AbortTransaction();
                }
                else
                {
                    db.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            finally
            {
                db.Close();
            }

            return Json(results);
        }
    }
}
