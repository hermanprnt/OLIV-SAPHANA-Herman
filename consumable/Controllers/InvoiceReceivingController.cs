using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using consumable.Models.InvoiceReceiving;
using consumable.Models;
using consumable.Models.Paging;
using Toyota.Common.Database;

namespace consumable.Controllers
{
    public class InvoiceReceivingController : PageController
    {
        private DatabaseManager databaseManager = DatabaseManager.Instance;
        private InvoiceReceivingRepository invoiceReceivingRepo = InvoiceReceivingRepository.Instance;
        
        public IEnumerable<InvoiceReceiving> tmpIR { set; get; }
        
        protected override void Startup()
        {
            Settings.Title = "Invoice Receiving";
            getInvoiceReceving(CommonFunction.Instance.DefaultPage(), CommonFunction.Instance.DefaultSize());

        }

        public ActionResult search(int page, int size)
        {
            getInvoiceReceving(page, size); ;

            return PartialView("_GridView");
        }


        private void getInvoiceReceving(int page, int size)
        {
            int countdata = 0;
            countdata = invoiceReceivingRepo.countInvoiceReceiving();

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            ViewData["InvoiceReceiving"] = invoiceReceivingRepo.GetInvoiceReceiving(pg.StartData, pg.EndData);
        }


        public JsonResult findInvoiceReceiving(string certificateId)
        {
            AjaxResult results = new AjaxResult();
            if (!"".Equals(certificateId.Trim()))
            {
                InvoiceReceiving itemResult = invoiceReceivingRepo.findInvoiceReceiving(certificateId);
                
                if (itemResult == null)
                {
                    results.Result = AjaxResult.VALUE_ERROR;
                    results.ErrMesgs = new String[] { "Certificate ID tidak ditemukan atau sudah diproses" };
                }
                else
                {
                    results.Result = AjaxResult.VALUE_SUCCESS;
                    results.Params = new object[] { itemResult.SUPPLIER_CD + " - "+ itemResult.SUPPLIER_NAME,
                    itemResult.INVOICE_NO, itemResult.TOTAL_AMOUNT,
                    itemResult.TAX_INVOICE_NO, itemResult.TAX_INVOICE_AMOUNT };
                }
            }
            else
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { "Certificate ID tidak ditemukan" };
            }

            return Json(results);
        }

        public JsonResult process(string certificateId, string processType, string noticeDate, string noticeRemark)
        {
            AjaxResult results = new AjaxResult();

            IDBContext db = databaseManager.GetContext();

            try
            {
                db.BeginTransaction();

                string NoReg = Lookup.Get<Toyota.Common.Credential.User>().RegistrationNumber;
                results = invoiceReceivingRepo.process(db, certificateId, processType, noticeDate, noticeRemark, NoReg);

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

        [HttpGet]
        public ContentResult GetInvoiceReceivingSort(int page, int size, string field, string sort)
        {
            int countdata = 0;
            countdata = invoiceReceivingRepo.countInvoiceReceiving();

            Paging pg = new Paging(countdata, page, size);
            ViewData["CountData"] = countdata;
            ViewData["Paging"] = pg;

            invoiceReceivingRepo.GetInvoiceReceiving(pg.StartData, pg.EndData);

            List<String> result = new List<String>();
            result = invoiceReceivingRepo.GetInvoiceReceivingSort(pg.StartData, pg.EndData, field, sort);

            return Content(String.Join("", result.ToArray()));
        }

        public void DownloadInvoiceReceiving()
        {
            string fileName = string.Format("InvoiceReceiving{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // Get User Id
            //string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;
            List<InvoiceReceiving> invoiceReceivingList = invoiceReceivingRepo.GetInvoiceReceivingListExcel();

            byte[] result = invoiceReceivingRepo.GenerateDownloadFile(invoiceReceivingList);
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
