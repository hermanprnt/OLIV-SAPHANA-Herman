using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consumable.Common.Data;
using Dapper;
using consumable.Common;

namespace consumable.Logic
{
    public enum ProfileType
    {
        None = 0,
        Tax = 1,
        Id = 2,
        Main = 3,
        Header = 4,
        Family = 5,
        Education = 6,
        Address = 7
    }

    public static class ProfileHelper
    {
        public static void GetIt<T>(List<T> x, string SQL, string NOREG)
        {
            IEnumerable<T> X = Sing.Me.Qx<T>(SQL,
                new { NoReg = NOREG, debug = AppSetting.DEBUG });
            if (X != null)
            {
                x.Clear();
                x.AddRange(X);
            }
        }

        public static void Get(this PersonData p)
        {
            GetIt<ProfileMain>(p.Main, "GetProfileMain", p.NOREG);
            GetIt<ProfileTax>(p.Tax, "GetProfileTax", p.NOREG);
            GetIt<ProfilePersonalId>(p.Ids, "GetProfileID", p.NOREG);
            GetIt<ProfileFamily>(p.Family, "GetProfileFamily", p.NOREG);
            GetIt<ProfileEducation>(p.Education, "GetProfileEducation", p.NOREG);
            GetIt<ProfileAddress>(p.Address, "GetProfileAddress", p.NOREG);
        }

        public static void Put(this PersonData p)
        {
            p.NameFromMain();

            foreach (var h in p.Header)
                h.PutObject();

            foreach (var m in p.Main)
                m.PutObject();

            foreach (var t in p.Tax)
                t.PutObject();

            foreach (var i in p.Ids)
                i.PutObject();

            foreach (var f in p.Family)
                f.PutObject();

            foreach (var e in p.Education)
                e.PutObject();

            foreach (var a in p.Address)
                a.PutObject();
        }

        public static void Posted(this PersonData p)
        {
            if (p == null) return;


            int posts = 0;
            StringBuilder bu = new StringBuilder("POSTED " + p.NOREG + "\r\n");

            if (p.Tax != null)
            {
                ++posts;
                bu.AppendLine("1\tTAX");
                Sing.Me.Do("SetApprovedToPosted",
                    new { NoReg = p.NOREG, InfoType = 1, SubType = "" });
            }

            if (p.Ids != null && p.Ids.Count > 0)
            {

                foreach (var i in p.Ids)
                {
                    ++posts;
                    bu.AppendLine("2\tID\tTYPE:" + i.ID_TYPE);
                    Sing.Me.Do("SetApprovedToPosted",
                        new { NoReg = p.NOREG, InfoType = 2, SubType = i.ID_TYPE });
                }
            }

            if (p.Main != null)
            {
                ++posts;
                bu.AppendLine("3\tMAIN");
                Sing.Me.Do("SetApprovedToPosted",
                    new { NoReg = p.NOREG, InfoType = 3, SubType = "" });
            }
            if (p.Family != null && p.Family.Count > 0)
            {
                foreach (var f in p.Family)
                {
                    ++posts;
                    bu.AppendLine("5\tFAMILY\tRELATION:" + f.RELATIONSHIP);
                    Sing.Me.Do("SetApprovedToPosted",
                        new { NoReg = p.NOREG, InfoType = 5, SubType = f.RELATIONSHIP });
                }
            }

            if (p.Education != null && p.Education.Count > 0)
            {
                foreach (var e in p.Education)
                {
                    ++posts;
                    bu.AppendLine("6\tEDUCATION\tLEVEL:" + e.LEVEL);
                    Sing.Me.Do("SetApprovedToPosted",
                        new { NoReg = p.NOREG, InfoType = 6, SubType = e.LEVEL });
                }
            }
            if (p.Address != null && p.Address.Count > 0)
            {
                foreach (var a in p.Address)
                {
                    ++posts;
                    bu.AppendLine("7\tADDRESS\tTYPE:" + a.ADDRESS_TYPE);
                    Sing.Me.Do("SetApprovedToPosted",
                        new { NoReg = p.NOREG, InfoType = 7, SubType = a.ADDRESS_TYPE });
                }
            }
            if (posts > 0)
            {
                Sing.Me.Log.Put(bu.ToString(), AppSetting.ApplicationID, "Posted");
            }

        }

        public static ProfileHeader Header(this LogHelper l)
        {
            return new ProfileHeader()
            {
                NOREG = l["NOREG"],
                NAME = l["NAME"], 
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                CLASS = l["CLASS"],
                POSITION = l["POSITION"],
                STATUS = l["STATUS"],
                ENTRY_DATE = l["ENTRY_DATE"].dateSQL(),
                DIVISION = l["DIVISION"],
                DEPARTMENT = l["DEPARTMENT"],
                SECTION = l["SECTION"],
                LINE = l["LINE"],
                GROUP = l["GROUP"],
                UNIT_CODE = l["UNIT_CODE"],
                MAIN_LOCATION = l["MAIN_LOCATION"],
                SUB_LOCATION = l["SUB_LOCATION"],
            };
        }

        public static ProfileMain MainData(this LogHelper l)
        {
            return new ProfileMain()
            {
                NOREG = l["NOREG"],
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                TITLE = l["TITLE"],
                NAME = l["NAME"],
                GENDER = l["GENDER"],
                DATE_OF_BIRTH = l["DATE_OF_BIRTH"].dateSQL(),
                PLACE_OF_BIRTH = l["PLACE_OF_BIRTH"],
                COUNTRY_OF_BIRTH = l["COUNTRY_OF_BIRTH"],
                PROVINCE_OF_BIRTH = l["PROVINCE_OF_BIRTH"],
                NATIONALITY = l["NATIONALITY"],
                RELIGION = l["RELIGION"],
                MARITAL_STATUS = l["MARITAL_STATUS"],
                MARITAL_DATE = l["MARITAL_DATE"].dateSQL(),
                BPK_STATUS = l["BPK_STATUS"]
            };
        }

        public static ProfileFamily Family(this LogHelper l)
        {
            return new ProfileFamily()
            {
                NOREG = l["NOREG"],
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                RELATIONSHIP = l["RELATIONSHIP"],
                CHILD_TO = l["CHILD_TO"],
                NAME = l["NAME"],
                GENDER = l["GENDER"],
                PLACE_OF_BIRTH = l["PLACE_OF_BIRTH"],
                DATE_OF_BIRTH = l["DATE_OF_BIRTH"].dateSQL(),
                NATIONALITY = l["NATIONALITY"],
                COMPLETE_OF_DATA = l["COMPLETE_OF_DATA"],
                STATUS_STUDY = l["STATUS_STUDY"]
            };
        }

        public static ProfilePersonalId Id(this LogHelper l)
        {
            return new ProfilePersonalId()
            {
                NOREG = l["NOREG"],
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                ID_TYPE = l["ID_TYPE"],
                ID_NO = l["ID_NO"],
                NAME = l["NAME"],
                ISSUE_DATE = l["ISSUE_DATE"].dateSQL(),
                PLACE_OF_ISSUE = l["PLACE_OF_ISSUE"],
                COUNTRY = l["COUNTRY"]
            };
        }

        public static ProfileEducation Education(this LogHelper l)
        {
            return new ProfileEducation()
            {     
                NOREG = l["NOREG"],
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                START_DATE = l["START_DATE"].dateSQL() ?? new DateTime(1900, 1, 1) , 
                LEVEL = l["LEVEL"],
                INSTITUTE_NAME = l["INSTITUTE_NAME"],
                COUNTRY = l["COUNTRY"],
                FINAL_GRADE = l["FINAL_GRADE"],
                MAJOR = l["MAJOR"],
                FINAL_CERTIFICATE = l["FINAL_CERTIFICATE"]
            };
        }

        public static ProfileAddress Address(this LogHelper l)
        {
            return new ProfileAddress()
            {
                NOREG = l["NOREG"],
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                ADDRESS_TYPE = l["ADDRESS_TYPE"],
                ADDRESS1_2 = l["ADDRESS1_2"],
                RT_RW = l["RT_RW"],
                ADDRESS3 = l["ADDRESS3"],
                ADDRESS4 = l["ADDRESS4"],
                ADDRESS5 = l["ADDRESS5"],
                ADDRESS6 = l["ADDRESS6"],
                CITY = l["CITY"],
                PROVINCE = l["PROVINCE"],
                COUNTRY = l["COUNTRY"],
                POSTAL_CODE = l["POSTAL_CODE"],
                TELEPHONE = l["TELEPHONE"],
                HANDPHONE_TYPE = l["HANDPHONE_TYPE"],
                HANDPHONE_NO = l["HANDPHONE_NO"]
            };
        }


        public static string[][] cols = new string[][] 
        {
            // TITLE
            new string [] {"", "TAX", "ID", "MAIN", "HEADER", "FAMILY", "EDUCATION", 
                "ADDRESS"}, 
            /// TAX 
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "NPWP_ID", 
                "NPWP_REG_DATE", "DEPENDENT_QTY"}, 
            /// ID 
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "ID_TYPE", "ID_NO", 
                "NAME", "ISSUE_DATE", "PLACE_OF_ISSUE", "COUNTRY"}, 
            /// MAIN 
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "TITLE", "NAME", 
                "GENDER", "DATE_OF_BIRTH", "PLACE_OF_BIRTH", "COUNTRY_OF_BIRTH", 
                "PROVINCE_OF_BIRTH", "NATIONALITY", "RELIGION", "MARITAL_STATUS", 
                "MARITAL_DATE", "BPK_STATUS"}, 
            /// HEADER
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "CLASS", "POSITION", 
                "STATUS", "ENTRY_DATE", "DIVISION", "DEPARTMENT", "SECTION",
                "LINE", "GROUP", "UNIT_CODE", "MAIN_LOCATION", "SUB_LOCATION"}, 
            /// FAMILY 
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "RELATIONSHIP", 
                "CHILD_TO", "NAME", "GENDER", "PLACE_OF_BIRTH", "DATE_OF_BIRTH", 
                "NATIONALITY", "COMPLETE_OF_DATA", "STATUS_STUDY"},
            /// EDUCATION
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "LEVEL", "INSTITUTE_NAME", 
                "COUNTRY", "FINAL_GRADE", "MAJOR", "FINAL_CERTIFICATE"}, 
            /// ADDRESS 
            new string [] {"NOREG", "VALID_FROM", "VALID_TO", "ADDRESS_TYPE", 
                 "ADDRESS1_2", "RT_RW", "ADDRESS3", "ADDRESS4", "ADDRESS5", 
                 "ADDRESS6", "CITY", "PROVINCE", "COUNTRY", "POSTAL_CODE", 
                 "TELEPHONE", "HANDPHONE_TYPE", "HANDPHONE_NO"} 
        };

        public static string AsString(this LogHelper l, int typo)
        {
            StringBuilder b = new StringBuilder("");
            int fi = 0;
            foreach (string f in cols[typo])
            {
                if (fi > 0) b.Append("\t");
                fi++;
                b.Append(l[f] ?? "");
            }
            return b.ToString();
        }

        public static ProfileTax Tax(this LogHelper l)
        {
            return new ProfileTax()
            {
                NOREG = l["NOREG"],
                VALID_FROM = l["VALID_FROM"].dateSQL(),
                VALID_TO = l["VALID_TO"].dateSQL(),
                NPWP_ID = l["NPWP_ID"],
                NPWP_REG_DATE = l["NPWP_REG_DATE"].dateSQL(),
                DEPENDENT_QTY = l["DEPENDENT_QTY"].Int()
            };
        }

        public static void Put(this ProfileData d)
        {
            string q = "Put" + d.GetType().Name;
            Sing.Me.Text.Say("DB", "{0} {1}", q, d.NOREG);
            try
            {
                Sing.Me.Do(q, d);
            }
            catch (Exception ex)
            {
                Sing.Me.Text.Say("DB", "{0}\r\n{1}", ex.Message, ex.Trace());
            }
        }


        public static void Cache(this LogHelper l, int iKey)
        {

        }

        public static ProfileData ToObject(this LogHelper l, int iKey)
        {
            ProfileType tx = ProfileType.None;
            if (Enum.IsDefined(typeof(ProfileType), iKey))
                tx = (ProfileType)iKey;
            ProfileData p = null;
            switch (tx)
            {
                case ProfileType.Tax:
                    p = l.Tax();
                    break;
                case ProfileType.Id:
                    p = l.Id();
                    break;
                case ProfileType.Main:
                    p = l.MainData();
                    break;
                case ProfileType.Header:
                    p = l.Header();
                    break;
                case ProfileType.Family:
                    p = l.Family();
                    break;
                case ProfileType.Education:
                    p = l.Education();
                    break;
                case ProfileType.Address:
                    p = l.Address();
                    break;
                default:
                    break;
            }
            return p;
        }

        public static void WritePersonData(this LogHelper l, int iKey, Dictionary<string, PersonData> d)
        {
            ProfileData p = l.ToObject(iKey);
            PersonData e = null;
            if (d.ContainsKey(p.NOREG))
                e = d[p.NOREG];
            else
            {
                e = new PersonData();
                d.Add(p.NOREG, e);
            }
        }

        public static void WriteProfile(this LogHelper l, int iKey)
        {

            ProfileType tx = ProfileType.None;
            if (Enum.IsDefined(typeof(ProfileType), iKey))
                tx = (ProfileType)iKey;
            switch (tx)
            {
                case ProfileType.Tax:
                    l.Tax().PutObject();
                    break;
                case ProfileType.Id:
                    l.Id().PutObject();
                    break;
                case ProfileType.Main:
                    l.MainData().PutObject();
                    break;
                case ProfileType.Header:
                    l.Header().PutObject();
                    break;
                case ProfileType.Family:
                    l.Family().PutObject();
                    break;
                case ProfileType.Education:
                    l.Education().PutObject();
                    break;
                case ProfileType.Address:
                    l.Address().PutObject();
                    break;
                default:
                    break;
            }
        }
    }
}
