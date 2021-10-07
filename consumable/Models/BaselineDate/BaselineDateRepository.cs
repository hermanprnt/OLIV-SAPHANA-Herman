using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.BaselineDate
{
    public class BaselineDateRepository
    {
        private BaselineDateRepository() { }

        #region Singleton
        private static BaselineDateRepository instance = null;
        public static BaselineDateRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BaselineDateRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countBaselineDate(string holidayDate, string baselineDate)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            string holidayDateFromStr = null;
            string holidayDateToStr = null;
            DateTime? holidayDateFrom = null;
            DateTime? holidayDateTo = null;
            if (holidayDate != null && !"".Equals(holidayDate))
            {
                string[] holidayDateArray = holidayDate.Split('-');
                
                holidayDateFromStr = holidayDateArray[0].Trim();
                holidayDateToStr = holidayDateArray[1].Trim();
                holidayDateFrom = DateTime.ParseExact(holidayDateFromStr, "dd.MM.yyyy", null);
                holidayDateTo = DateTime.ParseExact(holidayDateToStr, "dd.MM.yyyy", null);
            }

            string baselineDateFromStr = null;
            string baselineDateToStr = null;
            DateTime? baselineDateFrom = null;
            DateTime? baselineDateTo = null;
            if (baselineDate != null && !"".Equals(baselineDate))
            {
                string[] baselineDateArray = baselineDate.Split('-');
                baselineDateFromStr = baselineDateArray[0].Trim();
                baselineDateToStr = baselineDateArray[1].Trim();
                holidayDateFrom = DateTime.ParseExact(baselineDateFromStr, "dd.MM.yyyy", null);
                holidayDateTo = DateTime.ParseExact(baselineDateToStr, "dd.MM.yyyy", null);
            }

            dynamic args = new
            {
                HOLIDAY_DATE_FROM = holidayDateFrom,
                HOLIDAY_DATE_TO = holidayDateTo,
                BASELINE_DATE_FROM = baselineDateFrom,
                BASELINE_DATE_TO = baselineDateTo
            };
            return db.SingleOrDefault<int>("CountBaselineDate", args);
        }

        public List<BaselineDate> GetBaselineDate(string holidayDate, string baselineDate, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            string holidayDateFromStr = null;
            string holidayDateToStr = null;
            DateTime? holidayDateFrom = null;
            DateTime? holidayDateTo = null;
            if (holidayDate != null && !"".Equals(holidayDate))
            {
                string[] holidayDateArray = holidayDate.Split('-');

                holidayDateFromStr = holidayDateArray[0].Trim();
                holidayDateToStr = holidayDateArray[1].Trim();
                holidayDateFrom = DateTime.ParseExact(holidayDateFromStr, "dd.MM.yyyy", null);
                holidayDateTo = DateTime.ParseExact(holidayDateToStr, "dd.MM.yyyy", null);
            }

            string baselineDateFromStr = null;
            string baselineDateToStr = null;
            DateTime? baselineDateFrom = null;
            DateTime? baselineDateTo = null;
            if (baselineDate != null && !"".Equals(baselineDate))
            {
                string[] baselineDateArray = baselineDate.Split('-');
                baselineDateFromStr = baselineDateArray[0].Trim();
                baselineDateToStr = baselineDateArray[1].Trim();
                holidayDateFrom = DateTime.ParseExact(baselineDateFromStr, "dd.MM.yyyy", null);
                holidayDateTo = DateTime.ParseExact(baselineDateToStr, "dd.MM.yyyy", null);
            }

            dynamic args = new
            {
                HOLIDAY_DATE_FROM = holidayDateFrom,
                HOLIDAY_DATE_TO = holidayDateTo,
                BASELINE_DATE_FROM = baselineDateFrom,
                BASELINE_DATE_TO = baselineDateTo,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<BaselineDate> result = db.Fetch<BaselineDate>("GetBaselineDate", args);
            db.Close();
            return result;
        }

        public AjaxResult SaveData(IDBContext db, BaselineDate baselineDate, string NoReg)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    HOLIDAY_DATE = DateTime.ParseExact(baselineDate.HOLIDAY_DATE_STR, "dd.MM.yyyy", null),
                    BASELINE_DATE = DateTime.ParseExact(baselineDate.BASELINE_DATE_STR, "dd.MM.yyyy", null),
                    CREATED_BY = NoReg,
                };

                db.Execute("InsertBaselineDate", args);

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

        public AjaxResult DeleteData(IDBContext db, List<BaselineDate> baselineDate)
        {
            Console.Write("Delete Baseline Date Repo");
            int result = 1;

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (BaselineDate item in baselineDate)
                {
                    dynamic args = new
                    {
                        HOLIDAY_DATE = DateTime.ParseExact(item.HOLIDAY_DATE_STR, "dd.MM.yyyy", null)
                    };

                    result = db.Execute("DeleteBaselineDate", args);

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

            return ajaxResult;
        }

        public AjaxResult EditData(IDBContext db, BaselineDate baselineDate, string NoReg)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    HOLIDAY_DATE_OLD = DateTime.ParseExact(baselineDate.HOLIDAY_DATE_OLD_STR, "dd.MM.yyyy", null),
                    HOLIDAY_DATE = DateTime.ParseExact(baselineDate.HOLIDAY_DATE_STR, "dd.MM.yyyy", null),
                    BASELINE_DATE = DateTime.ParseExact(baselineDate.BASELINE_DATE_STR, "dd.MM.yyyy", null),
                    CHANGED_BY = NoReg
                };

                db.Execute("UpdateBaselineDate", args);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

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

        public BaselineDate GetByHolidayDate(string holidayDate)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                HOLIDAY_DATE = DateTime.ParseExact(holidayDate, "dd.MM.yyyy", null)
            };

            BaselineDate result =
                db.SingleOrDefault<BaselineDate>("GetByHolidayDate", args);

            db.Close();

            return result;
        }

    }
}