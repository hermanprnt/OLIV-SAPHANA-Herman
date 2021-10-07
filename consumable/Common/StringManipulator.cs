using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace consumable.Common
{
    /// <summary>
    /// static class extension methods for string 
    /// </summary>
    public static class StringManipulator
    {
        /* take string x and Multiply it n times */
        public static string Mul(this string x, int n)
        {
            StringBuilder s = new StringBuilder("");
            for (int i = 0; i < n; i++)
            {
                s.Append(x);
            }
            return s.ToString();
        }

        public static string CleanNameOf(this string fn)
        {
            while (!string.IsNullOrEmpty(fn))
            {
                int lastDot = fn.LastIndexOf('.');

                if (lastDot > 0)
                    fn = fn.Substring(0, lastDot);
                else
                    break;
            }
            return fn;
        }

        //public static bool isEmpty(this string s) /// already in StrTo
        //{
        //    return string.IsNullOrEmpty(s);
        //}

        public static List<string> explode(this string s, string separator = ";")
        {
            List<string> b = new List<string>();
            Regex x = new Regex("([\"]([^\"]+)[\"]|([^" + separator + "]+)|)[\\" + separator + "]?");
            MatchCollection c = x.Matches(s);
            if (c.Count > 0)
            {
                for (int i = 0; i < c.Count; i++)
                {
                    if (c[i].Groups.Count > 1 && c[i].Groups[2].Value.Length > 0)
                        b.Add(c[i].Groups[2].Value);
                    else if (c[i].Groups.Count > 0 && c[i].Groups[1].Value.Length > 0)
                        b.Add(c[i].Groups[1].Value);
                    else
                        b.Add("");
                }
            }
            return b;
        }

        public static string YMD(this string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Length > 9 && s.IndexOf(".") == 2 && s.LastIndexOf(".") == 5)
            {
                string t = s.Substring(6, 4) + s.Substring(3, 2) + s.Substring(0, 2);
                return t;
            }
            else
                return "";
        }

        public static string UnQuote(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string a = s.Substring(0, 1);
            string b = s.Substring(s.Length - 1, 1);
            if (s.Length > 2 && a.Equals(b)
                && (a.Equals("\"") || a.Equals("'") || a.Equals("`"))
                )
                s = s.Substring(1, s.Length - 2);
            else if (s.Length > 2 && a.Equals("'"))
                s = s.Substring(1, s.Length - 1);
            else if (a.Equals("="))
            {
                s = UnQuote(s.Substring(1, s.Length - 1));
            }
            return s;
        }

    } // class StringManipulator
}
