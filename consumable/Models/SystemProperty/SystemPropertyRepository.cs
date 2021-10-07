using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.SystemProperty
{
    public class SystemPropertyRepository
    {
        private SystemPropertyRepository() { }
        private static SystemPropertyRepository instance = null;

        public static SystemPropertyRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemPropertyRepository();
                }
                return instance;
            }
        }

        public SystemProperty GetStatus(string Status)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Status = Status
            };

            SystemProperty result = db.SingleOrDefault<SystemProperty>("SYSTEM_GetStatus", args);
            db.Close();
            return result;
        }

        public List<SystemProperty> GetTMMIN()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new{};
            List<SystemProperty> result = db.Fetch<SystemProperty>("SYSTEM_GetTMMIN", args);
            db.Close();
            return result;
        }

        public IEnumerable<SystemProperty> GetVAT_OUT_H_CSV()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SystemID = "TMMIN"
            };

            IEnumerable<SystemProperty> result = db.Fetch<SystemProperty>("SYSTEM_GetVAT_OUT_CSV_H", args);
            db.Close();
            return result;
        }

        public string GetLOG_H_STS(string SystemType)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_TYPE = SystemType
            };

            string result = db.SingleOrDefault<string>("SYSTEM_GetLOG_H_STS", args);
            db.Close();
            return result;
        }

        public SystemProperty GetVAT_OUT_STS(string SystemType)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_TYPE = SystemType
            };

            SystemProperty result = db.SingleOrDefault<SystemProperty>("SYSTEM_GetVAT_OUT_STS", args);
            db.Close();
            return result;
        }

        public string GetInvStatus(string InvStatus)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_TYPE = InvStatus
            };

            string result = db.SingleOrDefault<string>("SYSTEM_GetINV_STS", args);
            db.Close();
            return result;
        }

        public string GetIntStatus(string IntStatus)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_TYPE = IntStatus
            };

            string result = db.SingleOrDefault<string>("SYSTEM_GetINT_STS", args);
            db.Close();
            return result;
        }

        public string GetIntSource(string IntSrc)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_TYPE = IntSrc
            };

            string result = db.SingleOrDefault<string>("SYSTEM_GetINT_SRC", args);
            db.Close();
            return result;
        }

        public string GetPathFolderVAT_IN_Upload()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };

            string result = db.SingleOrDefault<string>("SYSTEM_GetPathFolder_VAT_IN_Upload", args);
            db.Close();
            return result;
        }

        public int GetMaxSize()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };

            int result = db.SingleOrDefault<int>("SYSTEM_GetMaxFileSize", args);
            db.Close();
            return result;
        }

        public string GetServer()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };

            string result = db.SingleOrDefault<string>("SYSTEM_GetSERVER", args);
            db.Close();
            return result;
        }

        public List<SystemProperty> GetProxy()
        {
            //List<SYSTEM> result = new List<SYSTEM>();
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };

            List<SystemProperty> result = db.Fetch<SystemProperty>("SYSTEM_GetPROXY", args);
            db.Close();
            return result;
        }

        public List<String> GetRecordsPerPage()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };

            List<String> result = db.Fetch<String>("SYSTEM_GetRecordsPerPage", args);
            db.Close();
            return result;
        }

        public bool isAllowReplacement()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new { };

            DateTime result = db.SingleOrDefault<DateTime>("SYSTEM_isAllowReplacement", args);
            db.Close();

            if (result.AddDays(1) > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int countSystemProperty(string systemPropertyCode, string systemPropertyType, string systemValueText)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_CD = systemPropertyCode,
                SYSTEM_TYPE = systemPropertyType,
                SYSTEM_VALUE_TEXT = systemValueText
            };
            return db.SingleOrDefault<int>("CountSystemProperty", args);
        }

        public List<SystemProperty> GetSystemProperty(string systemPropertyCode, string systemPropertyType, 
            string systemValueText, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SYSTEM_CD = systemPropertyCode,
                SYSTEM_TYPE = systemPropertyType,
                SYSTEM_VALUE_TEXT = systemValueText,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<SystemProperty> result = db.Fetch<SystemProperty>("GetSystemProperty", args);
            db.Close();
            return result;
        }

        public AjaxResult SaveData(IDBContext db, SystemProperty systemProperty, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                DateTime ?sysValueDt = null;
                if (systemProperty.S_SYSTEM_VALUE_DT != null)
                {
                    sysValueDt = DateTime.ParseExact(systemProperty.S_SYSTEM_VALUE_DT, "dd.MM.yyyy", null);
                }

                dynamic args = new
                {
                    SYSTEM_CD = systemProperty.SYSTEM_CD,
                    SYSTEM_TYPE = systemProperty.SYSTEM_TYPE,
                    SYSTEM_VALUE_TEXT = systemProperty.SYSTEM_VALUE_TEXT,
                    SYSTEM_VALUE_DT = sysValueDt,
                    SYSTEM_VALUE_NUM = systemProperty.SYSTEM_VALUE_NUM,
                    SYSTEM_DESC = systemProperty.SYSTEM_DESC,
                    CREATED_BY = NoReg,
                };

                db.Execute("InsertSystemProperty", args);

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

        public SystemProperty GetBySystemPropertyId(string systemPropertyId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                SYSTEM_ID = systemPropertyId
            };

            SystemProperty result =
                db.SingleOrDefault<SystemProperty>("GetSystemPropertyById", args);

            db.Close();

            return result;
        }

        public AjaxResult EditData(IDBContext db, SystemProperty systemProperty, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                DateTime? sysValueDt = null;
                if (systemProperty.S_SYSTEM_VALUE_DT != null)
                {
                    sysValueDt = DateTime.ParseExact(systemProperty.S_SYSTEM_VALUE_DT, "dd.MM.yyyy", null);
                }

                dynamic args = new
                {
                    SYSTEM_ID = systemProperty.SYSTEM_ID,
                    SYSTEM_VALUE_TEXT = systemProperty.SYSTEM_VALUE_TEXT,
                    SYSTEM_VALUE_DT = sysValueDt,
                    SYSTEM_VALUE_NUM = systemProperty.SYSTEM_VALUE_NUM,
                    SYSTEM_DESC = systemProperty.SYSTEM_DESC,
                    CHANGED_BY = NoReg
                };

                db.Execute("UpdateSystemProperty", args);

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

        public AjaxResult DeleteData(IDBContext db, List<SystemProperty> systemPropertyList)
        {
            //Console.Write("Delete System Property Repo");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (SystemProperty item in systemPropertyList)
                {
                    dynamic args = new
                    {
                        SYSTEM_ID = item.SYSTEM_ID
                    };

                    result = db.Execute("DeleteSystemProperty", args);

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

        public List<SystemProperty> GetBySystemPropertyType(string systemPropertyType)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                SYSTEM_TYPE = systemPropertyType
            };

            List<SystemProperty> result =
                db.Fetch<SystemProperty>("GetSystemPropertyByType", args);

            db.Close();

            return result;
        }

        public SystemProperty GetSysPropByCodeAndType(string sysPropCode, string sysPropType)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                SYSTEM_CD = sysPropCode,
                SYSTEM_TYPE = sysPropType
            };

            SystemProperty result =
                db.SingleOrDefault<SystemProperty>("GetSystemPropertyByCodeAndType", args);

            db.Close();

            return result;
        }
    }
}