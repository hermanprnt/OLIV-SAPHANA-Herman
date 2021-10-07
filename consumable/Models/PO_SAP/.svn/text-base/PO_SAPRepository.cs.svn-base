using System;
using consumable.Common.Data;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.PO_SAP
{
    public class PO_SAPRepository
    {
        private PO_SAPRepository() { }
          
        #region Singleton
        private static PO_SAPRepository instance = null;
        public static PO_SAPRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PO_SAPRepository();
                }
                return instance;
            }
        }
        #endregion

        public AjaxResult SaveData(SEND_PO_HEADER data, string NoReg)
        {
             AjaxResult ajaxResult = new AjaxResult(); 
             ajaxResult = SavePOHeaderData(data, NoReg);

             try
             {
                 for (int ph = 0; ph < data.PO_ITEM.Count; ph++)
                 {
                     ajaxResult = SavePOItemData(data.PO_ITEM[ph], NoReg);
                     
                     for (int x = 0; x < data.PO_ITEM[ph].PO_DETAIL_ITEM.Count; x++)
                     {
                         ajaxResult = SavePODetailItemData(data.PO_ITEM[ph].PO_DETAIL_ITEM[x], NoReg);
                     }

                     for (int x = 0; x < data.PO_ITEM[ph].PO_TEXT.Count; x++)
                     {
                         ajaxResult = SavePOTextData(data.PO_ITEM[ph].PO_TEXT[x], NoReg);
                     }

                     for (int x = 0; x < data.PO_ITEM[ph].PO_SERVICE.Count; x++)
                     {
                         ajaxResult = SavePOServiceData(data.PO_ITEM[ph].PO_SERVICE[x], NoReg);
                     }
                     
                 }
             }
             catch (Exception ex)
             {
                 ajaxResult.Result = AjaxResult.VALUE_ERROR;
                 ajaxResult.ErrMesgs = new String[] { 
                                string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
             }


            return ajaxResult;
        }


        public AjaxResult SavePOHeaderData(SEND_PO_HEADER data, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = data.PO_NUMBER,
                    VEND_CODE = data.VEND_CODE,
                    VEND_NAME = data.VEND_NAME,
                    ADDRESS = data.ADDRESS,
                    POST_CODE = data.POST_CODE,
                    CITY = data.CITY,
                    ATTENTION = data.ATTENTION,
                    TEL_NUMBER = data.TEL_NUMBER,
                    FAX_NUMBER = data.FAX_NUMBER,
                    DELIV_NAME = data.DELIV_NAME,
                    DELIV_ADDR = data.DELIV_ADDR,
                    DELIV_POST = data.DELIV_POST,
                    DELIV_CITY = data.DELIV_CITY,
                    CONTACT_PER = data.CONTACT_PER,
                    PO_DATE = data.PO_DATE,
                    PO_PAYTERM = data.PO_PAYTERM,
                    PO_TYPE = data.PO_TYPE,
                    PO_CAT = data.PO_CAT,
                    PO_PURCH_GRP = data.PO_PURCH_GRP,
                    PO_CURRENCY = data.PO_CURRENCY,
                    PO_AMOUNT = data.PO_AMOUNT,
                    PO_XCHG_RATE = data.PO_XCHG_RATE,
                    PO_DELETE_FLG = data.PO_DELETE_FLG,
                    PO_INCOTERM1 = data.PO_INCOTERM1,
                    PO_INCOTERM2 = data.PO_INCOTERM2,

                    CREATED_BY = "Sync"
                };
                
                db.Execute("InsertPOHeaderSAP", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
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
                db.Close();
            }

            return ajaxResult;
        }

        public AjaxResult SavePOItemData(SEND_PO_ITEM data, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = data.PO_NUMBER,
                    PO_ITEM = data.PO_ITEM,
                    INVOICE_FLG = data.INVOICE_FLG,
                    PR_NUMBER = data.PR_NUMBER,
                    PR_ITEM = data.PR_ITEM,
                    MAT_NUMBER = data.MAT_NUMBER,
                    MAT_DESCR = data.MAT_DESCR,
                    VAL_CLASS = data.VAL_CLASS,
                    DELIV_PLAN_DT = data.DELIV_PLAN_DT,
                    PLANT_CODE = data.PLANT_CODE,
                    SLOC_CODE = data.SLOC_CODE,
                    WBS_NUMBER = data.WBS_NUMBER,
                    COST_CTR = data.COST_CTR,
                    PO_QTY_ORI = data.PO_QTY_ORI,
                    PO_QTY_NEW = data.PO_QTY_NEW,
                    PO_QTY_USED = data.PO_QTY_USED,
                    PO_UNIT = data.PO_UNIT,
                    PRICE_PER_UNIT = data.PRICE_PER_UNIT,
                    ITM_DEL_FLG = data.ITM_DEL_FLG,
                    TAX_CODE = data.TAX_CODE,

                    CREATED_BY = "Sync"
                };

                db.Execute("InsertPOItemSAP", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
              
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
                db.Close();
            }

            return ajaxResult;
        }


        public AjaxResult SavePODetailItemData(SEND_PO_DETAIL_ITEM data, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = data.PO_NUMBER,
                    PO_ITEM = data.PO_ITEM,
                    COMP_CODE = data.COMP_CODE,
                    COMP_RATE = data.COMP_RATE,

                    CREATED_BY = "Sync"
                };

                db.Execute("InsertPODetailItemSAP", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
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
                db.Close();
            }

            return ajaxResult;
        }


        public AjaxResult SavePOServiceData(SEND_PO_SERVICE data, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = data.PO_NUMBER,
                    PO_ITEM = data.PO_ITEM,
                    SERV_ITEM = data.SERV_ITEM,
                    SERV_TEXT = data.SERV_TEXT,
                    SERV_UNIT = data.SERV_UNIT,
                    SERV_QTY = data.SERV_QTY,
                    SERV_GPRICE = data.SERV_GPRICE,

                    CREATED_BY = "Sync"
                };

                db.Execute("InsertPOServiceSAP", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
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
                db.Close();
            }

            return ajaxResult;
        }


        public AjaxResult SavePOTextData(SEND_PO_TEXT data, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = data.PO_NUMBER,
                    PO_ITEM = data.PO_ITEM,
                    LINE_NO = data.LINE_NO,
                    LINE_TEXT = data.LINE_TEXT,

                    CREATED_BY = "Sync"
                };

                db.Execute("InsertPOTextSAP", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
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
                db.Close();
            }

            return ajaxResult;
        }

        public AjaxResult SavePOMessageData(SEND_PO_TEXT data, string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    PO_NUMBER = data.PO_NUMBER,
                    PO_ITEM = data.PO_ITEM,
                    LINE_NO = data.LINE_NO,
                    LINE_TEXT = data.LINE_TEXT,

                    CREATED_BY = "Sync"
                };

                db.Execute("InsertPOTextSAP", args);
                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
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
                db.Close();
            }

            return ajaxResult;
        }

    }
}