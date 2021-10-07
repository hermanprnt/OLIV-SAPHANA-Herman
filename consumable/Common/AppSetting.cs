using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

namespace consumable.Common
{
    public static class AppSetting
    {
        public static object ReadObject(string name, string pType)
        {
            object o = null;
            
            if ("String".Equals(pType))
            {
                o = Read(name, null);
            }
            else if (pType.StartsWith("Int"))
            {
                o = ReadInt(name);
            }
            else if (pType.StartsWith("Bool"))
            {
                string bv = Read(name);

                bool r = false;
                if ("True".Equals(bv))
                    r = true;
                else if ("False".Equals(bv))
                    r = false;
                else
                {
                    int ibv = 0;
                    int.TryParse(bv, out ibv);
                    r = (ibv != 0);
                }
                o = r;
            }

            return o;
        }

        public static string GetNamespace()
        {
            string baseName = Assembly.GetExecutingAssembly().ToString();
            baseName = baseName.Substring(0, baseName.IndexOf(","));
            return baseName;
        }

        static AppSetting()
        {
            string n = GetNamespace(); 
            ApplicationID = n;
            UID = n; 

            /// fetch public property values using Reflection 
            foreach (PropertyInfo p in typeof(AppSetting).GetProperties())
            {
                string pt = p.PropertyType.Name;
                string pn = p.Name;               

                object o = ReadObject(pn, pt);

                if (o!=null && !string.IsNullOrEmpty(pn))
                    p.SetValue(null, o, null);
                else
                {
                    if (p.GetCustomAttributes(true).Length > 0)
                    {
                        object[] defaultValueAttribute =
                            p.GetCustomAttributes(typeof(DefaultSettingValueAttribute), true);
                        if (defaultValueAttribute != null && defaultValueAttribute.Length > 0)
                        {
                            DefaultSettingValueAttribute dva =
                                defaultValueAttribute[0] as DefaultSettingValueAttribute;
                            if (dva != null)
                            {
                                p.SetValue(null, dva.Value, null);
                                o = dva.Value;
                            }
                        }
                    }
                }
                //System.Diagnostics.Debug.WriteLine("public static {0} {1}={2}", pt, pn, o);
            }
            /// fetch fields values using Reflection 
            foreach (FieldInfo fi in 
                    typeof(AppSetting).GetFields(
                        BindingFlags.NonPublic 
                      | BindingFlags.Static)) 
            {
                string pt = fi.FieldType.Name;
                string fin = fi.Name;
                if (!fin.StartsWith("<"))
                {
                    object o = ReadObject(fin, pt);
                    fi.SetValue(null, o);

                    /// System.Diagnostics.Debug.WriteLine("private static {0} {1}={2}", pt, fin, fi.GetValue(null)); 
                }
            }
       }
        
        public static string UID { get; set; }

        public static string SAPPI_UID { get; set; }
        public static string SAPPI_PWD { get; set; }
        public static string CICO_UID { get; set; }
        public static string CICO_PWD { get; set; }
        public static string SAP_NAME { get; set; }
        public static string SAP_CLIENT { get; set; }
        public static string SAP_LANG { get; set; }
        public static string SAP_ASHOST { get; set; }
        public static string SAP_SYSN { get; set; }
        public static string SAP_MAX_POOL_SIZE { get; set; }
        public static string SAP_IDLE_TIMEOUT { get; set; }

        public static string LdapPath { get; set; }
        public static string LdapDomain { get; set; }

        [DefaultSettingValue("toyota")]
        public static string Domain { get; set; }

        public static string ApplicationID { get; set; }

        [DefaultSettingValue("DB")]
        public static string DB { get; set; }

        [DefaultSettingValue("0")]
        public static int DEBUG
        {
            get;
            set;
        }

        public static int ReadInt(string setting, int value = 0)
        {
            string b = ConfigurationManager.AppSettings[setting];
            if (string.IsNullOrEmpty(b))
                return value;
            else
            {
                int x = value;
                if (Int32.TryParse(b, out x))
                    return x;
                else
                    return value;
            }
        }

        public static string Read(string setting, string value = "")
        {
            string b = ConfigurationManager.AppSettings[setting];
            if (string.IsNullOrEmpty(b))
                return value;
            else
            {
                return b;
            }
        }
    }
}