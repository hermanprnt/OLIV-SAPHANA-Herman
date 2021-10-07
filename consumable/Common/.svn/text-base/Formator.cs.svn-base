using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Diagnostics;

using System.Reflection;

namespace consumable.Common
{
    public static class Formator
    {
        public static string Prop(object o, string allowedFields=null)
        {
            int allowed = 0;
            List<string> allow = ListFrom(allowedFields);
            allowed = allow.Count;

            string r = string.Join(", ",
                o.GetType()
                .GetProperties()
                .Where(pi => pi.CanRead && pi.GetGetMethod() != null 
                     && (allowed < 1 || allow.IndexOf(pi.Name) >= 0 ) )
                .Select(pi => pi.Name + ": " + pi.GetGetMethod().Invoke(o, null).OVal() ))
                ;
            return r;
        }

        public static string OVal(this object o)
        {
            if (o == null) return "";

            Type t = o.GetType();
            Debug.WriteLine(t.Name);

            if (t.Name.Equals("DateTime"))
                return ((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss");
            else
                return Convert.ToString(o);
        }

        public static List<string> Props<T>(IEnumerable<T> l) 
        {
            T o = l.FirstOrDefault();
            List<string> r = new List<string>();
            r.Add(
                string.Join(", ",
                    o.GetType()
                    .GetProperties()
                    .Where(pi => pi.CanRead && pi.GetGetMethod() != null)
                    .Select(pi => pi.Name))
            );
            return r;
        }

        public static string PropList<T>(List<T> l, string allowedFields = null)
        {
            List<string> r = new List<string>();
            
            List<string> allow = ListFrom(allowedFields);
            int allowed = 0;
            allowed = allow.Count; 

            if (!string.IsNullOrEmpty(allowedFields))
            {
                allow = allowedFields.Split(',').ToList();
                allowed = allow.Count;
            }
            foreach (T o in l)
            {
                r.Add( 
                    string.Join(", ",
                        o.GetType()
                        .GetProperties()
                        .Where(pi => pi.CanRead && pi.GetGetMethod() != null 
                            && (allowed < 1 || allow.IndexOf(pi.Name) >= 0))
                .Select(pi => pi.Name + ": " + pi.GetGetMethod().Invoke(o, null).OVal() ))
                );
            }
            return string.Join("\r\n", r);
        }

        public static List<string>ListFrom(string list) 
        {
            List<string> allow = new List<string>();
            if (!string.IsNullOrEmpty(list))
                allow = list.Split(',').ToList();
            return allow;
        }

        public static object[][] ArrayOf<T>(IEnumerable<T> l, string allowedFields = null)  {
            List<string> allow = ListFrom(allowedFields); 
            int allowed = 0;
            allowed = allow.Count;
            List<List<object>> a = new List<List<object>>();
            var f = l.FirstOrDefault();
            
            List<int> allowi = new List<int>();
            PropertyInfo[] propinfo = null;
            if (f != null)
            {
                propinfo = f.GetType().GetProperties();
                if (allowed == 0)
                {
                    for (int j = 0; j < propinfo.Length; j++)
                    {
                        if (propinfo[j].CanRead && propinfo[j].GetGetMethod() != null)
                            allowi.Add(j);
                    }
                }
                else 
                    for (int i = 0; i < allow.Count; i++)
                    {
                        int ix = -1;
                        for (int j = 0; j < propinfo.Length; j++)
                        {
                            if (propinfo[j].CanRead && propinfo[j].GetGetMethod() != null)
                                if (string.Compare(propinfo[j].Name, allow[i]) == 0)
                                {
                                    ix = j;
                                    break;
                                }

                        }
                        allowi.Add(ix);
                    }
            }
            foreach (T e in l)
            {

                List<object> ol = new List<object>();
                for (int i = 0; i < allowi.Count; i++) {
                    if (allowi[i] >= 0)
                    {
                        ol.Add(propinfo[allowi[i]].GetValue(e, null));
                    }
                    else
                        ol.Add(null);

                }
                a.Add(ol);
            }
            return a.Select(x => x.ToArray()).ToArray();
        }
    }
}