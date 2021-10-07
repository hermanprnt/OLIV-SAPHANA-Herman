using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class SystemRepository
    {
        private SystemRepository() { }
        private static SystemRepository instance = null;

        public static SystemRepository Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new SystemRepository();
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
    }
}