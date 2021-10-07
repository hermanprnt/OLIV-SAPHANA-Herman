using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;
using System.Configuration;
using System.DirectoryServices;

namespace consumable.Commons
{
    public class LDAPValidator : ILoginValidator
    {
        #region Singleton

        private static ILoginValidator instance = null;
        public static ILoginValidator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LDAPValidator();
                }
                return instance;
            }
        }
        #endregion Singleton


        public bool ValidateUser(string userName, string password)
        {
            return LDAPAuth(userName, password);
        }

        public int IsActiveDirectoryUser(string userName)
        {
            /// remarked because not connected to OpenLDAP db 
            //return db.Query<int>("ldap/ldapAuthentication",
            //    new { username = userName }).FirstOrDefault();
            return 1;
        }

        public int IsAuthenticNonActiveDirectoryUser(string userName, string password)
        {
            /// remarked because not connected to OpenLDAP db 
            //return db.Query<int>("ldap/ldapAuthencticationNotInAD",
            //   new { username = userName, password = password }).FirstOrDefault();
            return 1;
        }

        public bool LDAPAuth(string userName, string password)
        {
            string domain = ConfigurationManager.AppSettings["Domain"];
            string ldapPath = ConfigurationManager.AppSettings["LdapPath"];
            String domainAndUsername = domain + @"\" + userName;
            bool DebugMode = false;
#if DEBUG
            //DebugMode = true;
#endif

            DirectoryEntry entry = null;
            if (!DebugMode)
            {
                //Bind to the native AdsObject to force authentication.			
                entry = new DirectoryEntry(ldapPath, domainAndUsername, password);
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + userName + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }
            }


            return true;
        }



        // Copied from WCF Security Center 
        // actually useless unless linked to OpenLDAP 
        public bool Login(string userName, string password)
        {
            bool r = false;

            int isActiveDirectoryUser = IsActiveDirectoryUser(userName);
            int isAuthenticNonActiveDirectoryUser = 0;

            if (isActiveDirectoryUser > 0)
                r = LDAPAuth(userName, password);
            else
            {
                isAuthenticNonActiveDirectoryUser = IsAuthenticNonActiveDirectoryUser(userName, password);
                r = (isAuthenticNonActiveDirectoryUser == 1);
            }

            //Log.Put("is AD user: " + isActiveDirectoryUser.ToString()
            //                 + " is Authentic Non AD User: " + isAuthenticNonActiveDirectoryUser.ToString()
            //    , this.GetType().Name, string.Format("Login({0}, <password>)= {1}", userName, r.ToString()));

            return r;
        }
    }
}
