using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using consumable.Common.Data;

namespace consumable.Models.PoDownload
{
    public class PoDownloadRepository
    {
        private PoDownloadRepository() { }

        #region Singleton
        private static PoDownloadRepository instance = null;
        public static PoDownloadRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new PoDownloadRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countPoList(string createdDate, string supplierSearch, string releasedDate, string statusSearch,
            string poDescription, string poNumber, string poAmountFrom, string poAmountTo)
        {
            string createdDateFrom = "";
            string createdDateTo = "";
            if (createdDate != null && !"".Equals(createdDate))
            {
                string[] createdDateArray = createdDate.Split('-');
                createdDateFrom = createdDateArray[0].Trim();
                createdDateTo = createdDateArray[1].Trim();
            }

            string releasedDateFrom = "";
            string releasedDateTo = "";
            if (releasedDate != null && !"".Equals(releasedDate))
            {
                string[] releasedDateArray = releasedDate.Split('-');
                releasedDateFrom = releasedDateArray[0].Trim();
                releasedDateTo = releasedDateArray[1].Trim();
            }

            if (supplierSearch != null && !"".Equals(supplierSearch))
            {
                string[] supplierSearchArray = supplierSearch.Split(';');

                for (int i = 0; i < supplierSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplierSearch = "'" + supplierSearchArray[i] + "'";
                    }
                    else
                    {
                        supplierSearch = supplierSearch + ",'" + supplierSearchArray[i] + "'";
                    }
                }
            }

            //if (statusSearch != null && !"".Equals(statusSearch))
            //{
            //    string[] statusSearchArray = statusSearch.Split(';');

            //    for (int i = 0; i < statusSearchArray.Count(); i++)
            //    {
            //        if (i == 0)
            //        {
            //            statusSearch = "'" + statusSearchArray[i] + "'";
            //        }
            //        else
            //        {
            //            statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
            //        }
            //    }
            //}

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CREATED_DATE_FROM = createdDateFrom,
                CREATED_DATE_TO = createdDateTo,
                RELEASED_DATE_FROM = releasedDateFrom,
                RELEASED_DATE_TO = releasedDateTo,
                PO_NO = poNumber,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                PO_DESCRIPTION = poDescription,
                PO_AMOUNT_FROM = poAmountFrom,
                PO_AMOUNT_TO = poAmountTo
            };

            return db.SingleOrDefault<int>("CountPoList", args);
        }

        public List<PoDownload> GetPoList(string createdDate, string supplierSearch, string releasedDate, string statusSearch,
            string poDescription, string poNumber, string poAmountFrom, string poAmountTo, int fromNumber, int toNumber)
        {
            string createdDateFrom = "";
            string createdDateTo = "";
            if (createdDate != null && !"".Equals(createdDate))
            {
                string[] createdDateArray = createdDate.Split('-');
                createdDateFrom = createdDateArray[0].Trim();
                createdDateTo = createdDateArray[1].Trim();
            }

            string releasedDateFrom = "";
            string releasedDateTo = "";
            if (releasedDate != null && !"".Equals(releasedDate))
            {
                string[] releasedDateArray = releasedDate.Split('-');
                releasedDateFrom = releasedDateArray[0].Trim();
                releasedDateTo = releasedDateArray[1].Trim();
            }
            
            if (supplierSearch != null && !"".Equals(supplierSearch))
            {
                string[] supplierSearchArray = supplierSearch.Split(';');

                for (int i = 0; i < supplierSearchArray.Count(); i++)
                {
                    if (i == 0)
                    {
                        supplierSearch = "'" + supplierSearchArray[i] + "'";
                    }
                    else
                    {
                        supplierSearch = supplierSearch + ",'" + supplierSearchArray[i] + "'";
                    }
                }
            }

            //if (statusSearch != null && !"".Equals(statusSearch))
            //{
            //    string[] statusSearchArray = statusSearch.Split(';');

            //    for (int i = 0; i < statusSearchArray.Count(); i++)
            //    {
            //        if (i == 0)
            //        {
            //            statusSearch = "'" + statusSearchArray[i] + "'";
            //        }
            //        else
            //        {
            //            statusSearch = statusSearch + ",'" + statusSearchArray[i] + "'";
            //        }
            //    }
            //}

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CREATED_DATE_FROM = createdDateFrom,
                CREATED_DATE_TO = createdDateTo,
                RELEASED_DATE_FROM = releasedDateFrom,
                RELEASED_DATE_TO = releasedDateTo,
                PO_NO = poNumber,
                SUPPLIER = supplierSearch,
                STATUS = statusSearch,
                PO_DESCRIPTION = poDescription,
                PO_AMOUNT_FROM = poAmountFrom,
                PO_AMOUNT_TO = poAmountTo,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<PoDownload> result = db.Fetch<PoDownload>("GetPoList", args);
            db.Close();
            return result;
        }

        public List<SEND_PO_ITEM> GetPoItemList(string poNumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                PO_NO = poNumber
            };

            List<SEND_PO_ITEM> result = db.Fetch<SEND_PO_ITEM>("GetPoItemList", args);
            db.Close();
            return result;
        }

        public AjaxResult SubmitNotice(IDBContext db, List<PoDownload> poList)
        {
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (PoDownload item in poList)
                {
                    dynamic args = new
                    {
                        PoNumber = item.PO_NUMBER,
                        VendCode = item.VEND_CODE,
                        NoticeDate = item.NOTICE_DATE,
                        Remarks = item.NOTICE_REMARK,
                        NoticeBy = "temp.mr1",
                        ChangeBy = "temp.mr1"
                    };

                    result = db.Execute("NoticePO", args);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                }
            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
            }
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public AjaxResult ApproveRejectPo(IDBContext db, string poNumber, string vendCode, string NoRegApprover, string approverPosition, 
            string status)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                string approvalStatus = "";
                if (approverPosition != null && !"".Equals(approverPosition))
                {
                    if (status.Equals("APPROVE"))
                    {
                        if ("SH".Equals(approverPosition))
                        {
                            approvalStatus = "Approved By SH";
                        }
                        else if ("DPH".Equals(approverPosition))
                        {
                            approvalStatus = "Approved By DPH";
                        }
                        else if ("DH".Equals(approverPosition))
                        {
                            approvalStatus = "Approved By DH";
                        }
                    }
                    else
                    {
                        if ("SH".Equals(approverPosition))
                        {
                            approvalStatus = "Rejected By SH";
                        }
                        else if ("DPH".Equals(approverPosition))
                        {
                            approvalStatus = "Rejected By DPH";
                        }
                        else if ("DH".Equals(approverPosition))
                        {
                            approvalStatus = "Rejected By DH";
                        }
                    }

                    dynamic args = new
                    {
                    PO_NUMBER = poNumber,
                    VEND_CODE = vendCode,
                    NOREG_LOGIN = NoRegApprover,
                    POSITION = approverPosition,
                    APPROVAL_STATUS = approvalStatus,
                    UPDATED_BY = NoRegApprover,
                    UPDATED_DT = DateTime.Now
                    };

                    db.Execute("ApproveRejectPo", args);

                    ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
                }
                else
                {
                    ajaxResult.Result = AjaxResult.VALUE_ERROR;
                    if (status.Equals("APPROVE"))
                    {
                        ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", "Error", "You are not allowed to Approve"), 
                        };
                    }
                    else
                    {
                        ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", "Error", "You are not allowed to Reject"), 
                        };
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
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public List<PoApproval> GetPoApprovalByNoReg(string NoReg)
        {
           
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                NOREG = NoReg
            };

            List<PoApproval> result = db.Fetch<PoApproval>("GetPoApproval", args);
            db.Close();
            return result;
        }

        public AjaxResult SaveFilePo(IDBContext db, PoDownload poDownload)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                int i = 0;
                if (poDownload.PoAttachmentList != null)
                {
                    foreach (PoAttachment poAttachment in poDownload.PoAttachmentList)
                    {
                        dynamic args = new
                        {
                            PO_NUMBER = poDownload.PO_NUMBER,
                            FILE_NAME = poAttachment.FILE_NAME,
                            FILE_NAME_SERVER = poAttachment.FILE_NAME_SERVER
                        };
                        db.Execute("InsertPoAttachment", args);

                        i++;
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
            //finally
            //{
            //    db.Close();
            //}

            return ajaxResult;
        }

        public List<PoAttachment> GetListFileDownload(string poNumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                PO_NUMBER = poNumber
            };

            List<PoAttachment> result = db.Fetch<PoAttachment>("GetListFileDownloadPo", args);
            db.Close();

            return result;
        }

    }
}