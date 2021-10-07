using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.Organization
{
    public class OrganizationRepository
    {
        private OrganizationRepository() { }

        #region Singleton
        private static OrganizationRepository instance = null;
        public static OrganizationRepository Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new OrganizationRepository();
                }
                return instance;
            }
        }
        #endregion

        public List<Organization> GetDivision()
        {          
            IDBContext db = DatabaseManager.Instance.GetContext();
            dynamic args = new
            {
                Type = "Division"
            };

            List<Organization> result = db.Fetch<Organization>("GetOrganization", args);
            db.Close();
            return result;
        }
        
    }
}