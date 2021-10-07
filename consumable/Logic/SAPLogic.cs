using System;
using System.Text;
using consumable.Common;
using consumable.Common.Data;
using consumable.Commons.Constants;
using consumable.Models;
using consumable.Models.GR_IR;
using consumable.Models.InvoiceInquiry;
using consumable.Models.PO_SAP;
using SAP.Middleware.Connector;


namespace consumable.Logic
{
    public class SAPLogic
    {
        RfcDestinationManager.ConfigurationChangeHandler changeHandler;
        private InvoiceInquiryRepository invoiceInquiryRepo = InvoiceInquiryRepository.Instance;

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged
        {
            add
            {
                changeHandler = value;
            }
            remove
            {
                //do nothing
            }
        }
        public void removeDestination()
        {
            changeHandler("SAPNCO", new RfcConfigurationEventArgs(RfcConfigParameters.EventType.DELETED));
        }
        private IPut log = Sing.Me.Log;
        private StringBuilder bi, bo;

        private RfcDestination SapRfcDestination;
        private RfcRepository SapRfcRepository;
        //private RfcSessionManager SapSession;
        // private CommonFunction.Num2Str decFmt = CommonFunction.PostCurr;
        private IDestinationConfiguration _config;

        // /protected readonly GlobalResourceData _globalResource = new GlobalResourceData();

        public SAPLogic()
        {
            SapRfcDestination = RfcDestinationManager.GetDestination(AppSetting.SAP_NAME);
            /// SapRfcDestination = RfcDestinationManager.GetDestination("SAPNCO");
            SapRfcRepository = SapRfcDestination.Repository;

            //errs = new List<string>();
        }

        public SAPLogic(string _SAPUser, string _SAPPassword)
        {

            //errs = new List<string>();

            _config = new SAPConfigChange(_SAPUser, _SAPPassword);
            try
            {
                RfcDestinationManager.RegisterDestinationConfiguration(_config);
                SapRfcDestination = RfcDestinationManager.GetDestination("SAPNCO");
                RfcSessionManager.BeginContext(SapRfcDestination);
                SapRfcRepository = SapRfcDestination.Repository;
            }
            catch (RfcInvalidStateException e)
            {
                RfcDestinationManager.UnregisterDestinationConfiguration(
                    _config);
                throw new Exception("Invalid State " + e.Message);
            }
            catch (RfcCommunicationException e)
            {
                // network problem...
                RfcDestinationManager.UnregisterDestinationConfiguration(
                    _config);
                throw new Exception("Network Problem " + e.Message);
            }
            catch (RfcLogonException e)
            {
                // user could not logon...
                RfcDestinationManager.UnregisterDestinationConfiguration(
                    _config);
                throw new Exception("Bad Username or password " + e.Message);
            }
            catch (RfcAbapBaseException e)
            {
                // The function module returned an ABAP exception, 
                // an ABAP message or an ABAP class-based exception..
                RfcDestinationManager.UnregisterDestinationConfiguration(
                    _config);
                throw new
                    Exception("The function module returned an ABAP exception "
                        + e.Message);
            }
        }

        public void init()
        {
            bi = new StringBuilder(""); bo = new StringBuilder("");
            RfcSessionManager.BeginContext(SapRfcDestination);
            //errs.Clear();
        }

        public void del()
        {
            bi.Clear();
            bo.Clear();
            RfcSessionManager.EndContext(SapRfcDestination);
        }

        private void unreg()
        {
            if (_config == null) return;
            try
            {
                RfcDestinationManager.UnregisterDestinationConfiguration(
                    _config);
            }
            catch (Exception ex)
            {
                ex.Err();
            }
        }

        public bool ToText { get; set; }

        private string UID = "SAP";

        public AjaxResult GetGR(GR_IR_TABLE e)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string fn = "ZBAPI_SEND_GRIR";
            string loc = string.Format("GetGR()");

            //string[] inputs = new string[6];

            string[,] nt = new string[4, 2] { 
                {"MATDOC_NUMBER", "ZST_SELOPT_DOCNO"},
                {"MATDOC_DATE", "ZST_SELOPT_DATE"},
                {"GRIR_DATA", "ZST_SENDGR"},
                {"RETURN", "ZHR_UPD_PROFILE_FAMILY"}};

            //for (int iNT = 0; iNT < 4; iNT++)
            //{
            //    inputs[iNT] = nt[iNT, 0];
            //}

            LogHelper lo = new LogHelper(log, loc
               , new string[] {
                "MATDOC_NUMBER", 
                    "SIGN,OPTION,LOW,HIGH", 
                "MATDOC_DATE",
                    "SIGN,OPTION,LOW,HIGH",
                "GRIR_DATA",
                    "PO_NUMBER,PO_ITEM,MATDOC_YEAR,MATDOC_NUMBER,MATDOC_ITEM," +
                    "MATDOC_DATE,SPC_STOCK,MATDOC_AMOUNT,VEND_CODE,PURCHDOC_PRICE," +
                    "MAT_NUMBER,MAT_DESCR,PLANT_CODE,SLOC_CODE,MATDOC_UNIT,CANCEL," +
                    "ORI_MAT_NUMBER,MATDOC_CURRENCY,MATDOC_QTY,TAX_CODE," +
                    "PO_DATE,HEADER_TEXT,IR_NO,MOV_TYPE,REF_DOC,USRID",
                "RETURN",
                    "TYPE,MESSAGE"               
               }, UID, 0);

            try
            {
                init();
                IRfcFunction f = SapRfcRepository.CreateFunction(fn);

                IRfcTable _iParmDocNo = f.GetTable(nt[0, 0]);
                IRfcTable _iParmDocDate = f.GetTable(nt[1, 0]);

                log.Put("NOREGs:", UID, loc);

                foreach (var docNo in e.DOC_NO)
                {
                    IRfcStructure s = SapRfcRepository
                        .GetStructureMetadata("ZST_SELOPT_DOCNO")
                        .CreateStructure();

                    lo.Set(s, nt[0, 0]
                        , docNo.SIGN
                        , docNo.OPTION
                        , docNo.LOW.Trim()
                        , docNo.HIGH.Trim());

                    _iParmDocNo.Append(s);
                }

                foreach (var docDate in e.DOC_DATE)
                {
                    IRfcStructure sDate = SapRfcRepository
                        .GetStructureMetadata("ZST_SELOPT_DATE")
                        .CreateStructure();

                    lo.Set(sDate, nt[1, 0]
                        , docDate.SIGN
                        , docDate.OPTION
                        , docDate.LOW.SAPInDate()
                        , docDate.HIGH.SAPInDate());

                    _iParmDocDate.Append(sDate);
                }


                f.SetValue("MATDOC_NUMBER", _iParmDocNo);
                f.SetValue("MATDOC_DATE", _iParmDocDate);

                log.Put("Invoke " + fn, UID, loc);

                f.Invoke(SapRfcDestination);

                string k = "GRIR_DATA";
                IRfcTable r;

                r = f.GetTable(k);

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", k, r.RowCount.str(), r.Count.str()), UID, loc);

                if (r.Count > 0)
                {
                    for (int i = 0; i < r.Count; i++)
                    {
                        IRfcStructure x = r[i];
                        lo.Get(x, k);
                        e.DATA.Add(new GR_IR_DATA()
                        {
                            PO_NUMBER = lo["PO_NUMBER"],
                            PO_ITEM = lo["PO_ITEM"],
                            MATDOC_YEAR = lo["MATDOC_YEAR"],
                            MATDOC_NUMBER = lo["MATDOC_NUMBER"],
                            MATDOC_ITEM = lo["MATDOC_ITEM"],
                            MATDOC_DATE = DateTime.ParseExact(lo["MATDOC_DATE"], "yyyy-MM-dd", null),
                            SPC_STOCK = lo["SPC_STOCK"],
                            MATDOC_AMOUNT = !"".Equals(lo["MATDOC_AMOUNT"].Trim()) ? Double.Parse(lo["MATDOC_AMOUNT"].Trim().Replace(",", "")) : 0,
                            VEND_CODE = lo["VEND_CODE"],
                            PURCHDOC_PRICE = lo["PURCHDOC_PRICE"],
                            MAT_NUMBER = lo["MAT_NUMBER"],
                            MAT_DESCR = lo["MAT_DESCR"],
                            PLANT_CODE = lo["PLANT_CODE"],
                            SLOC_CODE = lo["SLOC_CODE"],
                            MATDOC_UNIT = lo["MATDOC_UNIT"],
                            CANCEL = lo["CANCEL"],
                            ORI_MAT_NUMBER = lo["ORI_MAT_NUMBER"],
                            MATDOC_CURRENCY = lo["MATDOC_CURRENCY"],
                            MATDOC_QTY = !"".Equals(lo["MATDOC_QTY"].Trim()) ? Double.Parse(lo["MATDOC_QTY"].Trim()) : 0,
                            TAX_CODE = lo["TAX_CODE"],
                            PO_DATE = DateTime.ParseExact(lo["PO_DATE"], "yyyy-MM-dd", null),
                            HEADER_TEXT = lo["HEADER_TEXT"],
                            IR_NO = lo["IR_NO"],
                            MOV_TYPE = lo["MOV_TYPE"],
                            REF_DOC = lo["REF_DOC"],
                            USRID = lo["USRID"],
                            GR_STATUS = "RECEIVED"
                        });

                        lo.Put(k);

                        if (e.DATA[i].PO_NUMBER != null && !"".Equals(e.DATA[i].PO_NUMBER))
                        {
                            ajaxResult = GR_IRRepository.Instance.SaveData(e.DATA[i], "");
                        }


                    }
                }

                k = "RETURN";
                IRfcTable rm;

                rm = f.GetTable(k);

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", k, rm.RowCount.str(), rm.Count.str()), UID, loc);

                if (rm.Count > 0)
                {
                    for (int i = 0; i < rm.Count; i++)
                    {
                        IRfcStructure x = rm[i];
                        lo.Get(x, k);
                        e.RETURN.Add(new GR_IR_RETURN()
                        {
                            TYPE = lo["TYPE"],
                            MESSAGE = lo["MESSAGE"]
                        });

                        lo.Put(k);
                    }
                }

                ////List GR yang tanpa REF_DOC terisi
                //List<GR_IR_REF> grIrCancelPartial = GR_IRRepository.Instance.GetNewDataGR("PARTIAL");
                //if (grIrCancelPartial.Count > 0)
                //{
                //    foreach (GR_IR_REF item in grIrCancelPartial)
                //    {
                //        //cek jika sudah diproses namun belum invoicing
                //        List<GR_IR_REF> grAlreadyProcess = GR_IRRepository.Instance.
                //            CheckGRAlreadyProcess(item.PO_NUMBER, item.PO_ITEM, item.VEND_CODE, item.MATDOC_NUMBER);
                //        {
                //            if (grAlreadyProcess.Count > 0)
                //            {

                //                ajaxResult = GR_IRRepository.Instance.UpdateGRStatus(item.MATDOC_NUMBER, item.GR_STATUS, "");
                //            }

                //            // jika sudah di invoicing maka update Tbl Invoice
                //            if (grAlreadyProcess[0].INVOICE_ID != null && !"".Equals(grAlreadyProcess[0].INVOICE_ID))
                //            {
                //                 ajaxResult = InvoiceInquiryRepository.Instance.updateInvoiceWithCancelGR(
                //                     grAlreadyProcess[0].INVOICE_ID, "CANCELGR");
                //            }
                //        }
                //    }
                //}



                //// Cancel GR Full (REf_Doc terisi)
                //string notifFlag = "";
                //List<GR_IR_REF> grIrCancelFull = GR_IRRepository.Instance.GetNewDataGR("FULL");
                //if (grIrCancelFull.Count > 0)
                //{
                //    foreach (GR_IR_REF item in grIrCancelFull)
                //    {
                //        //cek Gr mana yang dicancel
                //        List<GR_IR_DATA> grIrDetail = GR_IRRepository.Instance.GetGR_IR_DATA_Detail(item.REF_DOC);
                //        if (grIrDetail.Count > 0)
                //        {                          
                //            //jika sudah diproses kirim notif
                //            if (!"RECEIVED".Equals(grIrDetail[0].GR_STATUS))
                //            {
                //                // Kirim EMAIL dan Notif
                //                notifFlag = "Y";
                //            }

                //            //update tbl invoice agar tidak dapat diproses
                //            if (grIrDetail[0].INVOICE_ID != null && !"".Equals(grIrDetail[0].INVOICE_ID))
                //            {
                //                ajaxResult = InvoiceInquiryRepository.Instance.updateInvoiceWithCancelGR(
                //                    grIrDetail[0].INVOICE_ID, "CANCELGR");
                //            }

                //            //update data GR yang di cancel
                //            ajaxResult = GR_IRRepository.Instance.UpdateCancelGRInitial(item.REF_DOC, item.CANCEL_DOC_NO, notifFlag, "");

                //        }       

                //    }
                //}
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
                //throw;
            }
            finally
            {
                unreg();
                del();
            }

            return ajaxResult;
        }


        public CANCEL_GR_TABLE CancelGR(CANCEL_GR_TABLE e)
        {
            string fn = "ZBAPI_CANCEL_GR";
            string loc = string.Format("CancelGR()");

            //string[] inputs = new string[2];

            string[,] nt = new string[2, 2] { 
                {"INPUT", "ZST_CANCEL_GR_IN"},
                {"OUTPUT", "ZST_CANCEL_GR_OUT"}};

            //for (int iNT = 0; iNT < 2; iNT++)
            //{
            //    inputs[iNT] = nt[iNT, 0];
            //}

            LogHelper lo = new LogHelper(log, loc
               , new string[] {
                "INPUT", 
                    "MATDOC_NUMBER,ENTRYSHEET_NUM,MATDOC_YEAR,MATDOC_DOCDATE,MATDOC_POSTDATE,MATDOC_TEXT", 
                "OUTPUT",
                    "MATDOC_NUMBER,MATDOC_YEAR,REVMAT_NUMBER,REVMAT_YEAR,TYPE,MESSAGE"               
               }, UID, 0);

            try
            {
                init();
                IRfcFunction f = SapRfcRepository.CreateFunction(fn);

                IRfcTable _iParmInput = f.GetTable(nt[0, 0]);

                log.Put("NOREGs:", UID, loc);

                foreach (var item in e.INPUT)
                {
                    IRfcStructure s = SapRfcRepository
                        .GetStructureMetadata("ZST_CANCEL_GR_IN")
                        .CreateStructure();

                    lo.Set(s, nt[0, 0]
                        , item.MATDOC_NUMBER
                        , item.ENTRYSHEET_NUM
                        , item.MATDOC_YEAR
                        , item.MATDOC_DOCDATE.SAPInDate()
                        , item.MATDOC_POSTDATE.SAPInDate()
                        , item.MATDOC_TEXT
                        );

                    _iParmInput.Append(s);
                }

                f.SetValue("INPUT", _iParmInput);

                log.Put("Invoke " + fn, UID, loc);

                f.Invoke(SapRfcDestination);

                string k = "OUTPUT";
                IRfcTable r;

                r = f.GetTable(k);

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", k, r.RowCount.str(), r.Count.str()), UID, loc);

                if (r.Count > 0)
                {
                    for (int i = 0; i < r.Count; i++)
                    {
                        IRfcStructure x = r[i];
                        lo.Get(x, k);
                        e.OUTPUT.Add(new CANCEL_GR_OUT()
                        {
                            MATDOC_NUMBER = lo["MATDOC_NUMBER"],
                            MATDOC_YEAR = lo["MATDOC_YEAR"],
                            REVMAT_NUMBER = lo["REVMAT_NUMBER"],
                            REVMAT_YEAR = lo["REVMAT_YEAR"],
                            TYPE = lo["TYPE"],
                            MESSAGE = lo["MESSAGE"]
                        });

                        //if (e.DATA[i].PO_NUMBER != null && !"".Equals(e.DATA[i].PO_NUMBER))
                        //    GR_IRRepository.Instance.SaveData(e.DATA[i], "");

                        lo.Put(k);
                    }
                }
            }


            catch (Exception ex)
            {
                ex.Err();
                throw;
            }
            finally
            {
                unreg();
                del();
            }
            return e;
        }


        public AjaxResult GetPO(PO_TABLE e)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string fn = "ZBAPI_SEND_PO";
            string loc = string.Format("GetPO()");

            //string[] inputs = new string[6];

            string[,] nt = new string[8, 2] { 
                {"PO_NUMBER", "ZST_SELOPT_DOCNO"},
                {"PO_DATE", "ZST_SELOPT_DATE"},
                {"PO_HEADER", "ZST_SENDPO_HEADER"},
                {"PO_ITEM", "ZST_SENDPO_ITEM"},
                {"PO_DETAIL_ITEM", "ZST_SENDPO_DETAIL"},
                {"PO_SERVICE", "ZST_SENDPO_SERVICE"},
                {"PO_TEXT", "ZST_SENDPO_TEXT"},
                {"RETURN", "ZST_RETURN_MSG`"}};

            //for (int iNT = 0; iNT < 8; iNT++)
            //{
            //    inputs[iNT] = nt[iNT, 0];
            //}

            LogHelper lo = new LogHelper(log, loc
               , new string[] {
                "PO_NUMBER", 
                    "SIGN,OPTION,LOW,HIGH", 
                "PO_DATE",
                    "SIGN,OPTION,LOW,HIGH",
                "PO_HEADER",
                    "PO_NUMBER,VEND_CODE,VEND_NAME,ADDRESS,POST_CODE,CITY,ATTENTION," +
                    "TEL_NUMBER,FAX_NUMBER,DELIV_NAME,DELIV_ADDR,DELIV_POST,DELIV_CITY,"+
                    "CONTACT_PER,PO_DATE,PO_PAYTERM,PO_TYPE,PO_CAT,PO_PURCH_GRP,PO_CURRENCY,"+
                    "PO_AMOUNT,PO_XCHG_RATE,PO_DELETE_FLG,PO_INCOTERM1,PO_INCOTERM2",
                "PO_ITEM",
                    "PO_NUMBER,PO_ITEM,INVOICE_FLG,PR_NUMBER,PR_ITEM,MAT_NUMBER,MAT_DESCR," +
                    "VAL_CLASS,DELIV_PLAN_DT,PLANT_CODE,SLOC_CODE,WBS_NUMBER,COST_CTR," +
                    "PO_QTY_ORI,PO_QTY_NEW,PO_QTY_USED,PO_UNIT,PRICE_PER_UNIT,ITM_DEL_FLG,TAX_CODE",
                "PO_DETAIL_ITEM",
                    "PO_NUMBER,PO_ITEM,MATDOC_YEAR,MATDOC_NUMBER,MATDOC_ITEM," +
                    "MATDOC_DATE,SPC_STOCK,MATDOC_AMOUNT,VEND_CODE,PURCHDOC_PRICE," +
                    "MAT_NUMBER,MAT_DESCR,PLANT_CODE,SLOC_CODE,MATDOC_UNIT,CANCEL," +
                    "ORI_MAT_NUMBER,MATDOC_CURRENCY,MATDOC_QTY,TAX_CODE",
                "PO_SERVICE",
                    "PO_NUMBER,PO_ITEM,SERV_ITEM,SERV_TEXT,SERV_UNIT,SERV_QTY,SERV_GPRICE",
                "PO_TEXT",
                    "PO_NUMBER,PO_ITEM,LINE_NO,LINE_TEXT",
                "RETURN",
                    "TYPE,MESSAGE"               
               }, UID, 0);

            try
            {
                init();
                IRfcFunction f = SapRfcRepository.CreateFunction(fn);

                IRfcTable _iParmDocNo = f.GetTable(nt[0, 0]);
                IRfcTable _iParmDocDate = f.GetTable(nt[1, 0]);

                log.Put("NOREGs:", UID, loc);

                foreach (var poNo in e.PO_NO)
                {
                    IRfcStructure s = SapRfcRepository
                        .GetStructureMetadata("ZST_SELOPT_DOCNO")
                        .CreateStructure();

                    lo.Set(s, nt[0, 0]
                        , poNo.SIGN
                        , poNo.OPTION
                        , poNo.LOW.Trim()
                        , poNo.HIGH.Trim());

                    _iParmDocNo.Append(s);
                }

                foreach (var docDate in e.PO_DATE)
                {
                    IRfcStructure sDate = SapRfcRepository
                        .GetStructureMetadata("ZST_SELOPT_DATE")
                        .CreateStructure();

                    lo.Set(sDate, nt[1, 0]
                        , docDate.SIGN
                        , docDate.OPTION
                        , docDate.LOW.SAPInDate()
                        , docDate.HIGH.SAPInDate());

                    _iParmDocDate.Append(sDate);
                }


                f.SetValue("PO_NUMBER", _iParmDocNo);
                f.SetValue("PO_DATE", _iParmDocDate);

                log.Put("Invoke " + fn, UID, loc);

                f.Invoke(SapRfcDestination);


                IRfcTable rh;
                rh = f.GetTable("PO_HEADER");

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", "PO_HEADER", rh.RowCount.str(), rh.Count.str()), UID, loc);

                if (rh.Count > 0)
                {
                    for (int i = 0; i < rh.Count; i++)
                    {
                        IRfcStructure xh = rh[i];
                        lo.Get(xh, "PO_HEADER");
                        e.PO_HEADER.Add(new SEND_PO_HEADER()
                        {
                            PO_NUMBER = lo["PO_NUMBER"],
                            VEND_CODE = lo["VEND_CODE"],
                            VEND_NAME = lo["VEND_NAME"],
                            ADDRESS = lo["ADDRESS"],
                            POST_CODE = lo["POST_CODE"],
                            CITY = lo["CITY"],
                            ATTENTION = lo["ATTENTION"],
                            TEL_NUMBER = lo["TEL_NUMBER"],
                            FAX_NUMBER = lo["FAX_NUMBER"],
                            DELIV_NAME = lo["DELIV_NAME"],
                            DELIV_ADDR = lo["DELIV_ADDR"],
                            DELIV_POST = lo["DELIV_POST"],
                            DELIV_CITY = lo["DELIV_CITY"],
                            CONTACT_PER = lo["CONTACT_PER"],
                            PO_DATE = DateTime.ParseExact(lo["PO_DATE"], "yyyy-MM-dd", null),
                            PO_PAYTERM = lo["PO_PAYTERM"],
                            PO_TYPE = lo["PO_TYPE"],
                            PO_CAT = lo["PO_CAT"],
                            PO_PURCH_GRP = lo["PO_PURCH_GRP"],
                            PO_CURRENCY = lo["PO_CURRENCY"],
                            //PO_AMOUNT = Convert.ToDouble(lo["PO_AMOUNT"].Trim().Replace(",", "")),
                            PO_AMOUNT = string.IsNullOrEmpty(lo["PO_AMOUNT"].Trim()) ? 0 : Convert.ToDouble(lo["PO_AMOUNT"].Trim().Replace(",", "")),
                            PO_XCHG_RATE = lo["PO_XCHG_RATE"],
                            PO_DELETE_FLG = lo["PO_DELETE_FLG"],
                            PO_INCOTERM1 = lo["PO_INCOTERM1"],
                            PO_INCOTERM2 = lo["PO_INCOTERM2"]
                        });

                        lo.Put("PO_HEADER");

                        IRfcTable ri;
                        ri = f.GetTable("PO_ITEM");

                        log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", "PO_ITEM", ri.RowCount.str(), ri.Count.str()), UID, loc);

                        int y = 0;

                        if (ri.Count > 0)
                        {
                            for (int ii = 0; ii < ri.Count; ii++)
                            {
                                IRfcStructure xi = ri[ii];
                                lo.Get(xi, "PO_ITEM");

                                if (e.PO_HEADER[i].PO_NUMBER.Equals(lo["PO_NUMBER"]))
                                {
                                    e.PO_HEADER[i].PO_ITEM.Add(new SEND_PO_ITEM()
                                    {
                                        PO_NUMBER = lo["PO_NUMBER"],
                                        PO_ITEM = lo["PO_ITEM"],
                                        INVOICE_FLG = lo["INVOICE_FLG"],
                                        PR_NUMBER = lo["PR_NUMBER"],
                                        PR_ITEM = lo["PR_ITEM"],
                                        MAT_NUMBER = lo["MAT_NUMBER"],
                                        MAT_DESCR = lo["MAT_DESCR"],
                                        VAL_CLASS = lo["VAL_CLASS"],
                                        DELIV_PLAN_DT = DateTime.ParseExact(lo["DELIV_PLAN_DT"], "yyyy-MM-dd", null),
                                        PLANT_CODE = lo["PLANT_CODE"],
                                        SLOC_CODE = lo["SLOC_CODE"],
                                        WBS_NUMBER = lo["WBS_NUMBER"],
                                        COST_CTR = lo["COST_CTR"],
                                        PO_QTY_ORI = !"".Equals(lo["PO_QTY_ORI"].Trim()) ? Double.Parse(lo["PO_QTY_ORI"].Trim().Replace(",", "")) : 0,
                                        PO_QTY_NEW = !"".Equals(lo["PO_QTY_NEW"].Trim()) ? Double.Parse(lo["PO_QTY_NEW"].Trim().Replace(",", "")) : 0,
                                        PO_QTY_USED = !"".Equals(lo["PO_QTY_USED"].Trim()) ? Double.Parse(lo["PO_QTY_USED"].Trim().Replace(",", "")) : 0,
                                        PO_UNIT = lo["PO_UNIT"],
                                        PRICE_PER_UNIT = !"".Equals(lo["PRICE_PER_UNIT"].Trim()) ? Double.Parse(lo["PRICE_PER_UNIT"].Trim().Replace(",", "")) : 0,
                                        ITM_DEL_FLG = lo["ITM_DEL_FLG"],
                                        TAX_CODE = lo["TAX_CODE"]
                                    });

                                    lo.Put("PO_ITEM");


                                    IRfcTable rdi;
                                    rdi = f.GetTable("PO_DETAIL_ITEM");

                                    log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", "PO_DETAIL_ITEM", rdi.RowCount.str(), rdi.Count.str()), UID, loc);

                                    if (rdi.Count > 0)
                                    {
                                        for (int di = 0; di < rdi.Count; di++)
                                        {
                                            IRfcStructure xdi = rdi[di];
                                            lo.Get(xdi, "PO_DETAIL_ITEM");
                                            if (e.PO_HEADER[i].PO_NUMBER.Equals(lo["PO_NUMBER"]) &&
                                                e.PO_HEADER[i].PO_ITEM[y].PO_ITEM.Equals(lo["PO_ITEM"]))
                                            {
                                                e.PO_HEADER[i].PO_ITEM[y].PO_DETAIL_ITEM.Add(new SEND_PO_DETAIL_ITEM()
                                                {
                                                    PO_NUMBER = lo["PO_NUMBER"],
                                                    PO_ITEM = lo["PO_ITEM"],
                                                    COMP_CODE = lo["COMP_CODE"],
                                                    COMP_RATE = lo["COMP_RATE"]
                                                });

                                                lo.Put("PO_DETAIL_ITEM");
                                            }
                                        }
                                    }

                                    IRfcTable rs;
                                    rs = f.GetTable("PO_SERVICE");

                                    log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", "PO_SERVICE", rs.RowCount.str(), rs.Count.str()), UID, loc);

                                    if (rs.Count > 0)
                                    {
                                        for (int s = 0; s < rs.Count; s++)
                                        {
                                            IRfcStructure xs = rs[s];
                                            lo.Get(xs, "PO_SERVICE");
                                            if (e.PO_HEADER[i].PO_NUMBER.Equals(lo["PO_NUMBER"]) &&
                                                e.PO_HEADER[i].PO_ITEM[y].PO_ITEM.Equals(lo["PO_ITEM"]))
                                            {
                                                e.PO_HEADER[i].PO_ITEM[y].PO_SERVICE.Add(new SEND_PO_SERVICE()
                                                {
                                                    PO_NUMBER = lo["PO_NUMBER"],
                                                    PO_ITEM = lo["PO_ITEM"],
                                                    SERV_ITEM = lo["SERV_ITEM"],
                                                    SERV_TEXT = lo["SERV_TEXT"],
                                                    SERV_UNIT = lo["SERV_UNIT"],
                                                    SERV_QTY = lo["SERV_QTY"],
                                                    SERV_GPRICE = lo["SERV_GPRICE"]
                                                });

                                                lo.Put("PO_SERVICE");
                                            }
                                        }
                                    }

                                    IRfcTable rt;
                                    rt = f.GetTable("PO_TEXT");

                                    log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", "PO_TEXT", rt.RowCount.str(), rt.Count.str()), UID, loc);

                                    if (rt.Count > 0)
                                    {
                                        for (int t = 0; t < rt.Count; t++)
                                        {
                                            IRfcStructure xt = rt[t];
                                            lo.Get(xt, "PO_TEXT");
                                            if (e.PO_HEADER[i].PO_NUMBER.Equals(lo["PO_NUMBER"]) &&
                                                e.PO_HEADER[i].PO_ITEM[y].PO_ITEM.Equals(lo["PO_ITEM"]))
                                            {
                                                e.PO_HEADER[i].PO_ITEM[y].PO_TEXT.Add(new SEND_PO_TEXT()
                                                {
                                                    PO_NUMBER = lo["PO_NUMBER"],
                                                    PO_ITEM = lo["PO_ITEM"],
                                                    LINE_NO = lo["LINE_NO"],
                                                    LINE_TEXT = lo["LINE_TEXT"]
                                                });

                                                lo.Put("PO_TEXT");
                                            }
                                        }
                                    }


                                    y++; //penanda item bertambah
                                }

                            } // END FOR LIst Item
                        }
                    }
                }


                IRfcTable rm;
                rm = f.GetTable("RETURN");

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", "RETURN", rm.RowCount.str(), rm.Count.str()), UID, loc);

                if (rm.Count > 0)
                {
                    for (int i = 0; i < rm.Count; i++)
                    {
                        IRfcStructure x = rm[i];
                        lo.Get(x, "RETURN");
                        e.RETURN.Add(new SEND_PO_RETURN()
                        {
                            TYPE = lo["TYPE"],
                            MESSAGE = lo["MESSAGE"]
                        });

                        lo.Put("RETURN");
                    }
                }


                if (e.PO_HEADER.Count > 0)
                {
                    for (int ph = 0; ph < e.PO_HEADER.Count; ph++)
                    {
                        ajaxResult = PO_SAPRepository.Instance.SaveData(e.PO_HEADER[ph], "");
                    }
                }

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
                //throw;
            }
            finally
            {
                unreg();
                del();
            }
            return ajaxResult;
        }


        public AjaxResult CreateInv(CREATE_INV_TABLE e, InvoiceInquiry invoice, String NoReg)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string fn = "ZBAPI_CREATE_INV";
            string loc = string.Format("CreateInv()");

            // string[] inputs = new string[2];

            string[,] nt = new string[3, 2] { 
                {"INPUT", "ZST_CREATE_INV_IN"},
                {"GL_ACCOUNT", "ZST_CREATE_GLACCOUNT_IN"},
                {"OUTPUT", "ZST_CREATE_INV_OUT"}};

            //for (int iNT = 0; iNT < 2; iNT++)
            //{
            //    inputs[iNT] = nt[iNT, 0];
            //}

            LogHelper lo = new LogHelper(log, loc
               , new string[] {
                "INPUT", 
                    "INV_DATE,POST_DATE,REF_INV,AMOUNT,TAX_AMT,ITEM_TEXT, " +
                    "PO_NUMBER,PO_ITEM,BASE_DATE,PAY_METHOD,HEAD_TEXT,TERM_PAY,TAX_CODE,BVTYP,TAX_DATE",
                "GL_ACCOUNT",
                    "GL_ACCOUNT,AMOUNT,SHKZG",
                "OUTPUT",
                    "INVOICE_NO,ACCDOC_NO,YEAR,TYPE,MESSAGE"               
               }, UID, 0);

            try
            {
                init();
                IRfcFunction f = SapRfcRepository.CreateFunction(fn);

                IRfcTable _iParmInput = f.GetTable(nt[0, 0]);
                IRfcTable _iParmGLAccount = f.GetTable(nt[1, 0]);

                log.Put("NOREGs:", UID, loc);

                foreach (var item in e.INPUT)
                {
                    IRfcStructure s = SapRfcRepository
                        .GetStructureMetadata("ZST_CREATE_INV_IN")
                        .CreateStructure();

                    lo.Set(s, nt[0, 0]
                        , item.INV_DATE.SAPInDate()
                        , item.POST_DATE.SAPInDate()
                        , item.REF_INV
                        , item.AMOUNT
                        , item.TAX_AMT
                        , item.ITEM_TEXT
                        , item.PO_NUMBER
                        , item.PO_ITEM
                        , item.BASE_DATE.SAPInDate()
                        , item.PAY_METHOD
                        , item.HEAD_TEXT
                        , item.TERM_PAY
                        , item.TAX_CODE
                        , item.BVTYP
                        , item.TAX_DATE.SAPInDate()
                        );

                    _iParmInput.Append(s);
                }

                foreach (var item in e.GL_ACCOUNT)
                {
                    IRfcStructure s = SapRfcRepository
                        .GetStructureMetadata("ZST_CREATE_GLACCOUNT_IN")
                        .CreateStructure();

                    lo.Set(s, nt[1, 0]
                        , item.GL_ACCOUNT
                        , item.AMOUNT
                        , item.SHKZG
                        );

                    _iParmGLAccount.Append(s);
                }

                f.SetValue("INPUT", _iParmInput);
                f.SetValue("GL_ACCOUNT", _iParmGLAccount);

                log.Put("Invoke " + fn, UID, loc);

                f.Invoke(SapRfcDestination);

                string k = "OUTPUT";
                IRfcTable r;

                r = f.GetTable(k);

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", k, r.RowCount.str(), r.Count.str()), UID, loc);

                if (r.Count > 0)
                {
                    for (int i = 0; i < r.Count; i++)
                    {
                        IRfcStructure x = r[i];
                        lo.Get(x, k);
                        e.OUTPUT.Add(new CREATE_INV_OUT()
                        {
                            INVOICE_NO = lo["INVOICE_NO"],
                            ACCDOC_NO = lo["ACCDOC_NO"],
                            YEAR = lo["YEAR"],
                            TYPE = lo["TYPE"],
                            MESSAGE = lo["MESSAGE"]
                        });

                        lo.Put(k);
                    }
                }

                if (e.OUTPUT != null && e.OUTPUT.Count > 0)
                {
                    if (e.OUTPUT[0].TYPE != null && !"".Equals(e.OUTPUT[0].TYPE))
                    {
                        if (CommonConstant.STATUS_SUCCESS_SAP.Equals(e.OUTPUT[0].TYPE))
                        {
                            ajaxResult = invoiceInquiryRepo.SavePostInvoiceDirect(invoice, e, 
                                CommonConstant.STATUS_INVOICE_POSTED, NoReg);

                            ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                            ajaxResult.SuccMesgs = new String[] { 
                                    string.Format("{0}", e.OUTPUT[0].MESSAGE + ", Finance SAP Doc No : " + e.OUTPUT[0].ACCDOC_NO), 
                                    };
                        }
                        else
                        {
                            ajaxResult.Result = AjaxResult.VALUE_ERROR;
                            ajaxResult.ErrMesgs = new String[] {  string.Format("{0} = {1}", e.OUTPUT[0].TYPE, 
                                e.OUTPUT[0].MESSAGE )};
                        }
                    }
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    ajaxResult.ErrMesgs = new String[] { string.Format("{0}","No Return Message from SAP")};
                }

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
            }
            finally
            {
                unreg();
                del();
            }
            return ajaxResult;
        }


        public CANCEL_INV_TABLE CancelInv(CANCEL_INV_TABLE e)
        {
            string fn = "ZBAPI_CANCEL_INV";
            string loc = string.Format("CancelInv()");

            //string[] inputs = new string[2];

            string[,] nt = new string[2, 2] { 
                {"INPUT", "ZST_CANCEL_INV_IN"},
                {"OUTPUT", "ZST_CANCEL_INV_OUT"}};

            //for (int iNT = 0; iNT < 2; iNT++)
            //{
            //    inputs[iNT] = nt[iNT, 0];
            //}

            LogHelper lo = new LogHelper(log, loc
               , new string[] {
                "INPUT", 
                    "INVOICE_NO,YEAR,REASON,POST_DATE", 
                "OUTPUT",
                    "INVOICE_NO,YEAR,REV_INV_NO,REV_YEAR,TYPE,MESSAGE"               
               }, UID, 0);

            try
            {
                init();
                IRfcFunction f = SapRfcRepository.CreateFunction(fn);

                IRfcTable _iParmInput = f.GetTable(nt[0, 0]);

                log.Put("NOREGs:", UID, loc);

                foreach (var item in e.INPUT)
                {
                    IRfcStructure s = SapRfcRepository
                        .GetStructureMetadata("ZST_CANCEL_INV_IN")
                        .CreateStructure();

                    lo.Set(s, nt[0, 0]
                        , item.INVOICE_NO
                        , item.YEAR
                        , item.REASON
                        , item.POST_DATE.SAPInDate()
                        );

                    _iParmInput.Append(s);
                }

                f.SetValue("INPUT", _iParmInput);

                log.Put("Invoke " + fn, UID, loc);

                f.Invoke(SapRfcDestination);

                string k = "OUTPUT";
                IRfcTable r;

                r = f.GetTable(k);

                log.Put(string.Format("{0}( RowCount: {1}, Count: {2})", k, r.RowCount.str(), r.Count.str()), UID, loc);

                if (r.Count > 0)
                {
                    for (int i = 0; i < r.Count; i++)
                    {
                        IRfcStructure x = r[i];
                        lo.Get(x, k);
                        e.OUTPUT.Add(new CANCEL_INV_OUT()
                        {
                            INVOICE_NO = lo["INVOICE_NO"],
                            YEAR = lo["YEAR"],
                            REV_INV_NO = lo["REV_INV_NO"],
                            REV_YEAR = lo["REV_YEAR"],
                            TYPE = lo["TYPE"],
                            MESSAGE = lo["MESSAGE"]
                        });

                        lo.Put(k);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Err();
                throw;
            }
            finally
            {
                unreg();
                del();
            }
            return e;
        }


       
    }
}
