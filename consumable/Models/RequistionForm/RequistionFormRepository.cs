using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.RequistionForm
{
    public class RequistionFormRepository
    {
        private RequistionFormRepository() { }

        #region Singleton
        private static RequistionFormRepository instance = null;
        public static RequistionFormRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new RequistionFormRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countRf(string rfNo, string rfDate,
            string divisi, string picUser, string wbsNo, string prNo)
        {
            string rfDateFrom = "";
            string rfDateTo = "";

            if (rfDate != null && !"".Equals(rfDate))
            {
                string[] rfDateArray = rfDate.Split('-');
                rfDateFrom = rfDateArray[0].Trim();
                rfDateTo = rfDateArray[1].Trim();
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                RF_NO = rfNo,
                RF_DT_FROM = rfDateFrom,
                RF_DT_TO = rfDateTo,
                DIVISI = divisi,
                PIC_USER = picUser,
                WBS_NO = wbsNo,
                PR_NO = prNo
            };
           return db.SingleOrDefault<int>("CountRf", args);
        }

        public List<RequistionFormView> GetRequistionForm(string rfNo, string rfDate,
            string divisi, string picUser, string wbsNo, string prNo, int fromNumber, int toNumber)
        {
            string rfDateFrom = "";
            string rfDateTo = "";

            if (rfDate != null && !"".Equals(rfDate)) 
            {
                string[] rfDateArray = rfDate.Split('-');
                rfDateFrom = rfDateArray[0].Trim();
                rfDateTo = rfDateArray[1].Trim();
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                RF_NO = rfNo,
                RF_DT_FROM = rfDateFrom,
                RF_DT_TO = rfDateTo,
                DIVISI = divisi,
                PIC_USER = picUser,
                WBS_NO = wbsNo,
                PR_NO = prNo,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<RequistionFormView> result = db.Fetch<RequistionFormView>("GetRequistionForm", args);
            db.Close();
            return result;
        }

        public List<String> GetRequistionFormSort(string rfNo, string rfDate, string divisi, string picUser, 
            string wbsNo, string prNo, int fromNumber, int toNumber, string field, string sort)
        {
            List<String> result = new List<String>();
            List<RequistionFormView> resultItem = new List<RequistionFormView>();
            resultItem = GetRequistionForm(rfNo, rfDate, divisi, picUser, wbsNo, prNo, fromNumber, toNumber);

            List<RequistionFormView> returnResult = new List<RequistionFormView>();
            switch (field)
            {
                case "RF_DTL_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.RF_DTL_NO).ToList() : resultItem.OrderByDescending(o => o.RF_DTL_NO).ToList());
                    break;
                case "DIFFDATE":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.DIFFDATE).ToList() : resultItem.OrderByDescending(o => o.DIFFDATE).ToList());
                    break;
                case "PIC_USER":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PIC_USER).ToList() : resultItem.OrderByDescending(o => o.PIC_USER).ToList());
                    break;
                case "DIVISION":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.DIVISION).ToList() : resultItem.OrderByDescending(o => o.DIVISION).ToList());
                    break;
                case "WBS_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.WBS_NO).ToList() : resultItem.OrderByDescending(o => o.WBS_NO).ToList());
                    break;
                case "DESCRIPTION":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.DESCRIPTION).ToList() : resultItem.OrderByDescending(o => o.DESCRIPTION).ToList());
                    break;
                case "AMOUNT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.AMOUNT).ToList() : resultItem.OrderByDescending(o => o.AMOUNT).ToList());
                    break;
                case "PR_CREATOR":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_CREATOR).ToList() : resultItem.OrderByDescending(o => o.PR_CREATOR).ToList());
                    break;
                case "PR_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_NO).ToList() : resultItem.OrderByDescending(o => o.PR_NO).ToList());
                    break;
                case "PR_CREATED_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_CREATED_DT).ToList() : resultItem.OrderByDescending(o => o.PR_CREATED_DT).ToList());
                    break;
                case "PO_NO":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_NO).ToList() : resultItem.OrderByDescending(o => o.PO_NO).ToList());
                    break;
                case "PO_CREATED_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PO_CREATED_DT).ToList() : resultItem.OrderByDescending(o => o.PO_CREATED_DT).ToList());
                    break;
                case "CREATED_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CREATED_BY).ToList() : resultItem.OrderByDescending(o => o.CREATED_BY).ToList());
                    break;
                case "CREATED_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.CREATED_DT).ToList() : resultItem.OrderByDescending(o => o.CREATED_DT).ToList());
                    break;
                case "UPDATED_BY":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.UPDATED_BY).ToList() : resultItem.OrderByDescending(o => o.UPDATED_BY).ToList());
                    break;
                case "UPDATED_DT":
                    returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.UPDATED_DT).ToList() : resultItem.OrderByDescending(o => o.UPDATED_DT).ToList());
                    break;
            }

            if (returnResult != null && returnResult.Count > 0)
            {
                int i = 0;
                foreach (RequistionFormView rf in returnResult)
                {
                    string bgColor = null;
                    if (rf.DIFFDATE == 1 && !rf.PR_CREATED_DT.HasValue)
                    {bgColor="background-color:#FFFF99";}
                    else if (rf.DIFFDATE > 1 && !rf.PR_CREATED_DT.HasValue)
                    {bgColor="background-color:#FF9494";}
                    else if (rf.DIFFDATE == 0 && rf.PR_CREATED_DT.HasValue)
                    { bgColor = "background-color:none";}

                    result.Add(
                    "<tr>" +
                        "<td class=\"text-center\" width=\"30px\">" +
                            "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" id="+i+" name=\"selectedRf\" data-value="+"Data-"+i+" />" +
                        "</td>" +
                        "<td class=\"text-center cursor-link\" width=\"120px\">" +
                            "<a onclick=\"openEditPopup('"+rf.RF_REGISTER_ID+"');\">"+rf.RF_DTL_NO+"</a>" +
                        "</td>" +
                        "<td class=\"text-center\" width=\"80px\" " + bgColor + ">" +
                            (rf.RF_DT.HasValue ? rf.RF_DT.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-left\" width=\"120px\">" +
                            rf.PIC_USER +
                        "</td>" +
                        "<td class=\"text-left ellipsis\" width=\"100px\" style=\"max-width: 100px;\" title="+rf.DIVISION+">" +
                            rf.DIVISION +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            rf.WBS_NO +
                        "</td>" +
                        "<td class=\"text-left ellipsis\" width=\"200px\" style=\"max-width: 200px;\" title="+rf.DESCRIPTION+">" +
                            rf.DESCRIPTION +
                        "</td>" +
                        "<td class=\"text-right\" width=\"100px\">" +
                            rf.AMOUNT.ToString("N0") +
                        "</td>" +
                        "<td class=\"text-left\" width=\"120px\">" +
                            rf.PR_CREATOR +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            rf.PR_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            (rf.PR_CREATED_DT.HasValue ? rf.PR_CREATED_DT.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            rf.PO_NO +
                        "</td>" +
                        "<td class=\"text-center\" width=\"120px\">" +
                            (rf.PO_CREATED_DT.HasValue ? rf.PO_CREATED_DT.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-left\" width=\"110px\">" +
                            rf.CREATED_BY +
                        "</td>" +
                        "<td class=\"text-left\" width=\"100px\">" +
                            (rf.CREATED_DT.HasValue ? rf.CREATED_DT.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                        "<td class=\"text-left\" width=\"110px\">" +
                            rf.UPDATED_BY +
                        "</td>" +
                        "<td class=\"text-center\">" +
                            (rf.UPDATED_DT.HasValue ? rf.UPDATED_DT.Value.ToString("dd.MM.yyyy") : string.Empty) +
                        "</td>" +
                    "</tr>");

                    i++;
                }
            }
            else
            {
                result.Add(
                    "<tr>" +
                    "<td colspan=\"20\" class=\"text-center\">" +
                    "No data retrieved." +
                    "</td>" +
                    "</tr>");
            }

            return result;
        }

        public List<String> GetRfSort(string field, string sort)
        {
            List<String> result = new List<String>();
            List<RequistionFormView> resultItem = new List<RequistionFormView>();
            //PRInquiry item = null;
            //Random numGen = new Random();

            //for (int count = 1; count <= 20; count++)
            //{
            //    item = new PRInquiry();
            //    item.ROW_NUM = count;
            //    item.PR_NO = "PR" + Convert.ToString(count).PadLeft(5, '0');
            //    item.PR_DESC = "PR" + Convert.ToString(count).PadLeft(5, '0') + " Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
            //    item.PR_DATE = DateTime.Now;
            //    item.PR_STATUS = "Status " + Convert.ToString(numGen.Next(1, 5));
            //    item.PLANT = "Plant " + Convert.ToString(numGen.Next(1, 10));
            //    item.STORAGE = "Storage " + Convert.ToString(numGen.Next(1, 3));
            //    item.DIVISION = "Div " + Convert.ToString(numGen.Next(1, 6));
            //    item.PROJECT_NO = "PJ" + Convert.ToString(count).PadLeft(5, '0');
            //    item.VENDOR_CD = "VN" + Convert.ToString(count).PadLeft(5, '0');
            //    item.VENDOR_NAME = "Vendor " + Convert.ToString(count);
            //    item.CREATED_BY = "Dummy User";
            //    item.CREATED_DT = "01.06.2015";

            //    if (count == 5)
            //    {
            //        item.CHANGED_BY = "Dummy User";
            //        item.CHANGED_DT = "01.06.2015";
            //    }

            //    resultItem.Add(item);
            //}

            //List<PRInquiry> returnResult = new List<PRInquiry>();
            //switch (field)
            //{
            //    case "PR_NO":
            //        returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_NO).ToList() : resultItem.OrderByDescending(o => o.PR_NO).ToList());
            //        break;
            //    case "PR_DESC":
            //        returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_DESC).ToList() : resultItem.OrderByDescending(o => o.PR_DESC).ToList());
            //        break;
            //    case "PR_DATE":
            //        returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_DATE).ToList() : resultItem.OrderByDescending(o => o.PR_DATE).ToList());
            //        break;
            //    case "PR_STATUS":
            //        returnResult = ((sort == "asc" || sort == "none") ? resultItem.OrderBy(o => o.PR_STATUS).ToList() : resultItem.OrderByDescending(o => o.PR_STATUS).ToList());
            //        break;
            //}

            //foreach (PRInquiry pr in returnResult)
            //{
            //    result.Add("<tr>" +
            //            "<td width=\"20px\" class=\"text-center grid-checkbox-col\">" +
            //                "<input type=\"checkbox\" class=\"grid-checkbox grid-checkbox-body\" /> " +
            //            "</td>" +
            //            "<td width=\"30px\" class=\"text-left\">" + pr.ROW_NUM + "</td>" +
            //            "<td width=\"120px\" class=\"text-left\">" + pr.PR_NO + "</td>" +
            //            "<td width=\"200px\" style=\"max-width: 200px;\" class=\"text-left ellipsis\">" + pr.PR_DESC + "</td>" +
            //            "<td width=\"120px\" class=\"text-center\">" + (pr.PR_DATE != DateTime.MinValue ? pr.PR_DATE.ToString("dd.MM.yyyy") : "") + "</td>" +
            //            "<td width=\"100px\" class=\"text-center\">" + pr.PR_STATUS + "</td>" +
            //            "<td width=\"80px\" class=\"text-center\">" + pr.PLANT + "</td>" +
            //            "<td width=\"80px\" class=\"text-center\">" + pr.STORAGE + "</td>" +
            //            "<td width=\"170px\" class=\"text-center\">" + pr.DIVISION + "</td>" +
            //            "<td width=\"100px\" class=\"text-center\">" + pr.PROJECT_NO + "</td>" +
            //            "<td width=\"100px\" class=\"text-center\">" + pr.VENDOR_CD + "</td>" +
            //            "<td width=\"120px\" class=\"text-left\">" + pr.VENDOR_NAME + "</td>" +
            //            "<td width=\"120px\" class=\"_toggle-detail text-left\">" + pr.CREATED_BY + "</td>" +
            //            "<td width=\"90px\" class=\"_toggle-detail text-center\">" + pr.CREATED_DT + "</td>" +
            //            "<td width=\"120px\" class=\"_toggle-detail text-left\">" + pr.CHANGED_BY + "</td>" +
            //            "<td width=\"90px\" class=\"_toggle-detail text-center\">" + pr.CHANGED_DT + "</td>" +
            //        "</tr>");
            //}

            return result;
        }

        public AjaxResult SaveData(IDBContext db, RequistionForm rf, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();         
            AjaxResult ajaxResult = new AjaxResult();         

            try
            {
                dynamic args = new
                {
                    RF_NO = rf.RF_NO,
                    RF_DT = rf.S_RF_DT.FormatSQLDate(),
                    PIC_USER = rf.PIC_USER,
                    DIVISION = rf.DIVISION,
                    WBS_NO = rf.WBS_NO,
                    TOTAL_AMOUNT = rf.TOTAL_AMOUNT
                };

                //db.Execute("InsertRF", args);
                long regId = db.ExecuteScalar<long>("InsertRF", args);

                int i = 0;
                foreach (RequistionFormDtl rfDtl in rf.listRequistionFormDtl)
                {
                    char c = Convert.ToChar(65+i);

                    dynamic argsRfDtl = new
                    {
                        RegId = regId,
                        RfDtlNo = rf.RF_NO + "-" + c,
                        PrCreator = rfDtl.PR_CREATOR,
                        Description = rfDtl.DESCRIPTION,
                        Amount = rfDtl.AMOUNT
                    };

                    db.Execute("InsertRFDtl", argsRfDtl);

                    i++;
                }

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;
               
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

        public AjaxResult EditData(IDBContext db, RequistionForm rf, RequistionForm dataDelRf)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    RF_REGISTER_ID = rf.RF_REGISTER_ID,
                    RF_NO = rf.RF_NO,
                    RF_DT = rf.S_RF_DT.FormatSQLDate(),
                    PIC_USER = rf.PIC_USER,
                    DIVISION = rf.DIVISION,
                    WBS_NO = rf.WBS_NO,
                    TOTAL_AMOUNT = rf.TOTAL_AMOUNT,
                    CHANGED_BY = "wot.user"
                };

                db.Execute("UpdateRF", args);

                dynamic argsRfGetRFNo = new
                {
                    RegId = rf.RF_REGISTER_ID
                };
                string rfDtlNo = db.SingleOrDefault<string>("GetRfDtlByRfNo", argsRfGetRFNo);
                string[] splitRfDtlNo = rfDtlNo.Split('-');
                int asciiCd = Convert.ToChar(splitRfDtlNo[1]);

                foreach (RequistionFormDtl rfDtl in rf.listRequistionFormDtl)
                {
                    if (string.IsNullOrEmpty(rfDtl.RF_REGISTER_DTL_ID))
                    {
                        asciiCd++;
                        char c = Convert.ToChar(asciiCd);

                        dynamic argsRfDtl = new
                        {
                            RegId = rf.RF_REGISTER_ID,
                            RfDtlNo = rf.RF_NO + "-" + c,
                            PrCreator = rfDtl.PR_CREATOR,
                            Description = rfDtl.DESCRIPTION,
                            Amount = rfDtl.AMOUNT
                        };
                        db.Execute("InsertRFDtl", argsRfDtl);
                    }
                    else
                    {
                        dynamic argsRfDtl = new
                        {
                            RegDtlId = rfDtl.RF_REGISTER_DTL_ID,
                            PrCreator = rfDtl.PR_CREATOR,
                            Description = rfDtl.DESCRIPTION,
                            Amount = rfDtl.AMOUNT,
                            UpdatedBy = "wot.user"
                        };
                        db.Execute("UpdateRFDtl", argsRfDtl);
                    }
                }

                foreach (RequistionFormDtl rfDtl in dataDelRf.listRequistionFormDtl)
                {
                    dynamic argsRfDtlReqId = new
                    {
                        RegDtlId = rfDtl.RF_REGISTER_DTL_ID
                    };
                    db.Execute("DeleteRFDtl", argsRfDtlReqId);
                }

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

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

        //public RequistionForm GetByKey(IDBContext db, string rfNo)
        //{
        //    dynamic args = new
        //    {
        //        RfNo = rfNo
        //    };

        //    RequistionForm result =
        //        db.SingleOrDefault<RequistionForm>("GetRfByKey", args);

        //    return result;
        //}

        public RequistionForm GetByRfRegisterId(string rfRegisterId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            
            dynamic args = new
            {
                RfRegisterId = rfRegisterId
            };

            RequistionForm result =
                db.SingleOrDefault<RequistionForm>("GetRfByKey", args);

            db.Close();

            return result;
        }

        public IList<RequistionFormDtl> getRfDtlByRegisterId(string rfRegisterId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                RfRegisterId = rfRegisterId
            };

            List<RequistionFormDtl> result =
                db.Fetch<RequistionFormDtl>("GetRfDtlByKey", args);

            db.Close();

            return result;
        }

        public AjaxResult DeleteData(List<RequistionForm> rfList)
        {
            Console.Write("Delete RF Repo");
            int result = 1;
            IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (RequistionForm item in rfList)
                {
                    dynamic args = new
                    {
                        RfDtlNo = item.RF_DTL_NO
                    };

                    result = db.Execute("DeleteRf", args);

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
            finally
            {
                db.Close();
            }

            return ajaxResult;
        }
        
    }
}