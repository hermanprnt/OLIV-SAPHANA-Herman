using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consumable.Common.Data;
using consumable.Common;

namespace consumable.Logic
{
    public static class OrgHelper
    {
        public static OrgUnit Unit(this LogHelper l)
        {
            return new OrgUnit()
            {
                ORGID = l["ORGID"].Int(),
                PARENTID = l["PARENTID"].Int(),
                SHORT = l["SHORT"],
                STEXT = l["STEXT"],
                COSTCENTER = l["COSTCENTER"],
                CONT_AREA = l["CONT_AREA"],
                COSTCENTER_TEXT = l["COSTCENTER_TEXT"]
            };
        }

        public static OrgPers Personnel(this LogHelper l)
        {
            return new OrgPers()
            {
                OTYPE = l["OTYPE"],
                OBJID = l["OBJID"],
                EDITNAME = l["EDITNAME"],
                LASTNAME = l["LASTNAME"],
                FIRSTNAME = l["FIRSTNAME"],
                MIDDLENAME = l["MIDDLENAME"],
                DESCRIPTION = l["DESCRIPTION"],
                TITLE = l["TITLE"],
                DEPARTMENT = l["DEPARTMENT"],
                OFFICE_BLDG = l["OFFICE_BLDG"],
                OFFICE_ROOM = l["OFFICE_ROOM"],
                FAX = l["FAX"],
                MAIL = l["MAIL"],
                PHONE_NUMBER = l["PHONE_NUMBER"],
                MOBILE = l["MOBILE"],
                PAGER = l["PAGER"],
                HOMEPHONE = l["HOMEPHONE"],
                STREETADDRESS = l["STREETADDRESS"],
                LOCALITY = l["LOCALITY"],
                DISTRICT = l["DISTRICT"],
                POSTALCODE = l["POSTALCODE"],
                STATE = l["STATE"],
                COUNTRY = l["COUNTRY"]
            };
        }

        public static OrgPersRel Relation(this LogHelper l)
        {
            return new OrgPersRel()
            {
                ORGID = l["ORGID"].Int(),
                OTYPE = l["OTYPE"],
                OBJID = l["OBJID"],
                JOBCODE = l["JOBCODE"].Int(),
                JOBTITLE = l["JOBTITLE"],
                POSCODE = l["POSCODE"],
                POSTITLE = l["POSTITLE"],
                CLASS_CODE = l["CLASS_CODE"], 
                CLASS_TEXT = l["CLASS_TEXT"], 
                VALID_FROM = l["VALID_FROM"], 
                VALID_TO = l["VALID_TO"], 
                PERCENT= l["PERCENT"], 
                CHIEF_TAG = l["CHIEF_TAG"]
            };
        }

        public static OrgUserPosition UserPost(this LogHelper l) 
        {
            return new OrgUserPosition()
            {
                ORGID = l["ORGID"].Int(),
                OTYPE = l["OTYPE"],
                OBJID = l["OBJID"],
                JOBCODE = l["JOBCODE"].Int(),
                JOBTITLE = l["JOBTITLE"],
                POSCODE = l["POSCODE"],
                POSTITLE = l["POSTITLE"],
                CLASS_CODE = l["CLASS_CODE"],
                CLASS_TEXT = l["CLASS_TEXT"],
                VALID_FROM = l["VALID_FROM"],
                VALID_TO = l["VALID_TO"],
                PERCENT = l["PERCENT"],
                CHIEF_TAG = l["CHIEF_TAG"]
            };        
        }
         
        public static int PutObject<T>(this T u)
        {
            int rowcount =0;
            string q = "Put" + u.GetType().Name;
            Sing.Me.Text.Say("DB", "{0} {1}", q, Formator.Prop(u));
            try
            {
                rowcount = Sing.Me.Do(q, u);                
            }
            catch (Exception ex)
            {
                Sing.Me.Text.Say("DB", "{0}", ex.Trace());
            }
            return rowcount;
            
        }

        public static void Put(this OrgStruData d)
        {
            int Units = 0, Persons = 0, Relations = 0;
            List<OrgUnit> o = d.Tables.ORG_UNITS.TopoSort();
            foreach (OrgUnit u in o)
            {
                Units += u.PutObject<OrgUnit>();
            }

            foreach (OrgPers p in d.Tables.PERSON_TAB)
            {
                Persons += PutObject<OrgPers>(p);
            }

            foreach (OrgUserPosition r in d.Tables.ORG_PERS_REL)
            {
                Relations += PutObject<OrgUserPosition>(r);
            }
            Sing.Me.Do("UpdateHierarchyLevelId");
            Sing.Me.Text.Say("DB", "Updated {0} units {1} persons {2} relations", Units, Persons, Relations);
        }

        public static string Tag(this OrgUnit o, string op)
        {
            return string.Format("{0,8}\t{1}\t{2}", op, o.ORGID.ToString("00000000") , o.PARENTID.ToString("00000000"));
        }

        public static List<OrgUnit> TopoSort(this List<OrgUnit> o)
        {
            HashSet<OrgUnit> sorted = new HashSet<OrgUnit>();
            HashSet<OrgUnit> visited = new HashSet<OrgUnit>(); 
            Stack<OrgUnit> s = new Stack<OrgUnit>();
            //const string fx = "TOPO";
            // string logpath = TextLogger.GetPath();
            // Util.ReMoveFile(System.IO.Path.Combine(logpath, fx +  ".txt"));
            
            for (int i = 0; i < o.Count; i++)
            {
                s.Push(o[i]);                
                
                int loops = 0, maxloop = o.Count*o.Count; /// when pushed, stack never really exceeed two times loop 
                
                while (s.Count > 0)
                {
                    OrgUnit u = s.Pop();
                    
                    // Sing.Me.Text.Say(fx, u.Tag("Pop"));
                    int act = 0;
                    if (visited.Contains(u))
                    {
                        act = 4; 
                    }
                    else
                    {
                        OrgUnit p = o.Where(a => a.ORGID == u.PARENTID).FirstOrDefault();
                        
                        if (p != null && u.PARENTID != 0)
                        {
                            if (visited.Contains(p))
                            {
                                act = 2;                             
                            }
                            else 
                            {
                                s.Push(u);
                                // Sing.Me.Text.Say(fx, u.Tag(" Push"));
                                s.Push(p);
                                // Sing.Me.Text.Say(fx, p.Tag("-Push"));

                                act = 1;
                            }
                        }
                        else
                        {
                            act = 3;
                        }

                    }

                    if (act > 1 && act < 4)
                    {
                        visited.Add(u);
                        // Sing.Me.Text.Say(fx, u.Tag("Visit"));
                    }
                    if (act > 1 && !sorted.Contains(u))
                    {
                        // Sing.Me.Text.Say(fx, u.Tag("** Add"));
                        sorted.Add(u);                            
                        
                    }
                    ++loops;

                    // Sing.Me.Text.Say(fx, "\t\t\tAct: {0}", act);
                    if (loops > maxloop) 
                    {
                        // Sing.Me.Text.Say(fx, "max loop break");
                        break; 
                    }
                }
                // Sing.Me.Text.Say(fx, "\t\tloops:{0}", loops);
                if (loops > maxloop)
                {
                    // Sing.Me.Text.Say(fx, "max loop break");
                    break;
                }
            }
            
            return sorted.ToList();
             
        }

        public static void Put(this DirOrgStrucGetData d)
        {
            int Units = 0, Persons = 0, Relations = 0;

            List<int> added = new List<int>();
            while (added.Count < d.Tables.ORG_UNITS.Count) 
            {
                int lastAdd = added.Count;
                for (int i = 0; i < d.Tables.ORG_UNITS.Count; i ++) 
                {
                    int updated = 0;
                    if (added.Contains(i))
                        continue;
                    updated = PutObject<OrgUnit>(d.Tables.ORG_UNITS[i]);
                    if (updated > 0)
                        added.Add(i);
                }
                if (lastAdd == added.Count) 
                {
                    break;
                }
            }
            Units = added.Count;
            
            
            foreach (OrgPers p in d.Tables.PERSON_TAB)
            {
                Persons += PutObject<OrgPers>(p);
            }

            foreach (OrgPersRel r in d.Tables.ORG_PERS_REL)
            {
                Relations += PutObject<OrgPersRel>(r);
            }

            Sing.Me.Do("UpdateHierarchyLevelId");
            Sing.Me.Text.Say("DB", "Updated {0} units {1} persons {2} relations", Units, Persons, Relations);
        }
    }
}
