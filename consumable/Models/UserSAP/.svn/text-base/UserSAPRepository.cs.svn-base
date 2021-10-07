using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.UserSAP
{
    public class UserSAPRepository
    {
        private UserSAPRepository() { }
        private static UserSAPRepository instance = null;

        public static UserSAPRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserSAPRepository();
                }
                return instance;
            }
        }

        public int countUserSAP(string username, string completeName, string NoRegSearch, string group)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                USERNAME = username,
                COMPLETE_NAME = completeName,
                NOREG = NoRegSearch,
                GROUP = group
            };
            return db.SingleOrDefault<int>("CountUserSAP", args);
        }

        public List<UserSAP> GetUserSAP(string username, string completeName, string NoRegSearch, string group,
            int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                USERNAME = username,
                COMPLETE_NAME = completeName,
                NOREG = NoRegSearch,
                GROUP = group,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<UserSAP> result = db.Fetch<UserSAP>("GetUserSAP", args);
            db.Close();
            return result;
        }

        public AjaxResult SaveData(IDBContext db, UserSAP userSAP, string processBy)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    USERNAME = userSAP.USERNAME,
                    COMPLETE_NAME = userSAP.COMPLETE_NAME,
                    NOREG = userSAP.NOREG,
                    GROUP = userSAP.GROUP,
                    CREATED_BY = processBy,
                };

                db.Execute("InsertUserSAP", args);

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

        public AjaxResult EditData(IDBContext db, UserSAP userSAP, string processBy)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    USER_ID = userSAP.USER_ID,
                    USERNAME = userSAP.USERNAME,
                    COMPLETE_NAME = userSAP.COMPLETE_NAME,
                    NOREG = userSAP.NOREG,
                    GROUP = userSAP.GROUP,
                    UPDATED_BY = processBy
                };

                db.Execute("UpdateUserSAP", args);

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

        public AjaxResult DeleteData(IDBContext db, List<UserSAP> userSAPList)
        {
            //Console.Write("Delete System Property Repo");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (UserSAP item in userSAPList)
                {
                    dynamic args = new
                    {
                        USER_ID = item.USER_ID,
                    };

                    result = db.Execute("DeleteUserSAP", args);

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

        public UserSAP GetUserSAPById(string userId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                USER_ID = userId
            };

            UserSAP result =
                db.SingleOrDefault<UserSAP>("GetUserSAPById", args);

            db.Close();

            return result;
        }

    }
}