using consumable.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.POTemporary
{
    public class POTemporaryRepository
    {
        private POTemporaryRepository() { }
        #region Singleton
        private static POTemporaryRepository instance = null;
        public static POTemporaryRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new POTemporaryRepository();
                }
                return instance;
            }
        }
        #endregion

        public List<GR_IR_TEMP_DATA> GetList(string poDateSearch, string matDocNoSearch, string poNoSearch,  string procStatusSearch,
            string supplierSearch, string suppStatusSearch, int fromNumber, int toNumber)
        {
            string poDateSearchFrom = "";
            string poDateSearchTo = "";
            if (poDateSearch != null && !"".Equals(poDateSearch))
            {
                string[] poDateSearchArray = poDateSearch.Split('-');
                poDateSearchFrom = poDateSearchArray[0].Trim();
                poDateSearchTo = poDateSearchArray[1].Trim();
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

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER_CD = supplierSearch,
                SUPPLIER_STS = suppStatusSearch,
                MATDOC_NO = matDocNoSearch,
                PROCESS_STS = procStatusSearch,
                NumberFrom = fromNumber.ToString(),
                NumberTo = toNumber.ToString()
            };

            List<GR_IR_TEMP_DATA> result = db.Fetch<GR_IR_TEMP_DATA>("GetPOTemporary", args);
            db.Close();
            return result;
        }
        public int CountList(string poDateSearch, string matDocNoSearch, string poNoSearch, string procStatusSearch,
            string supplierSearch, string suppStatusSearch)
        {
            string poDateSearchFrom = "";
            string poDateSearchTo = "";
            if (poDateSearch != null && !"".Equals(poDateSearch))
            {
                string[] poDateSearchArray = poDateSearch.Split('-');
                poDateSearchFrom = poDateSearchArray[0].Trim();
                poDateSearchTo = poDateSearchArray[1].Trim();
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

            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                PO_DATE_FROM = poDateSearchFrom,
                PO_DATE_TO = poDateSearchTo,
                PO_NO = poNoSearch,
                SUPPLIER_CD = supplierSearch,
                SUPPLIER_STS = suppStatusSearch,
                MATDOC_NO = matDocNoSearch,
                PROCESS_STS = procStatusSearch
            };

            return db.SingleOrDefault<int>("CountPOTemporary", args);
        }

        public List<string> ProcessPoTemporary(string UserName, string keys)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                LOGIN_USER_NAME = UserName,
                SELECTED_KEYS = keys
            };

            List<string> result = db.Fetch<string>("ProcessPOTemporary", args);
            db.Close();
            return result;
        }
    }
}