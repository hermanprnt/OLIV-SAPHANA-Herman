using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.UnitApproverMaintenance
{
    public class UnitApproverMaintenanceRepository
    {
        #region Singelton
        private UnitApproverMaintenanceRepository() { }
        private static UnitApproverMaintenanceRepository instance = null;

        public static UnitApproverMaintenanceRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnitApproverMaintenanceRepository();
                }
                return instance;
            }
        }
        #endregion

        #region Searching
        public List<UnitApproverMaintenance> GetDataListUnitApproveMaintenance(String PlantCD, String SlocCD, String PIC, int StartData, int EndData)
        {
            List<UnitApproverMaintenance> result = new List<UnitApproverMaintenance>();
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PLANT_CD = PlantCD,
                    SLOC_CD = SlocCD,
                    PIC = PIC,
                    StartData = StartData,
                    EndData = EndData
                };
                result = db.Fetch<UnitApproverMaintenance>("UnitApproveMaintenanceGetDataList", args);
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

        public int GetDataCountUnitApproveMaintenance(String PlantCD, String SlocCD, String PIC)
        {
            int result = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PLANT_CD = PlantCD,
                    SLOC_CD = SlocCD,
                    PIC = PIC
                };
                result = db.SingleOrDefault<int>("UnitApproveMaintenanceGetDataCount", args);
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
        #endregion

        #region Add Edit Delete
        public String SaveData(String screenMode, List<UnitApproverMaintenance> model, String userName)
        {
            String result = String.Empty;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                db.BeginTransaction();
                foreach (UnitApproverMaintenance m in model)
                {
                    dynamic args = new
                    {
                        ScreenMode = screenMode,
                        PLANT_CD = m.PLANT_CD,
                        SLOC_CD = m.SLOC_CD,
                        PIC = m.PIC,
                        DELETION_FLAG = m.DELETION_FLAG,
                        CHANGED_BY = m.CHANGED_BY,
                        CHANGED_DT = m.CHANGED_DT,
                        USER_ID = userName
                    };
                    result = db.SingleOrDefault<String>("UnitApproveMaintenanceSaveData", args);
                    if (result.Split('|')[0].Equals("E"))
                    {
                        break;
                    }
                }
                if (result.Split('|')[0].Equals("E"))
                {
                    db.AbortTransaction();
                }
                else
                {
                    db.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                result = "E|" + e.Message;
                db.AbortTransaction();
            }
            finally
            {
                db.Close();
            }
            return result;
        }
        #endregion
    }
}