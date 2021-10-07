using System;
using System.Collections.Generic;
using System.Text;
using consumable.Common;
using SAP.Middleware.Connector;

namespace consumable.Logic
{
    public class LogHelper
    {
        private IPut l = null;
        private string Loc;

        private string sep = "\r\n\t";
        private string fsep = ": ";
        private string colsep = "\t";
        private int pid = 0;
        private string uid = "";

        private readonly char[] colSplits = new char[] { '|', ',', ';', ' ', '\r', '\n' };

        private string[][] cols;
        private string[][] fill;
        // stores object names as 'key' 
        private List<string> d;

        public List<string> Keys
        {
            get
            {
                return d.Clone();
            }
        }

        public string ColumnSeparator
        {
            get { return colsep;  }
            set { colsep = value; }
        }
        

        public StringIndexer Columns;
        

        public LogHelper(IPut logger, string Location,
                string[] def,
                string UserName = "", int ProcessId = 0)
        {
            l = logger;
            Loc = Location;

            uid = UserName;
            pid = ProcessId;

            d = new List<string>();

            int n = def.Length / 2;
            cols = new string[n][];
            fill = new string[n][];

            int j = 0;
            for (int i = 0; i < (int)(def.Length / 2); i++)
            {
                string k = def[i * 2];
                string v = def[i * 2 + 1];

                d.Add(k);
                cols[j] = v.Split(colSplits, StringSplitOptions.RemoveEmptyEntries);
                fill[j] = new string[cols[j].Length];
                j++;
            }

            Columns = new StringIndexer(cols, d);
        }

        public string Separator
        {
            get { return sep; }
            set { sep = value; }
        }

        private int SelectedValue = -1;
        public string Selected
        {
            get
            {
                if (SelectedValue >= 0 && SelectedValue < d.Count)
                    return d[SelectedValue];
                else
                    return null;
            }
            set
            {
                SelectedValue = d.IndexOf(value);
            }
        }


        private string SetFill(string index, string value)
        {
            if (SelectedValue >= 0 && SelectedValue < d.Count)
            {
                int k = -1;
                for (int i = 0; i < cols[SelectedValue].Length; i++)
                {
                    if (cols[SelectedValue][i].ToLower().Equals(index.ToLower()))
                    {
                        k = i;
                        break;
                    }
                }
                if (k >= 0)
                {
                    if (value != null)
                        fill[SelectedValue][k] = value;
                    return fill[SelectedValue][k];
                }
                else
                    return null;
            }
            else
                return null;
        }

        public string this[string index]
        {
            get
            {
                return SetFill(index, null);
            }

            set
            {
                SetFill(index, value);
            }
        }

        ///  returns data row fields specified by key         
        public string Row(string Key)
        {
            StringBuilder b = new StringBuilder();
            int ki = d.IndexOf(Key);
            if (ki >= 0)
            {
                for (int i = 0; i < fill[ki].Length; i++)
                {
                    if (i > 0) b.Append(colsep);
                    b = b.Append(fill[ki][i]);
                }
            }
            return b.ToString();
        }

        // returns text inside fill when "x" empty titled with "Title" by column specified in "Key" 
        // in format separated by 'sep' 
        public string Text(string Key, string Title, params object[] x)
        {
            string[] col = null;
            int n = d.IndexOf(Key);
            if (n >= 0)
            {
                col = cols[n];
            }
            else
            {
                return null;
            }
            StringBuilder t = new StringBuilder(Title.isEmpty() ? Key : Title);
            t.Append(sep);
            if (x != null && x.Length > 0)
            {
                int j = Math.Min(x.Length, col.Length);
                for (int i = 0; i < j; ++i)
                {
                    t.Append((i > 0) ? sep : "");
                    t.Append(col[i]);
                    t.Append(fsep);
                    t.Append(x[i].str());
                }
            }
            else
            {
                int j = Math.Min(fill[n].Length, col.Length);
                for (int i = 0; i < j; ++i)
                {
                    t.Append((i > 0) ? sep : "");
                    t.Append(col[i]);
                    t.Append(fsep);
                    t.Append(fill[n][i]);
                }
            }
            return t.ToString();
        }

        public void Put(string Key, params object[] x)
        {
            Log(Key, Key, x);
        }

        public void Log(string Key, string Title, params object[] x)
        {
            pid = l.Put(Text(Key, Title, x), uid, Loc, pid, "", "", 0, "MSTD00001INF");
        }

        public bool Set(IRfcDataContainer x, string Key, params string[] z)
        {
            string[] col = null;
            int n = d.IndexOf(Key);
            if (n >= 0)
            {
                col = cols[n];
            }
            else
            {
                return false;
            }

            if (x != null && z != null && z.Length > 0)
            {
                int j = Math.Min(z.Length, col.Length);
                ISay so = Sing.Get<ISay>();
                so.Say("sap", "{0}", Key);
                for (int i = 0; i < j; i++)
                {
                    fill[n][i] = z[i];
                    so.Say("sap", "\t{0}: {1}", col[i], z[i]);
                    x.SetValue(col[i], z[i]);
                }
                so.Say("sap", "", "");
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool Get(IRfcDataContainer x, string Key)
        {
            string[] col = null;
            string[] f = null;
            int n = d.IndexOf(Key);
            if (n >= 0)
            {
                col = cols[n];

                f = fill[n];

                SelectedValue = n;
            }
            else
            {
                SelectedValue = -1;
                return false;
            }
            string colname = "";
            try
            {
                for (int i = 0; i < col.Length; i++)
                {
                    colname = col[i];
                    f[i] = x.GetString(colname);
                }
            }
            catch (Exception ex)
            {
                pid = l.Put("\"" + colname + "\" not found\r\n" + ex.Trace(), "err", "Help", pid,"","",0, "MSTD00002ERR");
                return false;
            }

            return true;
        }

    }

    public class StringIndexer
    {
        private string[][] cx = null;
        private List<string> tx = null;
        public StringIndexer(string[][] cols, List<string> tabs)
        {
            cx = cols;
            tx = tabs;
        }
        
        public string[] this[string Index]
        {
            get
            {
                int i = -1;
                if (tx != null)
                {
                    i = tx.IndexOf(Index);
                }
                if (i >= 0)
                    return cx[i];
                else 
                    return null;
            }
        }
    }

}
