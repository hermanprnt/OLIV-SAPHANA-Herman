using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.PLANT
{
    public class PlantRepository
    {
        #region Singelton
        private PlantRepository() { }
        private static PlantRepository instance = null;

        public static PlantRepository Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new PlantRepository();
                }
                return instance;
            }
        }
        #endregion

        public List<Plant> GetDataListPlant(String param, int StartData, int EndData)
        {
            List<Plant> result = new List<Plant>();
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PARAM = param,
                    StartData = StartData,
                    EndData = EndData
                };

                result = db.Fetch<Plant>("PLANTGetDataList", args);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
            return result;
        }

        public int GetDataCountPlant(String param)
        {
            int result = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PARAM = param
                };

                result = db.SingleOrDefault<int>("PLANTGetDataCount", args);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
            return result;
        }
    }
}