using System;
using System.Collections.Generic;
using System.Web.Mvc;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.Paging;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Models.MaintenanceApproval;
using consumable.Models.SUPPLIER;
using consumable.Models.EmployeeProfile;

namespace consumable.Controllers
{
    public class MaintenanceApprovalController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private MaintenanceApprovalRepository maintenanceApprovalRepo = MaintenanceApprovalRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private EmployeeProfileRepository employeeProfileRepo = EmployeeProfileRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Maintenance Approval";

            getLookupSupplierSearch(
               CommonFunction.Instance.DefaultStringValue(),
               CommonFunction.Instance.DefaultPage(),
               CommonFunction.Instance.DefaultSize());

            getLookupApproverSearch(
               CommonFunction.Instance.DefaultStringValue(),
               CommonFunction.Instance.DefaultPage(),
               CommonFunction.Instance.DefaultSize());

            getListMaintenanceApproval("", "", "", CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }


        public ActionResult search(string supplierCode, string supplierName, string approverUnitCode, int page, int size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListMaintenanceApproval(supplierCode, supplierName, approverUnitCode, page, size);

            return PartialView("_GridView");
        }

        private void getListMaintenanceApproval(string supplierCode, string supplierName, string approverUnitCode, int page, int size)
        {
            Paging pg = new Paging(maintenanceApprovalRepo.CountMaintenanceApproval(supplierCode, supplierName, approverUnitCode), page, size);
            ViewData["Paging"] = pg;
            List<MaintenanceApproval> maintenanceApprovalList = maintenanceApprovalRepo.GetMaintenanceApproval(supplierCode, supplierName, approverUnitCode, pg.StartData, pg.EndData);
            ViewData["maintenanceApprovalList"] = maintenanceApprovalList;
        }


        public ActionResult InitAddMaintenanceApproval()
        {
            MaintenanceApproval item = new MaintenanceApproval();

            ViewData["maintenanceApproval"] = item;

            return PartialView("_MaintenanceApprovalAddPopUp");
        }

        public JsonResult SaveInput(string screenMode, MaintenanceApproval maintenanceApproval)
        {
            IDBContext db = databaseManager.GetContext();

            AjaxResult results = new AjaxResult();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                if (CommonConstant.SCREEN_MODE_ADD.Equals(screenMode))
                {
                    results = maintenanceApprovalRepo.SaveData(db, maintenanceApproval, NoReg);
                }
                else if (CommonConstant.SCREEN_MODE_EDIT.Equals(screenMode))
                {
                    results = maintenanceApprovalRepo.EditData(db, maintenanceApproval, NoReg);
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

        public ActionResult GetByKey(string maintenanceApprovalId)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            MaintenanceApproval maintenanceApproval = maintenanceApprovalRepo.GetMaintenanceApprovalById(maintenanceApprovalId);

            ViewData["maintenanceApproval"] = maintenanceApproval;

            return PartialView("_MaintenanceApprovalAddPopUp");
        }

        public ActionResult DeleteMaintenanceApproval(List<MaintenanceApproval> maintenanceApprovalList)
        {
            IDBContext db = databaseManager.GetContext();

            AjaxResult results = null;

            try
            {
                results = maintenanceApprovalRepo.DeleteData(db, maintenanceApprovalList);

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


        public ActionResult onLookupSupplierSearchAndInput(string Parameter, int Page, string PartialViewSearchAndInput)
        {
            getLookupSupplierSearch(
                Parameter,
                Page,
                CommonFunction.Instance.DefaultSize());
            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupSupplierSearch(string name, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier("", name), page, size);
            ViewData["LookupPaging"] = pg;
            ViewData["Suppliers"] = supplierRepo.GetSupplier("", name, pg.StartData, pg.EndData);
        }


        public ActionResult onLookupApproverSearchAndInput(string Parameter, int Page, string PartialViewSearchAndInput)
        {
            getLookupApproverSearch(
                Parameter,
                Page,
                CommonFunction.Instance.DefaultSize());
            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupApproverSearch(string name, int page, int size)
        {
            Paging pg2 = new Paging(employeeProfileRepo.countEmployeeProfile(name, "LINE HEAD"), page, size);
            ViewData["LookupPaging2"] = pg2;
            ViewData["EmployeeProfiles"] = employeeProfileRepo.GetEmployeeProfile(name, "LINE HEAD", pg2.StartData, pg2.EndData);
        }


    }
}
