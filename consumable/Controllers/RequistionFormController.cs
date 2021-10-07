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
    public class RequistionFormController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private RequistionFormRepository requistionFormRepo = RequistionFormRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Requistion Form";

            constructComboBoxes();

            getLookupUserSearch(
                CommonFunction.Instance.DefaultStringValue(),
                CommonFunction.Instance.DefaultPage(),
                CommonFunction.Instance.DefaultSize());

            ViewData["rf"] = new RequistionForm();

            getListRf(null, null, null, null, null, null, CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }

        public void constructComboBoxes()
        {
            //ViewData["EmployeeProfiles"] = (List<EmployeeProfile>)EmployeeProfileRepository.Instance.GetEmployeeProfile();
            ViewData["Divisions"] = (List<Organization>)OrganizationRepository.Instance.GetDivision();
            
        }

        public ActionResult onLookupUserSearchAndInput(string Parameter, int Page, string PartialViewSearchAndInput)
        {
            getLookupUserSearch(
                Parameter,
                Page,
                CommonFunction.Instance.DefaultSize());
            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupUserSearch(string name, int page, int size)
        {
            Paging pg = new Paging(EmployeeProfileRepository.Instance.countEmployeeProfile(name, ""), page, size);
            ViewData["LookupPaging"] = pg;
            ViewData["EmployeeProfiles"] = EmployeeProfileRepository.Instance.GetEmployeeProfile(name, "", pg.StartData, pg.EndData);
        }

        public ActionResult search(string rfNo, string rfDate, string divisi, string picUser, 
            string wbsNo, string prNo, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListRf(rfNo, rfDate, divisi, picUser, wbsNo, prNo, page, size);

            return PartialView("_GridView");
        }

        private void getListRf(string rfNo, string rfDate, string divisi, string picUser,
            string wbsNo, string prNo, int page, int size)
        {
            Paging pg = new Paging(requistionFormRepo.countRf(rfNo, rfDate, divisi, picUser, wbsNo, prNo),
                page, size);
            ViewData["Paging"] = pg;
            List<RequistionFormView> ListRequistionForm = requistionFormRepo.GetRequistionForm(rfNo,
                rfDate, divisi, picUser, wbsNo, prNo, pg.StartData, pg.EndData);
            ViewData["RequistionForm"] = ListRequistionForm;
        }

        [HttpGet]
        public ContentResult GetRFSort(string rfNo, string picUser, string rfDate, string wbsNo,
            string division, string prNo, int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = requistionFormRepo.countRf(rfNo, rfDate, division, picUser, wbsNo, prNo);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<String> result = new List<String>();
            result = requistionFormRepo.GetRequistionFormSort(rfNo,
                rfDate, division, picUser, wbsNo, prNo, pg.StartData, pg.EndData, field, sort);

            return Content(String.Join("", result.ToArray()));
        }

        public ActionResult InitRf()
        {
            constructComboBoxes();

            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            RequistionForm rf = new RequistionForm();

            ViewData["rf"] = rf;

            return PartialView("_RFAddPopUp");
        }

        public JsonResult SaveInput(string screenMode, RequistionForm rf, RequistionForm dataDelRf)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                {
                    results = requistionFormRepo.SaveData(db, rf, NoReg);
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    results = requistionFormRepo.EditData(db, rf, dataDelRf);
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

        public ActionResult SaveInputRfDtl(RequistionFormDtl json, string screenMode)
        {
            AjaxResult results = new AjaxResult();

            try
            {
                List<RequistionFormDtl> listRequistionFormDtl = new List<RequistionFormDtl>();

                listRequistionFormDtl.Add(json);

                ViewData["rfDtl"] = listRequistionFormDtl;

            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            return Json(results);
        }

        //public JsonResult GetByKey(string rfRegisterId)
        //{
        //    AjaxResult ajaxResult = new AjaxResult();

        //    RequistionForm result = null;

        //    //IDBContext db = DatabaseManager.Instance.GetContext();

        //    try
        //    {
        //        result = requistionFormRepo.GetByRfRegisterId(rfRegisterId);

        //        if (result == null)
        //        {
        //            ajaxResult.Result = AjaxResult.VALUE_ERROR;
        //            ajaxResult.ErrMesgs = new string[] { 
        //                string.Format("No Data with the selected key found, please refresh the screen first")
        //            };

        //            return Json(ajaxResult);
        //        }

        //        ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
        //        ajaxResult.Params = new object[] {
        //            result
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        ajaxResult.Result = AjaxResult.VALUE_ERROR;
        //        ajaxResult.ErrMesgs = new string[] { 
        //            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)
        //        };
        //    }
        //    //finally
        //    //{
        //    //    db.Close();
        //    //}

        //    return Json(ajaxResult);
        //}

        public ActionResult GetByKey(string rfRegisterId)
        {
            constructComboBoxes();

            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            RequistionForm rf = requistionFormRepo.GetByRfRegisterId(rfRegisterId);
            IList<RequistionFormDtl> listRfDtl = requistionFormRepo.getRfDtlByRegisterId(rfRegisterId);
            rf.listRequistionFormDtl = listRfDtl;

            ViewData["rf"] = rf;

            return PartialView("_RFAddPopUp");
        }

        public ActionResult DeleteRf(List<RequistionForm> rfList)
        {
            string message = "";
            //int results = 0;
            AjaxResult results = null;

            try
            {
                //string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                results = requistionFormRepo.DeleteData(rfList);
                //message = "Success";
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            return Json(results);
        }
    }
}
