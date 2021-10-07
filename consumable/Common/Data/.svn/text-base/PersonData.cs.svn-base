using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class PersonData
    {

        private string _noreg = null;
        public string NOREG
        {
            get { return _noreg; } 
            set { _noreg = value; }  
        }
        public List<ProfileTax> Tax { get; set; }
        public List<ProfilePersonalId> Ids { get; set; }
        public List<ProfileMain> Main { get; set; }
        public List<ProfileFamily> Family { get; set; }
        public List<ProfileEducation> Education { get; set; }
        public List<ProfileAddress> Address { get; set; }
        public List<ProfileHeader> Header { get; set; }

        public void NameFromMain()
        {
            string nm = null;
            var firstName = Main.Where(a => (a.VALID_TO ?? new DateTime(9999,12,31))
                        .Equals(new DateTime(9999, 12, 31)))
                        .FirstOrDefault(); 
                        
            if (firstName != null)
            {
                nm = firstName.NAME;
                foreach (var h in Header)
                {
                    h.NAME = nm;
                }
            }
            
        }
        public void Add(ProfileMain m)
        {
            Main.Add(m);
        }

        public void Add(ProfileFamily f)
        {
            Family.Add(f);
        }

        public void Add(ProfileEducation e)
        {
            Education.Add(e);
        }

        public void Add(ProfileTax t)
        {
            Tax.Add(t);
        }

        public void Add(ProfilePersonalId i)
        {
            Ids.Add(i);
        }

        public void Add(ProfileAddress a)
        {
            Address.Add(a);
        }

        public void Add(ProfileHeader h)
        {
            Header.Add(h);
        }

        public PersonData()
        {
            NOREG = null;
            Tax = new List<ProfileTax>();
            Ids = new List<ProfilePersonalId>();
            Main = new List<ProfileMain>();
            Family = new List<ProfileFamily>();
            Education = new List<ProfileEducation>();
            Address = new List<ProfileAddress>();
            Header = new List<ProfileHeader>();
        }
        
        public bool isEmpty()
        {
            return (Tax == null || Tax.Count < 1)
                && (Ids == null || Ids.Count < 1)
                && (Main == null || Main.Count < 1)
                && (Family == null || Family.Count < 1)
                && (Education == null || Education.Count < 1)
                && (Address == null || Address.Count < 1);
        }
    }
}
