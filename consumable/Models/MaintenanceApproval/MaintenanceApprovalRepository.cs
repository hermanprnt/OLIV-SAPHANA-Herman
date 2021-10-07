using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.MaintenanceApproval
{
    public class MaintenanceApprovalRepository
    {
        private MaintenanceApprovalRepository() { }
        private static MaintenanceApprovalRepository instance = null;

        public static MaintenanceApprovalRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaintenanceApprovalRepository();
                }
                return instance;
            }
        }

        public int CountMaintenanceApproval(string supplierCode, string supplierName, string approverUnitCode)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SUPPLIER_CD = supplierCode,
                SUPPLIER_NAME = supplierName,
                APPROVER_UNIT_CD = approverUnitCode
            };
            return db.SingleOrDefault<int>("CountMaintenanceApproval", args);
        }

        public List<MaintenanceApproval> GetMaintenanceApproval(string supplierCode, string supplierName, string approverUnitCode,
            int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                SUPPLIER_CD = supplierCode,
                SUPPLIER_NAME = supplierName,
                APPROVER_UNIT_CD = approverUnitCode,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<MaintenanceApproval> result = db.Fetch<MaintenanceApproval>("GetMaintenanceApproval", args);
            db.Close();
            return result;
        }

        public AjaxResult SaveData(IDBContext db, MaintenanceApproval item, string processBy)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    SUPPLIER_CD = item.SUPPLIER_CD,
                    APPROVER_UNIT_CD = item.APPROVER_UNIT_CD,
                    CREATED_BY = processBy,
                };

                db.Execute("InsertMaintenanceApproval", args);

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

        public AjaxResult EditData(IDBContext db, MaintenanceApproval item, string processBy)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    MAINTENANCE_APPROVAL_ID = item.MAINTENANCE_APPROVAL_ID,
                    SUPPLIER_CD = item.SUPPLIER_CD,
                    APPROVER_UNIT_CD = item.APPROVER_UNIT_CD,
                    UPDATED_BY = processBy
                };

                db.Execute("UpdateMaintenanceApproval", args);

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

        public AjaxResult DeleteData(IDBContext db, List<MaintenanceApproval> itemList)
        {
            //Console.Write("Delete System Property Repo");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (MaintenanceApproval item in itemList)
                {
                    dynamic args = new
                    {
                        MAINTENANCE_APPROVAL_ID = item.MAINTENANCE_APPROVAL_ID,
                    };

                    result = db.Execute("DeleteMaintenanceApproval", args);

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

        public MaintenanceApproval GetMaintenanceApprovalById(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                MAINTENANCE_APPROVAL_ID = id
            };

            MaintenanceApproval result =
                db.SingleOrDefault<MaintenanceApproval>("GetMaintenanceApprovalById", args);

            db.Close();

            return result;
        }

    }
}