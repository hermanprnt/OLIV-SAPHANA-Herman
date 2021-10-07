using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using consumable.Common;
using System.Dynamic;
using System.Collections;
using Dapper; 

namespace consumable.Logic
{
    public class TableDef
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Def { get; set; }

        public void Add(string k, string v)
        {
            KV.Add(k, v);
        }

        public Dictionary<string, string> KV = new Dictionary<string, string>();
        public void AddRow()
        {
            if (KV.Count > 0)
            {
                object[] rowdata = new object[Cols.Count];
                for (int i = 0; i < Cols.Count; i++)
                {
                    rowdata[i] = null;
                    if (!KV.ContainsKey(Cols[i].FieldName))
                    {
                        Console.WriteLine("no " + Cols[i].FieldName + " defined");
                        continue;
                    }
                    string v = KV[Cols[i].FieldName];

                    switch (Cols[i].FieldType)
                    {
                        case "D":
                            DateTime dx = v.Date();
                            if (dx != StrTo.NULL_DATE)
                                rowdata[i] = dx;
                            break;
                        case "N":
                            if (!v.isEmpty())
                                rowdata[i] = v.Dec();
                            break;
                        case "I":
                            if (!v.isEmpty())
                                rowdata[i] = v.Int();
                            break;
                        case "L":
                            if (!v.isEmpty())
                                rowdata[i] = v.Long();
                            break;
                        case "B":
                            if (!v.isEmpty())
                                rowdata[i] = v.Bool();
                            break;
                        default:
                            rowdata[i] = v;
                            break;
                    }
                }
                DataRow r = Table.Rows.Add(rowdata);
            }
            KV.Clear();
        }

        public List<ColumnDef> Cols = new List<ColumnDef>();
        public DataTable Table { get; set; }

        public DataTable MakeTable(string TableDef, string tableName)
        {
            DataTable t = new DataTable(tableName);

            string[] cols = TableDef.Split(new string[] { "\r\n", "|" }, StringSplitOptions.None);
            Cols.Clear();
            List<ColumnDef> cd = new List<ColumnDef>();
            foreach (string col in cols)
            {
                string[] co = col.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (co != null && co.Length > 0)
                {
                    string fieldName = co[0];

                    string dt = "C";
                    if (co.Length > 1)
                        dt = co[1];
                    Type tx = StringType(dt);

                    t.Columns.Add(fieldName, tx);
                    Cols.Add(new ColumnDef() { FieldName = fieldName, FieldType = dt });
                }
            }
            return t;
        }

        public static Type StringType(string dt)
        {
            if (dt.Length > 1)
                dt = dt.Substring(0, 1);

            Type tx = typeof(string);

            switch (dt)
            {
                case "C":
                    tx = typeof(string);
                    break;
                case "D":
                    tx = typeof(DateTime);
                    break;
                case "N":
                    tx = typeof(decimal);
                    break;
                case "I":
                    tx = typeof(Int32);
                    break;
                case "L":
                    tx = typeof(long);
                    break;
                case "B":
                    tx = typeof(bool);
                    break;
                default:
                    tx = typeof(string);
                    break;

            }
            return tx;
        }


        public static string TypeString(Type t)
        {
            string r = "C";
            switch (t.Name)
            {
                case "DateTime": r = "D"; break;
                case "decimal": r = "N"; break;
                case "Int32": r = "I"; break;
                case "long": r = "L"; break;
                case "bool": r = "B"; break;
                default:
                    break;
            }
            return r;
        }

        public string TableAsText(string separator = "\t", bool header = true)
        {
            StringBuilder b = new StringBuilder("");
            if (header)
            {
                for (int i = 0; i < Table.Columns.Count; i++)
                {
                    if (i > 0)
                        b.Append(separator);
                    b.Append(Table.Columns[i].ColumnName);
                }
                b.Append("\r\n");
            }
            int n = Table.Columns.Count;
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                DataRow r = Table.Rows[i];

                for (int j = 0; j < n; j++)
                {
                    if (j > 0)
                        b.Append(separator);
                    b.Append(r[j]);
                }
            }
            return b.ToString();
        }

        public void Save(string sql=null)
        {
            if (string.IsNullOrEmpty(sql))
                sql = "Put" + Name;

            if (!string.IsNullOrEmpty(Sing.Me.SQL[sql]))
            {
                string pid = DateTime.Now.ToString("yyyyMMddHHmmss");
                Sing.Me.Text.Say("DB", "{0}", sql);
                int seq = 0;
                for (int i = 0; i < Table.Rows.Count; i++)
                {
                    DataRow r = Table.Rows[i];
                    // IDictionary<string, object> d = new Dictionary<string, object>();

                    DynamicParameters d = new DynamicParameters();
                    StringBuilder B = new StringBuilder("");
                    int iq = 0; 

                    foreach (ColumnDef c in Cols)
                    {
                        if (iq > 0) B.Append(",");
                        B.AppendFormat("{0}:{1}", c.FieldName, r[c.FieldName]);
                        if (!r[c.FieldName].Equals(System.DBNull.Value))
                            d.Add(c.FieldName, r[c.FieldName]);
                        else
                            d.Add(c.FieldName, null);
                        ++iq;
                        // d.Add(c.FieldName, r[c.FieldName]); 
                    }
                    ++ seq;
                    B.AppendFormat("PID:{0}, SEQ:{1}", pid, seq);
                    d.Add("PID", pid);
                    d.Add("SEQ", seq);
                    int rowcount = 0;
                    try
                    {
                        Sing.Me.Text.Say("DB", "{0}", B.ToString());
                        rowcount = Sing.Me.Do(sql, d);
                    }
                    catch (Exception ex)
                    {
                        Sing.Me.Text.Say("DB", "{0}", ex.Trace());
                    }
                }
                
            }
        }

        public string DynValues(IDictionary<string, object> d)
        {
            StringBuilder b = new StringBuilder("");
            int i = 0;
            foreach (string k in d.Keys)
            {
                if (i > 0)
                    b.Append(",");
                b.AppendFormat("{0}:{1}", k, d[k]);
                i ++;
            }
            return b.ToString();
        }

        public int PutObject(string sql, IDictionary<string, object> d)
        {
            int rowcount = 0;
            
            Sing.Me.Text.Say("DB", "{0} {1}", sql, DynValues(d));
            try
            {
                rowcount = Sing.Me.Do(sql, Expando(d));
            }
            catch (Exception ex)
            {
                Sing.Me.Text.Say("DB", "{0}", ex.Trace());
            }
            return rowcount;
        }


        public static ExpandoObject Expando(IDictionary<string, object>  
            dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;
            foreach (var item in dictionary)
            {
                expandoDic.Add(item);
            }
            return expando;
        }

    }
}
