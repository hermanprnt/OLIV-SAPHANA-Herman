using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.SLOC
{
    public class SlocRepository
    {
        #region Singelton
        private SlocRepository() { }
        private static SlocRepository instance = null;

        public static SlocRepository Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new SlocRepository();
                }
                return instance;
            }
        }
        #endregion

        public List<Sloc> GetDataListSlocByPlant(String PlantCD, String param, int StartData, int EndData)
        {
            List<Sloc> result = new List<Sloc>();
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PLANT_CD = PlantCD,
                    PARAM = param,
                    StartData = StartData,
                    EndData = EndData
                };

                result = db.Fetch<Sloc>("SLOCGetDataListByPlant", args);
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

        public int GetDataCountSlocByPlant(String PlantCD, String param)
        {
            int result = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PLANT_CD = PlantCD,
                    PARAM = param
                };

                result = db.SingleOrDefault<int>("SLOCGetDataCountByPlant", args);
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