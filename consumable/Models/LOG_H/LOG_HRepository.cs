using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class LOG_HRepository
    {
        private LOG_HRepository() { }
        private static LOG_HRepository instance = null;
        public static LOG_HRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LOG_HRepository();
                }

                return instance;
            }
        }

        public IEnumerable<LOG_H> GetListData(DateTime? ProcessDtFrom, DateTime? ProcessDtTo, string Status, string ProcessID, string FunctionID, int Start, int Length)
        {
            if (Status == "")
            {
                Status = null;
            }
            if (ProcessID == "")
            {
                ProcessID = null;
            }
            if(FunctionID == "")
            {
                FunctionID = null;
            }

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                ProcessDtFrom = ProcessDtFrom,
                ProcessDtTo = ProcessDtTo,
                Status = Status,
                ProcessID = ProcessID,
                FunctionID = FunctionID,
                Start = Start,
                Length = Length
            };

            IEnumerable<LOG_H> result = db.Fetch<LOG_H>("LOG_HGetListData", args);
            db.Close();

            return result;
        }

        public int LogHeaderCount(DateTime? ProcessDtFrom, DateTime? ProcessDtTo, string Status, string ProcessID, string FunctionID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                ProcessDtFrom = ProcessDtFrom,
                ProcessDtTo = ProcessDtTo,
                Status = Status,
                ProcessID = ProcessID,
                FunctionID = FunctionID
            };

            int result = 0;

            try
            {
                result = db.SingleOrDefault<int>("LogHeaderCount", args);
            }
            catch (Exception)
            {
                
                result = 0;
            }
            
            db.Close();
            return result;
        }

        public LOG_H GetDataByID(string ProcessID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                ProcessID = ProcessID
            };

            LOG_H result = db.SingleOrDefault<LOG_H>("LOG_HGetDataByID", args);
            db.Close();
            return result;
        }

        public string GetProcessID()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

           var result = db.SingleOrDefault<string>("LOG_H_GET_PROCESS_ID");
            db.Close();
            return result;
        }

        public void LogHeaderStart(string pProcess_ID, string pFUNCTION_ID, string pMODUL_ID, string pUSER_ID, string pREMARK, string pMSG_ID, string pERR_MSG)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var list = new
            {
                PROCESS_ID = pProcess_ID,
                FUNCTION_ID = pFUNCTION_ID,
                MODULE_ID = pMODUL_ID,
                USER_ID = pUSER_ID,
                REMARK = pREMARK,
                MSG_ID = pMSG_ID,
                ERR_MSG = pERR_MSG
            };

            db.Execute("LogHeaderStartProcessID", list);

        }
    }
}