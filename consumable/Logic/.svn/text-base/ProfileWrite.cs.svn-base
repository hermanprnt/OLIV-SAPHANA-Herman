using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consumable.Common.Data;
using consumable.Common;

namespace consumable.Logic
{
    public class ProfileWrite
    {
        public ProfileWrite()
        {
            
        }
        
        public delegate void Updaton();
        
        private PersonData aPerson;

        
        public void PutHeader()
        {
            foreach (var h in aPerson.Header)
                h.PutObject();
        }

        public void PutMain() {
            foreach (var m in aPerson.Main)
                m.PutObject();
        }

        public void PutTax() {
            foreach (var t in aPerson.Tax)
                t.PutObject();
        }

        public void PutId() {
            foreach (var i in aPerson.Ids)
                i.PutObject();
            }
        public void PutFamily() {
            foreach (var f in aPerson.Family)
                f.PutObject();
        }
        public void PutEducation() {
            foreach (var e in aPerson.Education)
                e.PutObject();
        }
        public void PutAddress() {
            foreach (var a in aPerson.Address)
                a.PutObject();
        }

        public List<ProfileType> ParseOption(string Option)
        {
            List<ProfileType> profilex = new List<ProfileType>();
            if (!Option.isEmpty())
            {
                List<string> pt = Enum.GetNames(typeof(ProfileType)).Select(a => a.ToLower()).ToList();

                foreach (string o in Option.Split(new char[] { ';' }))
                {
                    int iProfileType = pt.IndexOf(o);
                    if (iProfileType >= 0)
                    {
                        profilex.Add((ProfileType)iProfileType);
                    }

                }
            }
            return profilex;
        }

        public void Write(Dictionary<string, PersonData> d, List<ProfileType> t)
        {
            List<Updaton> x = new List<Updaton>() {
                        PutHeader, 
                        PutMain, 
                        PutTax, 
                        PutId, 
                        PutFamily, 
                        PutEducation, 
                        PutAddress};
            if (t.Count > 0)
            {
                if (!t.Contains(ProfileType.Header))
                    x.Remove(PutHeader);
                if (!t.Contains(ProfileType.Main))
                    x.Remove(PutMain);           
                if (!t.Contains(ProfileType.Tax))
                    x.Remove(PutTax);
                if (!t.Contains(ProfileType.Id))
                    x.Remove(PutId);
                if (!t.Contains(ProfileType.Family))
                    x.Remove(PutFamily);
                if (!t.Contains(ProfileType.Education))
                    x.Remove(PutEducation);
                if (!t.Contains(ProfileType.Address))
                    x.Remove(PutAddress); 
            }
        
            foreach (var kv in d)
            {
                aPerson = kv.Value;
                aPerson.NameFromMain();

                foreach (Updaton xx in x)
                {
                    xx(); 
                }
            }
        }


    }
}
