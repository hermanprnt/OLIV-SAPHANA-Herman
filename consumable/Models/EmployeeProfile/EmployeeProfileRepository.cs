﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.EmployeeProfile
{
    public class EmployeeProfileRepository
    {
        private EmployeeProfileRepository() { }

        #region Singleton
        private static EmployeeProfileRepository instance = null;
        public static EmployeeProfileRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new EmployeeProfileRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countEmployeeProfile(string name, string position)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Name = name,
                Position = position
            };
            return db.SingleOrDefault<int>("CountEmployeeProfile", args);
        }

        public List<EmployeeProfile> GetEmployeeProfile(string name, string position, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Name = name,
                Position = position,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<EmployeeProfile> result = db.Fetch<EmployeeProfile>("GetEmployeeProfile", args);
            db.Close();
            return result;
        }

        public EmployeeProfile GetUniqueEmployeeProfile(string NoReg)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                NOREG = NoReg
            };

           EmployeeProfile result = db.SingleOrDefault<EmployeeProfile>("GetUniqueEmployeeProfile", args);
            db.Close();
            return result;
        }

        public List<EmployeeProfile> GetDataListEmployeeProfile(String param, int StartData, int EndData)
        {
            List<EmployeeProfile> result = new List<EmployeeProfile>();
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PARAM = param,
                    StartData = StartData,
                    EndData = EndData
                };

                result = db.Fetch<EmployeeProfile>("EmployeeProfileGetDataList", args);
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

        public int GetDataCountEmployeeProfile(String param)
        {
            int result = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    PARAM = param
                };

                result = db.SingleOrDefault<int>("EmployeeProfileGetDataCount", args);
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