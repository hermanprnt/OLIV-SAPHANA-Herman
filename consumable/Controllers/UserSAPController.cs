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
using consumable.Models.UserSAP;

namespace consumable.Controllers
{
    public class UserSAPController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private UserSAPRepository userSAPRepo = UserSAPRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "User SAP";

            getListUserSAP(null, null, null, null, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }


        public ActionResult search(string username, string completeName, string noregSearch, string group, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListUserSAP(username, completeName, noregSearch, group, page, size);

            return PartialView("_GridView");
        }

        private void getListUserSAP(string username, string completeName, string noregSearch, string group, int page, int size)
        {
            Paging pg = new Paging(userSAPRepo.countUserSAP(username, completeName, noregSearch, group), page, size);
            ViewData["Paging"] = pg;
            List<UserSAP> userSAPList = userSAPRepo.GetUserSAP(username, completeName, noregSearch, group, pg.StartData, pg.EndData);
            ViewData["userSAPList"] = userSAPList;
        }

        public ActionResult InitAddUserSAP()
        {
            UserSAP userSAP = new UserSAP();

            ViewData["userSAP"] = userSAP;

            return PartialView("_UserSAPAddPopUp");
        }

        public JsonResult SaveInput(string screenMode, UserSAP userSAP)
        {
            IDBContext db = databaseManager.GetContext();

            AjaxResult results = new AjaxResult();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                {
                    results = userSAPRepo.SaveData(db, userSAP, NoReg);
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    results = userSAPRepo.EditData(db, userSAP, NoReg);
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

        public ActionResult GetByKey(string userId)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            UserSAP userSAP = userSAPRepo.GetUserSAPById(userId);

            ViewData["userSAP"] = userSAP;

            return PartialView("_UserSAPAddPopUp");
        }

        public ActionResult DeleteUserSAP(List<UserSAP> userSAPList)
        {
            IDBContext db = databaseManager.GetContext();

            AjaxResult results = null;

            try
            {
                results = userSAPRepo.DeleteData(db, userSAPList);

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
