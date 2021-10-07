using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics; 

namespace consumable.Common
{
    public class XmlConfigReader : IConfigReader
    {
        public SimpleTree t = new SimpleTree() { Text = "", ID = 0};

        public int IntVal(string key, int defaultValue = 0)
        {
            string x = t.GetText(key);
            int r = defaultValue;

            if (!string.IsNullOrEmpty(x) && int.TryParse(x, out r))
            {
                return r;
            }
            else
                return defaultValue;
        }

        public string Val(string key, string defaultValue = "")
        {
            string x = t.GetText(key);
            if (string.IsNullOrEmpty(x))
                return defaultValue;
            else
                return x;
        }

        public string Get(string key)
        {
            return t.GetText(key);
        }

        public string Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool Read(string filename)
        {
            if (File.Exists(filename))
            {
                XmlTextReader x = new XmlTextReader(filename);
                Stack<SimpleTree> s = new Stack<SimpleTree>();
                string[] keyAttribute = "key;name;assembly;type".Split(new char[] { ';' });
                string[] valueAttribute = "value;connectionString".Split(new char[] { ';' });
                int LastDepth = -1; /// indeterminate depth
                while (x.Read())
                {
                    switch (x.NodeType)
                    {
                        case XmlNodeType.Element:
                            SimpleTree tx = null;
                            if (s.Count < 1 && t.GetMax() == 0)
                            {
                                t.Text = x.Name;
                                t.ID = 0;
                                t.Prev = null;
                                t.Next = null;
                                s.Push(t);
                                LastDepth = 0;
                                tx = t;

                            }
                            else
                            {
                                tx = s.Peek();
                                if (LastDepth < x.Depth)
                                {
                                    LastDepth = x.Depth;
                                    SimpleTree txa = tx.Mul(x.Name);

                                    s.Push(txa);
                                    tx = txa;
                                }
                                else
                                {
                                    SimpleTree z = tx.Add(x.Name);
                                    s.Pop();
                                    s.Push(z);
                                    tx = z;
                                }
                            }


                            if (x.HasAttributes)
                            {
                                x.MoveToFirstAttribute();
                                for (int i = 0; i < x.AttributeCount; i++)
                                {
                                    if (keyAttribute.Contains(x.Name.ToLower()))
                                    {
                                        tx.Text = x.Value;
                                    }
                                    else if (valueAttribute.Contains(x.Name.ToLower()))
                                    {
                                        tx.Mul(x.Value);
                                    }
                                    else
                                    {
                                        SimpleTree txx = tx.Mul(x.Name);
                                        txx.Mul(x.Value);
                                    }
                                    x.MoveToNextAttribute();
                                }
                            }
                            // begin 
                            break;
                        case XmlNodeType.Text:
                            // fill
                            if (s.Count > 0)
                            {
                                SimpleTree txt = s.Peek();
                                txt.Mul(x.Value);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (LastDepth > x.Depth && s.Count > 0)
                            {
                                s.Pop();
                                LastDepth = x.Depth;
                            }
                            else
                            {

                            }
                            break;

                        case XmlNodeType.Attribute:
                            Debug.WriteLine("{0}= {1}", x.Name, x.Value);

                            break;

                        /// list of ignored elements 
                        case XmlNodeType.Comment:
                        case XmlNodeType.Whitespace:
                            break;

                        default:
                            break;
                    }
                }

                return true;
            }
            return false;
        }


        public string this[string index]
        {
            get
            {
                string v = t.GetText(index, true);
                return v;
            }
        }

    }
}
