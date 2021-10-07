using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using consumable.Common.Data;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.GR_IR;
using consumable.Models.Paging;
using consumable.Models.ReceivingList;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class ReceivingListController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private ReceivingListRepository receivingListRepo = ReceivingListRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;
        private SystemPropertyRepository systemPropertyRepo = SystemPropertyRepository.Instance;
        private GR_IRRepository grIrRepo = GR_IRRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Receiving List";

            string statusDefault = CommonConstant.STATUS_GR_RECEIVED;
            ViewData["Status"] = statusDefault;

            string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
            ViewData["vendCodeLogin"] = vendCodeLogin;
            if (vendCodeLogin != null)
            {
                ViewData["readonly"] = "readonly";
                ViewData["disabled"] = "disabled";

                ViewData["visibilityVndr"] = "true";
            }
            else
            {
                ViewData["readonly"] = "";
                ViewData["disabled"] = "";

                ViewData["visibilityVndr"] = "false";
            }

            constructComboBoxes();
            getRecevingList(null, null, vendCodeLogin, null, null, statusDefault,
                CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }

        public ActionResult search(string poNoSearch, string receivingDateSearch,
            string supplierSearch, string poDateSearch, string matDocNoSearch, string statusSearch,
            int page, int size)
        {
            //string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getRecevingList(poNoSearch, receivingDateSearch,
            supplierSearch, poDateSearch, matDocNoSearch, statusSearch,
                page, size);

            return PartialView("_GridView");
        }

        private void getRecevingList(string poNoSearch, string receivingDateSearch,
            string supplierSearch, string poDateSearch, string matDocNoSearch, string statusSearch,
            int page, int size)
        {            
            int countdata = 0;
            countdata = receivingListRepo.countReceivingList(poNoSearch, supplierSearch, poDateSearch, statusSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;           

            ViewData["ReceivingListInquiry"] = receivingListRepo.GetReceivingList(poNoSearch, supplierSearch, poDateSearch, 
                statusSearch, pg.StartData, pg.EndData);
        }

        [HttpGet]
        public ContentResult GetReceivingListSort(string poNoSearch, string supplierSearch, string poDateSearch,
            string statusSearch, int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = receivingListRepo.countReceivingList(poNoSearch, supplierSearch, poDateSearch, statusSearch);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<String> result = new List<String>();
            result = receivingListRepo.GetReceivingListSort(poNoSearch, supplierSearch, poDateSearch,
                statusSearch, pg.StartData, pg.EndData, field, sort);

            return Content(String.Join("", result.ToArray()));
        }

        //public ActionResult SearchMobile()
        //{
        //    ViewData["ReceivingListInquiry"] = receivingListRepo.GetReceivingListInquiry();

        //    return View("~/Views/ReceivingListMobile/ReceivingListMobile.cshtml");
        //}

        public void constructComboBoxes()
        {
            //List<String> statusList = new List<string>();
            List<SystemProperty> listSystemProperty = (List<SystemProperty>)systemPropertyRepo.GetBySystemPropertyType(CommonConstant.SYSTEM_TYPE_STATUS_GR);
            //foreach (SystemProperty sysProp in listSystemProperty)
            //{
            //    statusList.Add(sysProp.SYSTEM_CD);
            //}
            ViewData["StatusList"] = listSystemProperty;

            getLookupSupplierSearch(
                CommonFunction.Instance.DefaultStringValue(),
                CommonFunction.Instance.DefaultPage(),
                CommonFunction.Instance.DefaultSize());
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

        private void getLookupSupplierSearch(string parameter, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier(parameter), page, size);

            ViewData["LookupPaging"] = pg;

            ViewData["Suppliers"] = supplierRepo.GetSupplier(parameter, pg.StartData, pg.EndData);
        }

        public ActionResult GetReceivingListDetail(string poNumber, string poItem, string podate, 
            string vendCode, string grStatus, string invoiceId)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            ViewData["MaterialList"] = receivingListRepo.GetReceivingListDetail(poNumber, poItem, podate, vendCode, grStatus, invoiceId);

            return PartialView("_MatDocDetailPopUp");
        }


        public JsonResult ApproveGR(List<GR_IR_DATA> items)
        {
            AjaxResult results = new AjaxResult();
            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;

                for (int i = 0; i < items.Count; i++)                
                {
                    results = receivingListRepo.ApproveGR(db, items[i].PO_NUMBER, items[i].PO_ITEM,
                        items[i].PO_DATE_STRING, items[i].VEND_CODE, items[i].GR_STATUS, items[i].INVOICE_ID, NoReg);                    
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

        //public JsonResult CancelGR(string matDocNumber, string entrySheetNum,
        //        string matDocYear, string matDocDocdate, string matDocPostDate, string matDocText)
        //{
        //    AjaxResult results = new AjaxResult();

        //    try
        //    {
        //        string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
        //        SAPLogic sap = new SAPLogic();
               
              
        //        CANCEL_GR_TABLE e = new CANCEL_GR_TABLE();
        //        e.INPUT = new List<CANCEL_GR_IN>();
        //        CANCEL_GR_IN gr_in = new CANCEL_GR_IN();
        //        gr_in.MATDOC_NUMBER = matDocNumber;
        //        gr_in.ENTRYSHEET_NUM = entrySheetNum;
        //        gr_in.MATDOC_YEAR = matDocYear;
        //        gr_in.MATDOC_DOCDATE = DateTime.ParseExact(matDocDocdate, "dd.MM.yyyy", null);
        //        gr_in.MATDOC_POSTDATE = DateTime.ParseExact(matDocPostDate, "dd.MM.yyyy", null);
        //        gr_in.MATDOC_TEXT = matDocText;

        //        e.INPUT.Add(gr_in);
        //        e = sap.CancelGR(e);

        //        if (e != null)
        //        {
        //            results = GR_IRRepository.Instance.SaveCancelGR(e, "");
        //        }

              
        //    }
        //    catch (Exception e)
        //    {
        //        results.Result = AjaxResult.VALUE_ERROR;
        //        results.ErrMesgs = new String[] { 
        //                            string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
        //    }
        //    return Json(results);
        //}

        public void DownloadReceivingList(string poNoSearch, string receivingDateSearch,
            string supplierSearch, string poDateSearch, string matDocNoSearch, string statusSearch)
        {
            string fileName = string.Format("ReceivingList{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // Get User Id
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;
            List<GR_IR_DATA> receivingList = receivingListRepo.GetReceivingListExcel(poNoSearch, receivingDateSearch,
            supplierSearch, poDateSearch, matDocNoSearch, statusSearch).ToList();

            byte[] result = receivingListRepo.GenerateDownloadFile(receivingList);
            SendToClientBrowser(fileName, result);
        }

        protected void SendToClientBrowser(string fileName, byte[] hasil)
        {
            Response.Clear();
            //Response.ContentType = result.MimeType;
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Expires = -1;
            Response.Buffer = true;

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", Convert.ToString(hasil.Length));

            Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", fileName));
            Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");

            Response.BinaryWrite(hasil);
            Response.End();
        }


    }
}
