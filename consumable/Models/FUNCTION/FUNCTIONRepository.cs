using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models
{
    public class FUNCTIONRepository
    {
        private FUNCTIONRepository() { }
        private static FUNCTIONRepository instance = null;
        public static FUNCTIONRepository Instance 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new FUNCTIONRepository();
                }
                return instance;
            }
        }

        public IEnumerable<FUNCTION> GetFuncNameList()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var args = new
            {

            };

            IEnumerable<FUNCTION> result = db.Fetch<FUNCTION>("FUNCTION_NAMEGetList", args);
            db.Close();

            return result;
        }

        public string GetDataByID(string Mdl_ID, string Function_ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var args = new
            {
                MDL_ID = Mdl_ID,
                FUNCTION_ID = Function_ID
            };

            string result = db.SingleOrDefault<string>("FunctionGetByID", args);
            db.Close();

            return result;
        }
    }
}