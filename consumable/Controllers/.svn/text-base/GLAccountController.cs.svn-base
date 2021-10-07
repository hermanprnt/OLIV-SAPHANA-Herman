using System;
using System.Collections.Generic;
using System.Web.Mvc;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.Paging;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Models.GLAccount;

namespace consumable.Controllers
{
    public class GLAccountController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private GLAccountRepository glAccountRepo = GLAccountRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "GL Account";
            constructComboBoxes();
            getListGL(null, null, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }

        public void constructComboBoxes()
        {
            List<SystemProperty> listTermPaym = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.TERM_PAY);
            ViewData["TermPaymList"] = listTermPaym;

            
        }

        public ActionResult search(string glAccountNo, string glAccountName, int page, int size)
        {
            getListGL(glAccountNo, glAccountName, page, size);

            return PartialView("_GridView");
        }

        private void getListGL(string glAccountNo, string glAccountName, int page, int size)
        {
            Paging pg = new Paging(glAccountRepo.countGLAccount(glAccountNo, glAccountName), page, size);
            ViewData["Paging"] = pg;
            List<GLAccount> glAccountList = glAccountRepo.GetGLAccount(glAccountNo, glAccountName, pg.StartData, pg.EndData);
            ViewData["GLAccountList"] = glAccountList;
        }

        public ActionResult InitAddGL()
        {
            constructComboBoxes();
            GLAccount item = new GLAccount();

            ViewData["glAccount"] = item;

            return PartialView("_GLAccountAddPopUp");
        }

        public JsonResult SaveInput(string screenMode, GLAccount glAccount)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                {
                    results = glAccountRepo.SaveData(db, glAccount, NoReg);
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    results = glAccountRepo.EditData(db, glAccount, NoReg);
                }
                //message = "Success";

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

        public ActionResult GetByKey(string glAccountId)
        {
            constructComboBoxes();

            GLAccount glAccount = glAccountRepo.GetByGLAccountId(glAccountId);

            ViewData["glAccount"] = glAccount;

            return PartialView("_GLAccountAddPopUp");
        }

        public ActionResult DeleteGLAccount(List<GLAccount> glAccountList)
        {
            //string message = "";
            //int results = 0;
            IDBContext db = databaseManager.GetContext();
            AjaxResult results = null;

            try
            {
                results = glAccountRepo.DeleteData(db, glAccountList);
                //message = "Success";

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
