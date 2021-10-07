using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.GLAccount
{
    public class GLAccountRepository
    {
        private GLAccountRepository() { }

        #region Singleton
        private static GLAccountRepository instance = null;
        public static GLAccountRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new GLAccountRepository();
                }
                return instance;
            }
        }
        #endregion

        public int countGLAccount(string glAccountNo, string glAccountName)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                GL_ACCOUNT_NO = glAccountNo,
                GL_ACCOUNT_NAME = glAccountName
            };
            return db.SingleOrDefault<int>("CountGLAccount1", args);
        }

        public List<GLAccount> GetGLAccount(string glAccountNo, string glAccountName, int fromnumber, int tonumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                GL_ACCOUNT_NO = glAccountNo,
                GL_ACCOUNT_NAME = glAccountName,
                NumberFrom = fromnumber,
                NumberTo = tonumber
            };

            List<GLAccount> result = db.Fetch<GLAccount>("GetGLAccount1", args);
            db.Close();
            return result;
        }

        public AjaxResult SaveData(IDBContext db, GLAccount glAccount, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    GL_ACCOUNT_NO = glAccount.GL_ACCOUNT_NO,
                    CATEGORY_CD = glAccount.CATEGORY_CD,
                    TYPE = glAccount.TYPE,
                    NAME = glAccount.NAME,
                    CODE = glAccount.CODE,
                    PERCENTAGE = glAccount.PERCENTAGE,
                    CREATED_BY = NoReg,
                };

                db.Execute("InsertGLAccount", args);

                ajaxResult.Result = AjaxResult.VALUE_SUCCESS;

            }
            catch (Exception ex)
            {
                ajaxResult.Result = AjaxResult.VALUE_ERROR;
                ajaxResult.ErrMesgs = new String[] { 
                                    string.Format("{0} = {1}", ex.GetType().FullName, ex.Message), 
                };
            }
            finally
            {
                db.Close();
            }

            return ajaxResult;
        }
     
        public AjaxResult EditData(IDBContext db, GLAccount glAccount, string NoReg)
        {
            //IDBContext db = DatabaseManager.Instance.GetContext();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                dynamic args = new
                {
                    GL_ACCOUNT_ID = glAccount.GL_ACCOUNT_ID,
                    GL_ACCOUNT_NO = glAccount.GL_ACCOUNT_NO,
                    CATEGORY_CD = glAccount.CATEGORY_CD,
                    TYPE = glAccount.TYPE,
                    NAME = glAccount.NAME,
                    CODE = glAccount.CODE,
                    PERCENTAGE = glAccount.PERCENTAGE,
                    CHANGED_BY = NoReg
                };

                db.Execute("UpdateGLAccount", args);

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

        public List<GLAccount> GetGLAccountByCategory(string categoryCode)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CATEGORY_CD = categoryCode
            };

            List<GLAccount> result = db.Fetch<GLAccount>("GetGLAccountList", args);
            db.Close();
            return result;
        }

        public GLAccount GetGLAccountByCode(string code)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                CODE = code
            };

            GLAccount result = db.SingleOrDefault<GLAccount>("GetGLAccountByCode", args);
            db.Close();
            return result;
        }

        public GLAccount GetByGLAccountId(string glAccountId)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic args = new
            {
                glAccountId = glAccountId
            };

            GLAccount result =
                db.SingleOrDefault<GLAccount>("GetGLAccountById", args);

            db.Close();

            return result;
        }

        public AjaxResult DeleteData(IDBContext db, List<GLAccount> glAccountList)
        {
            Console.Write("Delete GL Account Repo");
            int result = 1;
            //IDBContext db = DatabaseManager.Instance.GetContext();

            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                foreach (GLAccount item in glAccountList)
                {
                    dynamic args = new
                    {
                        GL_ACCOUNT_ID = item.GL_ACCOUNT_ID
                    };

                    result = db.Execute("DeleteGLAccount", args);

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

        
    }
}