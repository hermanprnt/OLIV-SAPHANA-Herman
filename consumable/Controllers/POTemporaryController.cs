using System;
using System.Collections.Generic;
using System.Web.Mvc;
using consumable.Models;
using consumable.Models.Paging;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;
using consumable.Models.Message;
using Toyota.Common.Web.Platform;
using consumable.Models.POTemporary;
using System.Globalization;

namespace consumable.Controllers
{
    public class POTemporaryController : PageController
    {
        private POTemporaryRepository poTemporaryRepo = POTemporaryRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private MessageRepository messageRepo = MessageRepository.Instance;

        public const int DATA_ROW_INDEX_START = 1;//20200707

        protected override void Startup()
        {
            Settings.Title = "LIV - PO Temporary";

            SystemProperty INIT_DATE_RANGE_DEFAULT = systemPropertyRepo.GetSysPropByCodeAndType(
                                "RANGE_DATE_DEFAULT", "SEARCH_CRITERIA_PO_INTERFACE");

            SystemProperty INIT_DATE_MONTH_RANGE = systemPropertyRepo.GetSysPropByCodeAndType(
                                "RANGE_MONTH", "SEARCH_CRITERIA_PO_INTERFACE");

            DateTime dateTime = DateTime.UtcNow.Date;
            var addTheDay = (INIT_DATE_RANGE_DEFAULT == null) ? 30 : INIT_DATE_RANGE_DEFAULT.SYSTEM_VALUE_NUM;
            var amonthpast = dateTime.AddDays(-addTheDay);
            var datenow = dateTime.ToString("dd.MM.yyyy");
            var dateamonthpast = amonthpast.ToString("dd.MM.yyyy");
            string paraminitialpodate = dateamonthpast + " - " + datenow ;
            string paraminitialprocstatus = "Not Process";
            string paraminitialsuppstatus = "Active";

            ViewData["paraminitialpodate"] = paraminitialpodate;
            ViewData["paraminitialprocstatus"] = paraminitialprocstatus;
            ViewData["paraminitialsuppstatus"] = paraminitialsuppstatus;
            ViewData["initialdaterange"] = (INIT_DATE_MONTH_RANGE == null)?12: INIT_DATE_MONTH_RANGE.SYSTEM_VALUE_NUM;

            constructComboBoxes();

            getList(paraminitialpodate, "", "", paraminitialprocstatus, "", paraminitialsuppstatus, CommonFunction.Instance.DefaultPage(), 100);
        }
        private void getLookupSupplierSearch(string parameter, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier(parameter), page, size);

            ViewData["LookupPaging"] = pg;

            ViewData["Suppliers"] = supplierRepo.GetSupplier(parameter, pg.StartData, pg.EndData);
        }
        public ActionResult onLookupSupplier(string Parameter, string PartialViewSearchAndInput, int Page)
        {
            if (PartialViewSearchAndInput.Equals("_LookupTableSupplier"))
            {
                getLookupSupplierSearch(
                    Parameter,
                    Page,
                    CommonFunction.Instance.DefaultSize());
            }

            return PartialView(PartialViewSearchAndInput);
        }
        public void constructComboBoxes()
        {
            //ViewData["Suppliers"] = (List<Supplier>)supplierRepo.GetSupplier("A", 1, 10);
            getLookupSupplierSearch(
                CommonFunction.Instance.DefaultStringValue(),
                CommonFunction.Instance.DefaultPage(),
                CommonFunction.Instance.DefaultSize());
        }

        public ActionResult search(string poDateSearch, string matDocNoSearch, string poNoSearch, string procStatusSearch,
            string supplierSearch, string suppStatusSearch, int page, int size)
        {
            getList(poDateSearch, matDocNoSearch, poNoSearch, procStatusSearch, supplierSearch, suppStatusSearch, page, size);

            return PartialView("_GridView");
        }

        private void getList(string poDateSearch, string matDocNoSearch, string poNoSearch, string procStatusSearch,
            string supplierSearch, string suppStatusSearch, int page, int size)
        {
            int countdata = 0;
            countdata = poTemporaryRepo.CountList(poDateSearch, matDocNoSearch, poNoSearch, procStatusSearch,
            supplierSearch, suppStatusSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            ViewData["DataGrid"] = poTemporaryRepo.GetList(poDateSearch, matDocNoSearch, poNoSearch, procStatusSearch,
            supplierSearch, suppStatusSearch, pg.StartData, pg.EndData);
        }

        public JsonResult processPoTemporary(string keys)
        {
            string UserName = Lookup.Get<Toyota.Common.Credential.User>().FirstName;
            string message = string.Empty;
            List<string> result = poTemporaryRepo.ProcessPoTemporary(UserName, keys);
            foreach (var item in result)
            {
                message += item + ", "; 
            }
            message = message.Substring(0, message.LastIndexOf(','));
            return Json(new { result = message });
        }
        public JsonResult validateDateRangePicker(string poDateSearch)
        {
            AjaxResult ajaxResult = new AjaxResult();
            string message = string.Empty;

            if (String.IsNullOrEmpty(poDateSearch))
            {
                Message msg = messageRepo.GetMessageById("GEUPLD000013");
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                            string.Format(msg.MSG_TEXT)
                        };
                return Json(ajaxResult);
            }

            SystemProperty LIMIT_PODATE_SEARCH = systemPropertyRepo.GetSysPropByCodeAndType(
                                "RANGE_MONTH", "SEARCH_CRITERIA_PO_INTERFACE");

            int _LIMIT = LIMIT_PODATE_SEARCH.SYSTEM_VALUE_NUM;

            string poDateSearchFrom = "";
            string poDateSearchTo = "";
            if (poDateSearch != null && !"".Equals(poDateSearch))
            {
                string[] poDateSearchArray = poDateSearch.Split('-');
                poDateSearchFrom = poDateSearchArray[0].Trim();
                poDateSearchTo = poDateSearchArray[1].Trim();
            }

            DateTime oDateFrom = DateTime.ParseExact(poDateSearchFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime oDateTo = DateTime.ParseExact(poDateSearchTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            var negativeornot = (oDateTo - oDateFrom).TotalDays;
            var diffDate = Math.Abs((oDateFrom.Month - oDateTo.Month) + 12 * (oDateFrom.Year - oDateTo.Year));
            if (negativeornot < 0)
            {
                Message msg = messageRepo.GetMessageById("GEUPLD000014");
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                            string.Format(msg.MSG_TEXT)
                        };

                return Json(ajaxResult);
            }

            if((diffDate+1) > _LIMIT)
            {
                Message msg = messageRepo.GetMessageById("GEUPLD000012");
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new string[] {
                            string.Format(msg.MSG_TEXT, _LIMIT)
                        };

            }

            return Json(ajaxResult);
        }
    }
}
