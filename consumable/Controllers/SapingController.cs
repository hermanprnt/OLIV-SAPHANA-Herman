using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using consumable.Common.Data;
//using consumable.Logic;
using consumable.Models;
using consumable.Models.GR_IR;
using Toyota.Common.Web.Platform;

namespace consumable.Controllers
{
    public class SapingController : PageController
    {
        private GR_IRRepository grIrRepo = GR_IRRepository.Instance;

        protected override void Startup()
        {
            Settings.Title = "SAPING";
            getGrNotif("");
           
        }
        
        public ActionResult Upload(HttpPostedFileBase file)
        {
            Stream tes = file.InputStream;
            AjaxResult ajaxResult = new AjaxResult();

            var fileStream = System.IO.File.Create("D:\\backup\\consumable\\"+ DateTime.Now.ToString("yyyyMMdd") + ".zip");
            tes.Seek(0, SeekOrigin.Begin);
            tes.CopyTo(fileStream);
            fileStream.Close();
            
            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
            ajaxResult.Params = new string[] { "Sukses" };

            return Json(ajaxResult);
        }

        public JsonResult saping(string matDocNoFrom, string docDateFrom,
            string dropDown1, string matDocNoTo, string docDateTo, string dropDown2)
        {
            AjaxResult results = new AjaxResult();
            string NoReg = Lookup.Get<Toyota.Common.Credential.User>().Id;

            try
            {
                //results = sapingGrIr(matDocNoFrom, docDateFrom, dropDown1, matDocNoTo, docDateTo, dropDown2);
                //results = sapingPO();
            }
            catch (Exception e)
            {
                results.Result = AjaxResult.VALUE_ERROR;
                results.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", e.GetType().FullName, e.Message)};
            }
            return Json(results);
        }

        //public AjaxResult sapingGrIr(string matDocNoFrom, string docDateFrom,
        //    string dropDown1, string matDocNoTo, string docDateTo, string dropDown2)
        //{
        //    AjaxResult results = new AjaxResult();

        //    try
        //    {
        //        SAPLogic sap = new SAPLogic();
        //        GR_IR_TABLE e = new GR_IR_TABLE();

        //        e.DOC_NO = new List<GR_IR_DOC_NO>();// {LOW = "5000059225", HIGH = "5000059225" };
        //        GR_IR_DOC_NO dn = new GR_IR_DOC_NO();
        //        dn.SIGN = "I";
        //        dn.OPTION = dropDown1;
        //        //dn.LOW = "5000059225";
        //        //dn.HIGH = "5000059225";
        //        dn.LOW = matDocNoFrom;
        //        dn.HIGH = matDocNoTo;

        //        e.DOC_DATE = new List<GR_IR_DOC_DATE>();// {LOW = "5000059225", HIGH = "5000059225" };
        //        GR_IR_DOC_DATE dc = new GR_IR_DOC_DATE();
        //        dc.SIGN = "I";
        //        dc.OPTION = dropDown2;
        //        dc.LOW = DateTime.ParseExact(docDateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //        dc.HIGH = DateTime.ParseExact(docDateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);

        //        e.DOC_NO.Add(dn);
        //        e.DOC_DATE.Add(dc);
        //        results = sap.GetGR(e);

        //        //ViewData["GR_IR_DATA"] = e.DATA;
        //    }
        //    catch (Exception ex)
        //    {
        //        results.Result = AjaxResult.VALUE_ERROR;
        //        results.ErrMesgs = new String[] { 
        //                            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)};
        //    }

        //    return results;
        //}



        //public AjaxResult sapingPO()
        //{
        //    AjaxResult results = new AjaxResult();

        //    try
        //    {
        //        SAPLogic sap = new SAPLogic();
        //        PO_TABLE e = new PO_TABLE();
        //        PO_TABLE entity = new PO_TABLE();

        //        e.PO_NO = new List<SEND_PO_NO>();// {LOW = "5000059225", HIGH = "5000059225" };
        //        SEND_PO_NO poNo = new SEND_PO_NO();
        //        poNo.SIGN = "I";
        //        poNo.OPTION = "EQ";
        //        //dn.LOW = "5000059225";
        //        //dn.HIGH = "5000059225";
        //        poNo.LOW = "";
        //        poNo.HIGH = "";

        //        e.PO_DATE = new List<SEND_PO_DATE>();// {LOW = "5000059225", HIGH = "5000059225" };
        //        SEND_PO_DATE poDate = new SEND_PO_DATE();
        //        poDate.SIGN = "I";
        //        poDate.OPTION = "BT";
        //        poDate.LOW = DateTime.ParseExact("24/06/2015", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        poDate.HIGH = DateTime.ParseExact("24/10/2015", "dd/MM/yyyy", CultureInfo.InvariantCulture); ;

        //        e.PO_NO.Add(poNo);
        //        e.PO_DATE.Add(poDate);
        //        results = sap.GetPO(e);
        //    }
        //    catch (Exception ex)
        //    {
        //        results.Result = AjaxResult.VALUE_ERROR;
        //        results.ErrMesgs = new String[] { 
        //                            string.Format("{0} = {1}", ex.GetType().FullName, ex.Message)};
        //    }

        //    return results;
        //}



        private void getGrNotif(string vendCode)
        {
            ViewData["GR_IR_DATA"] = (List<GR_IR_DATA>)grIrRepo.GetGR_IR_Notif("");
        }
    }
}
