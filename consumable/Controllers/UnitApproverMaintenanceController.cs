using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.EmployeeProfile;
using consumable.Models.Paging;
using consumable.Models.PLANT;
using consumable.Models.SLOC;
using consumable.Models.UnitApproverMaintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class UnitApproverMaintenanceController : PageController
    {
        private UnitApproverMaintenanceRepository UnitApproverMaintenanceRepository = UnitApproverMaintenanceRepository.Instance;
        private PlantRepository PlantRepository = PlantRepository.Instance;
        private SlocRepository SlocRepository = SlocRepository.Instance;
        private EmployeeProfileRepository EmployeeProfileRepository = EmployeeProfileRepository.Instance;
        
        protected override void Startup()
        {
            Settings.Title = "Unit Approver Maintenance";

            #region Searching
            GetDataUnitApproverMaintenance("", "", "", CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            #endregion

            #region Lookup
            getLookupPlantSearchAndInput(CommonFunction.Instance.DefaultStringValue(), CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            getLookupSlocByPlantSearchAndInput(CommonFunction.Instance.DefaultStringValue(), CommonFunction.Instance.DefaultStringValue(), CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            getLookupEmployeeProfileSearchAndInput(CommonFunction.Instance.DefaultStringValue(), CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
            #endregion
        }

        #region Searching
        public ActionResult Search(String PlantCD, String SlocCD, String PIC, int Page, int Size)
        {
            GetDataUnitApproverMaintenance(PlantCD, SlocCD, PIC, Page, Size);

            return PartialView("UnitApproverMaintenanceGridView");
        }

        private void GetDataUnitApproverMaintenance(String PlantCD, String SlocCD, String PIC, int Page, int Size)
        {
            #region Paging
            Paging pg = new Paging(UnitApproverMaintenanceRepository.GetDataCountUnitApproveMaintenance(PlantCD, SlocCD, PIC), Page, Size);
            ViewData["Paging"] = pg;
            #endregion

            #region Get Data
            List<UnitApproverMaintenance> UnitApproverMaintenanceList = UnitApproverMaintenanceRepository.GetDataListUnitApproveMaintenance(PlantCD, SlocCD, PIC, pg.StartData, pg.EndData);
            ViewData["UnitApproverMaintenanceList"] = UnitApproverMaintenanceList;
            #endregion
        }
        #endregion

        #region Save Data
        public JsonResult SaveData(string screenMode, UnitApproverMaintenance unitApproverMaintenance)
        {
            AjaxResult results = new AjaxResult();
            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                string Username = Lookup.Get<Toyota.Common.Credential.User>().Username;

                List<UnitApproverMaintenance> ListModel = new List<UnitApproverMaintenance>();
                if (screenMode.Equals("add"))
                {
                    ListModel.Add(unitApproverMaintenance);
                }
                else if (screenMode.Equals("delete"))
                {
                    var listData = unitApproverMaintenance.KEY_UPDATE_DATA.Split(';');
                    foreach (var item in listData)
                    {
                        var listModel = item.Split('|');
                        UnitApproverMaintenance model = new UnitApproverMaintenance();
                        model.PLANT_CD = listModel[0];
                        model.SLOC_CD = listModel[1];
                        model.PIC = listModel[2];
                        model.CHANGED_BY = listModel[3];
                        model.CHANGED_DT = listModel[4];
                        ListModel.Add(model);
                    }
                }

                String result = UnitApproverMaintenanceRepository.SaveData(screenMode, ListModel, Username);
                if (result.Split('|')[0].Equals("E"))
                {
                    results.Result = AjaxResult.VALUE_ERROR;
                    results.ErrMesgs = new String[] { string.Format("{0}", result.Split('|')[1]) };
                }
                else
                {
                    results.Result = AjaxResult.VALUE_SUCCESS;
                    results.ErrMesgs = new String[] { string.Format("{0}", result.Split('|')[1]) };
                }
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { string.Format("{0} = {1}", e.GetType().FullName, e.Message) };
            }
            return Json(results);
        }
        #endregion

        #region Lookup
        public ActionResult onLookupPlantSearchAndInput(string Parameter, int Page, string PartialViewSearchAndInput)
        {
            getLookupPlantSearchAndInput(Parameter, Page, CommonFunction.Instance.DefaultSize());
            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupPlantSearchAndInput(string parameter, int page, int size)
        {
            #region Paging
            Paging pg = new Paging(PlantRepository.GetDataCountPlant(parameter), page, size);
            ViewData["LookupPaging"] = pg;
            #endregion

            #region Get Data
            ViewData["Plant"] = PlantRepository.GetDataListPlant(parameter, pg.StartData, pg.EndData);
            #endregion
        }

        public ActionResult onLookupSlocByPlantSearchAndInput(string PlantCD, string Parameter, int Page, string PartialViewSearchAndInput)
        {
            getLookupSlocByPlantSearchAndInput(PlantCD, Parameter, Page, CommonFunction.Instance.DefaultSize());
            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupSlocByPlantSearchAndInput(string plantCD, string parameter, int page, int size)
        {
            #region Paging
            Paging pg = new Paging(SlocRepository.GetDataCountSlocByPlant(plantCD, parameter), page, size);
            ViewData["LookupPaging"] = pg;
            #endregion

            #region Get Data
            ViewData["Sloc"] = SlocRepository.GetDataListSlocByPlant(plantCD, parameter, pg.StartData, pg.EndData);
            #endregion
        }

        public ActionResult onLookupEmployeeProfileSearchAndInput(string Parameter, int Page, string PartialViewSearchAndInput)
        {
            getLookupEmployeeProfileSearchAndInput(Parameter, Page, CommonFunction.Instance.DefaultSize());
            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupEmployeeProfileSearchAndInput(string parameter, int page, int size)
        {
            #region Paging
            Paging pg = new Paging(EmployeeProfileRepository.GetDataCountEmployeeProfile(parameter), page, size);
            ViewData["LookupPaging"] = pg;
            #endregion

            #region Get Data
            ViewData["EmployeeProfile"] = EmployeeProfileRepository.GetDataListEmployeeProfile(parameter, pg.StartData, pg.EndData);
            #endregion
        }
        #endregion
    }
}
