using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models;
using consumable.Models.Paging;
using consumable.Models.ProcurementTracking;
using consumable.Models.SUPPLIER;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using Toyota.Common.Database;
namespace consumable.Controllers
{
    public class ProcurementTrackingController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private ProcurementTrackingRepository procurementTrackingRepo = ProcurementTrackingRepository.Instance;
        private SupplierRepository supplierRepo = SupplierRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "Procurement Tracking";

            string vendCodeLogin = CommonFunction.Instance.getVendCodeLogin(Lookup.Get<Toyota.Common.Credential.User>());
            ViewData["vendCodeLogin"] = vendCodeLogin;
            if (vendCodeLogin != null)
            {
                ViewData["readonly"] = "readonly";
                ViewData["disabled"] = "disabled";
            }
            else
            {
                ViewData["readonly"] = "";
                ViewData["disabled"] = "";
            }

            constructComboBoxes();

            getListProcurement(null, vendCodeLogin, null, null, null, null, null, null, null, null,
                CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());
        }

        public void constructComboBoxes()
        {
            //ViewData["Suppliers"] = (List<Supplier>)supplierRepo.GetSupplier("A", 1, 10);
            getLookupSupplierSearch(
                CommonFunction.Instance.DefaultStringValue(),
                CommonFunction.Instance.DefaultPage(),
                CommonFunction.Instance.DefaultSize());
            getLookupSupplierInputInvoice(
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
            else
            {
                getLookupSupplierInputInvoice(
                    Parameter,
                    Page,
                    CommonFunction.Instance.DefaultSize());
            }

            //ViewData["Suppliers"] = (List<Supplier>)supplierRepo.GetSupplier(Parameter, page ?? CommonFunction.Instance.DefaultPage(), 
            //    CommonFunction.Instance.DefaultSize());

            return PartialView(PartialViewSearchAndInput);
        }

        private void getLookupSupplierSearch(string parameter, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier(parameter), page, size);
            
            ViewData["LookupPaging"] = pg;

            ViewData["Suppliers"] = supplierRepo.GetSupplier(parameter, pg.StartData, pg.EndData);
        }

        private void getLookupSupplierInputInvoice(string parameter, int page, int size)
        {
            Paging pg = new Paging(supplierRepo.countSupplier(parameter), page, size);

            ViewData["LookupPagingInput"] = pg;

            ViewData["SuppliersInputInvoice"] = supplierRepo.GetSupplier(parameter, pg.StartData, pg.EndData);
        }

        public ActionResult search(string receivedDate, string supplier, string sapDocNo, string planPaymentDate,
            string verifiedStatus, string paymentStatus, string taxInvoiceNo,
            string invoiceNo, string actualPaymentDate, string typeOfTransaction, 
            int? page, int? size)
        {
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            getListProcurement(receivedDate, supplier, sapDocNo, planPaymentDate, verifiedStatus, paymentStatus,
                taxInvoiceNo, invoiceNo, actualPaymentDate, typeOfTransaction, page ?? CommonFunction.Instance.DefaultPage(), 
                size ?? CommonFunction.Instance.DefaultSize());

            return PartialView("_GridView");
        }

        private void getListProcurement(string receivedDate, string supplier, string sapDocNo, string planPaymentDate,
            string verifiedStatus, string paymentStatus, string taxInvoiceNo,
            string invoiceNo, string actualPaymentDate, string typeOfTransaction, 
            int page, int size)
        {
            int countdata = 0;
            countdata = procurementTrackingRepo.countProcurement(receivedDate, supplier, sapDocNo, planPaymentDate, verifiedStatus,
                paymentStatus, taxInvoiceNo, invoiceNo, actualPaymentDate, typeOfTransaction);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<ProcurementTracking> procurementTrackingList = procurementTrackingRepo.GetProcurementTracking(receivedDate,
                supplier, sapDocNo, planPaymentDate, verifiedStatus, paymentStatus, taxInvoiceNo, 
                invoiceNo, actualPaymentDate, typeOfTransaction,
                pg.StartData, pg.EndData);
            ViewData["ProcurementTracking"] = procurementTrackingList;
        }

        [HttpGet]
        public ContentResult GetProcurementTrackingSort(string receivedDate, string supplier, string sapDocNo,
            string typeOfTransaction, string planPaymentDate, string verifiedStatus, string paymentStatus,
            string taxInvoiceNo, string invoiceNo, string actualPaymentDate,
            int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = procurementTrackingRepo.countProcurement(receivedDate, supplier, sapDocNo, planPaymentDate, verifiedStatus,
                paymentStatus, taxInvoiceNo, invoiceNo, actualPaymentDate, typeOfTransaction);

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            List<String> result = new List<String>();
            result = procurementTrackingRepo.GetProcurementTrackingSort(receivedDate, supplier, sapDocNo, planPaymentDate, verifiedStatus,
                paymentStatus, taxInvoiceNo, invoiceNo, actualPaymentDate, typeOfTransaction, pg.StartData, pg.EndData, field, sort);

            return Content(String.Join("", result.ToArray()));
        }

        public JsonResult SaveInputOtherInvoice(ProcurementTracking procTrack)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                results = procurementTrackingRepo.SaveDataInputOtherInvoice(db, procTrack, NoReg);
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

        public ActionResult DeleteProcTrack(List<ProcurementTracking> procTrackList)
        {
            //string message = "";
            //int results = 0;
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                results = procurementTrackingRepo.DeleteData(db, procTrackList);
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

        public ActionResult SubmitNotice(List<ProcurementTracking> procTrackList)
        {
            //string message = "";
            //int results = 0;
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                results = procurementTrackingRepo.SubmitNotice(db, procTrackList);
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

        public void DownloadProcurementTracking(string receivedDate, string supplier, 
            string sapDocNo, string planPaymentDate, string verifiedStatus, string paymentStatus,
            string taxInvoiceNo, string invoiceNo, string actualPaymentDate, string typeOfTransaction)
        {
            string fileName = string.Format("ProcurementTracking{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // Get User Id
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;
            List<ProcurementTracking> procurementTrackingList = procurementTrackingRepo
                .GetProcurementTrackingExcel(receivedDate,
                supplier, sapDocNo, planPaymentDate, verifiedStatus, paymentStatus,
                taxInvoiceNo, invoiceNo, actualPaymentDate, typeOfTransaction).ToList();

            byte[] result = procurementTrackingRepo.GenerateDownloadFile(procurementTrackingList);
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
