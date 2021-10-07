using System;
using System.Collections.Generic;
using System.Web.Mvc;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.Paging;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Models.BaselineDate;

namespace consumable.Controllers
{
    public class BaselineDateController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private BaselineDateRepository baselineDateRepo = BaselineDateRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Baseline Date";
            getListBasedate(null, null, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }

        private void getListBasedate(string holidayDateStr, string baselineDateStr, int page, int size)
        {
            Paging pg = new Paging(baselineDateRepo.countBaselineDate(holidayDateStr, baselineDateStr), page, size);
            ViewData["Paging"] = pg;
            List<BaselineDate> glAccountList = baselineDateRepo.GetBaselineDate(holidayDateStr, baselineDateStr, pg.StartData, pg.EndData);
            ViewData["BaselineDateList"] = glAccountList;
        }

        public ActionResult search(string holidayDate, string baselineDate, int page, int size)
        {
            getListBasedate(holidayDate, baselineDate, page, size);

            return PartialView("_GridView");
        }

        public JsonResult SaveInput(string screenMode, BaselineDate baselineDate)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                {
                    results = baselineDateRepo.SaveData(db, baselineDate, NoReg);
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    results = baselineDateRepo.EditData(db, baselineDate, NoReg);
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

        public ActionResult GetByKey(string holidayDate)
        {
            BaselineDate baselineDate = baselineDateRepo.GetByHolidayDate(holidayDate);

            ViewData["baselineDate"] = baselineDate;

            return PartialView("_BaselineDateAddPopUp");
        }

        public ActionResult DeleteBaselineDate(List<BaselineDate> baselineDate)
        {
            IDBContext db = databaseManager.GetContext();
            AjaxResult results = null;

            try
            {
                results = baselineDateRepo.DeleteData(db, baselineDate);

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
