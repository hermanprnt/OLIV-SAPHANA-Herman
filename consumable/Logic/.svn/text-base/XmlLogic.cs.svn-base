using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using consumable.Common;
using System.Data;
using System.Text.RegularExpressions;

namespace consumable.Logic
{
    public class XmlLogic
    {
        public static int TAB_SPACE = 4;
        public static string STARTE = "{";
        public static string HAS = ":";
        public static string NUL = "null";
        public static string SEP = ",";
        public static string ENDE = "}";
        public static string EOL = "\r\n";
        public static string PRE_DEBUG = "#\t\t\t\t";
        public static string QUOT = "\"";

        public LinkedList<Entry> Pile = new LinkedList<Entry>();
        public SimpleTree Root = new SimpleTree("/", 0, null);

        public List<TableDef> db = new List<TableDef>();
        public TableDef CurrentTable = null;
        public int iTable = -1;

        public bool PrefixLog { get; set; }

        public static string TABS(int n) 
        {
            return (new string(' ', n * TAB_SPACE));
        }

        public event EventHandler OnNew;
        public event EventHandler OnEnd;
        public event EventHandler OnBit;
        //public event EventHandler OnCollect;
        public event Log OnLog;

        public XmlLogic()
        {
            PrefixLog = false;
        }

        public string LogPrefix
        {
            get
            {
                return ((PrefixLog || Debug != 0) ? (PRE_DEBUG) : "");
            }
            
        }

        public void say(string w, params object[] x)
        {
            if (OnLog != null) 
                OnLog(w, x);

            if (Debug != 0) { 
                debugLogs.AppendFormat(w, x);
                debugLogs.AppendLine();
            }
        }

        public void log(string w, params object[] x)
        {
            if (Debug != 0 && OnLog != null) 
                OnLog(w, x);

            if (Debug != 0)
            {
                debugLogs.AppendFormat(LogPrefix + w, x);
                debugLogs.AppendLine();
            }
        }

        public void Ende()
        {
            string currentPath = CurrentPath;
            log("OnEnd " + currentPath);
            TableDef t = TableFromPath(currentPath);
            if (t != null)
            {
                CurrentTable = t;
                t.AddRow();
                iTable = db.IndexOf(t);
            }
            
            if (OnEnd != null)
            {
                OnEnd(this, null);
            }
        }

        public void Bite()
        {
            string currentPath = CurrentPath;
            log("OnBit " + currentPath);

            if (iTable >= 0)
            {
                CurrentTable.Add(Key, Val);
            }

            if (OnBit != null)
            {
                OnBit(this, null);
            }
        }

        public void Newe()
        {
            log("OnNew " + CurrentPath);

            TableDef t = TableFromPath(CurrentPath);
            if (t != null)
            {
                CurrentTable = t;
                iTable = db.IndexOf(t);
            }
            if (OnNew != null)
            {                
                OnNew(this, null);
            }
        }

        public string CurrentPath
        {
            get
            {
                StringBuilder b = new StringBuilder("");
                for (int i = 0; i < Pile.Count; i++)
                {
                    if (i> 0) 
                        b.Append("/");
                    b.Append(Pile.ElementAt(i).Text);
                }
                return b.ToString();
            }
        }

        public int CurrentLevel
        {
            get
            {
                return (Pile.Count > 0) ? Pile.Last().Level : -1;
            }
        }

        public string CurrentNode
        {
            get
            {
                return (Pile.Count > 0)? Pile.Last().Text: null;                
            }
        }

        public int NextSeq 
        {
            get
            {
                ++seq;
                return seq;                
            }
        }
        private string template = null;
     
        public string TemplateName 
        {
            get 
            {
                return template;
            }
            set 
            {
                template = value;
                SetTemplate(template);
            }
        }
        public int tab = 0;
        public int lastTab = 0;
        public int openTab = 0;
        public int seq = 0;
        public string Key = null;
        public string Val = null;
        public int Debug = 0;
        public StringBuilder debugLogs = new StringBuilder("");

        public string Read(string filename)
        {
            StringBuilder b = new StringBuilder("");
            debugLogs = new StringBuilder("");
            if (!File.Exists(filename))
            {
                say("file {0} not found", filename);
                return null;
            }
            FileStream f = new FileStream(filename, FileMode.Open);
            tab = 0;    
            lastTab = 0;
            openTab = 0;
            seq = 1;
            XmlNodeType lastNodeType = XmlNodeType.Whitespace;
            string nodeType = "";

            StringBuilder x = new StringBuilder("");
            StringBuilder attr = new StringBuilder("");
            int pendingSEP = 0;
            XmlReader r = XmlReader.Create(f);
            
            seq = 0;
            while (r.Read())
            {
                bool isStart = r.IsStartElement();
                x.Clear();
                if (pendingSEP > 0 && r.NodeType != XmlNodeType.EndElement)
                {
                    x.Append(SEP);
                    pendingSEP = 0;
                }
                
                switch (r.NodeType)
                {
                    case XmlNodeType.Element:
                        nodeType = "Element";
                        if (lastNodeType == XmlNodeType.EndElement && isStart)
                        {
                            x.Append(SEP);
                        }
                        if (openTab > 0)
                        {
                            x.Append(EOL);
                            x.Append(TABS(openTab-1));
                            x.Append(STARTE);
                            

                            if (attr.Length > 0)
                            {
                                x.Append(EOL);
                                x.Append(TABS(openTab));
                                x.Append(attr.ToString());
                                
                                attr.Clear();

                                if (isStart)
                                {
                                    x.Append(SEP);
                                }
                            }
                            openTab = 0;
                        }

                        x.Append(EOL);

                        if (tab > 0)
                        {
                            x.Append(TABS(tab));
                        }
                        string NodeName = CleanNodeName(r.Name);

                        x.Append(NodeName);
                        Key = NodeName;
                        Pile.AddLast(new Entry() { id = NextSeq, Text = NodeName, Level = tab });
                        DebugPile(); 
                        if (r.IsEmptyElement)
                        {
                            x.Append(HAS + NUL);
                            Val = null;
                            Bite();
                            pendingSEP = 1;
                            log("K=V {0}={1}", Key, Val);
                        }
                        else
                        {
                            x.Append(HAS);                            
                            Newe();
                        }

                        if (isStart && !r.IsEmptyElement)
                        {
                            ++tab;
                            openTab = tab;
                        }
                        break;

                    case XmlNodeType.Text:
                        nodeType = "Text";
                        Val = r.Value;
                        x.Append(QUOT);
                        x.Append(r.Value);
                        x.Append(QUOT);
                        openTab = 0;

                        break;

                    case XmlNodeType.EndElement:
                        nodeType = "EndElement";

                        if (tab > 0 && (lastNodeType != XmlNodeType.Text))
                        {
                            x.Append(EOL);
                            x.Append(TABS(tab-1));
                            x.Append(ENDE);
                        }

                        if (lastNodeType == XmlNodeType.Text)
                        {
                            /// direct KV                            
                            Bite();
                            Pile.RemoveLast();
                            log("RemoveLast() endtext");
                            log("K=V {0}={1}", Key, Val);
                        }

                        pendingSEP = 0;

                        --tab;
                        break;
                    default:
                        nodeType = r.NodeType.ToString();
                        break;
                }
                
                log("{0} tab:{1} lvl:{2} {3}", nodeType, tab, CurrentLevel, (r.HasAttributes) ? " *" : "");
                if (tab <= CurrentLevel)
                {
                    Pile.RemoveLast();
                    log("RemoveLast()");

                    Ende();
                }
                DebugPile();               
                
                if (r.HasAttributes)
                {
                    int ia = 0;
                    while (r.MoveToNextAttribute())
                    {
                        if (ia > 0)
                            attr.Append(SEP + " ");
                        attr.AppendFormat("{0}:\"{1}\"", CleanNodeName(r.Name), r.Value);
                        ++ia;
                    }
                    /// pendingSEP = 1;
                }
                b.Append(x);

                debugLogs.Append(EOL);
                debugLogs.AppendLine(x.ToString());                

                lastTab = tab;
                lastNodeType = r.NodeType;
            }
            f.Close();
            return ((Debug != 0) ? debugLogs.ToString() : b.ToString());
        }

        public static string CleanNodeName(string name)
        {
            string r = name;
            if (!string.IsNullOrEmpty(r))
            {
                int colonPos = r.IndexOf(':');
                if (colonPos > 0)
                {
                    r = r.Substring(colonPos + 1, r.Length - colonPos - 1);
                }
                r = r.Replace(SEP, "").Replace(HAS, ".");
            }
            return r;
        }

        public void DebugPile()
        {
            if (Debug != 0)
            {
                ShowPile(); 
            }
        }

        public void ShowPile()
        {
            debugLogs.AppendFormat(LogPrefix +  "id\tlv\tText" + EOL);
            foreach (Entry e in Pile)
            {
                debugLogs.AppendFormat(LogPrefix + "{0,2}\t{1,2}\t{2}" + EOL, e.id, e.Level, e.Text);
            }
        }

        public void SetTemplate(string templateFileName)
        {
            string t = null;
            if (File.Exists(templateFileName))
            {
                t = File.ReadAllText(templateFileName);
            }
            else if (!templateFileName.Contains("\\")) 
            {
                t = Argu.GetResource(templateFileName);
            }
            if (t != null) 
            {
                template = templateFileName;
                Parse(t);
                MakeStructure();
            }
        }

        public void MakeStructure()
        {
            if (Root.GetMax() > 1)
            {
                db.Clear();
                SimpleTree a = Root.Go("/Tables");
                a = a.Child;

                while (a != null)
                {
                    string def = ((a.Child !=null) ? a.Child.Item as string : null);
                    db.Add(new TableDef() 
                        { 
                            Name = a.Text, 
                            Path = a.Item as string, 
                            Def= def
                        });

                    a = a.Next;
                }

                foreach (TableDef d in db)
                {
                    if (d.Def != null)
                        d.Table = d.MakeTable(d.Def, d.Name);
                }
                
            }
        }

        
        /**
         * parsing Metadata 
         * structured as : 
         * # any lines begins with #, /, * are comments 
         * @ table name begins wiith @ followed by path its resides on 
         * eq: 
         * @SIMPLE_TABLE /simple/table
         *    ID L  
         *    NAME C
         *    CREATED D 
         *    ACTIVE B 
         *    COUNTER N
         *    TAG I 
         
         * simple definition of a table followed by item 
         
        **/
        public int Parse(string MetaData)
        {
            int ret = -1;

            if (string.IsNullOrEmpty(MetaData))
                return ret;

            SimpleTree tx = Root.Mul("Tables", 0);

            log("mul Tables ID={0}", tx.ID);


            string[] l = MetaData.Split(new string[] { "\r\n", "|" },
                    StringSplitOptions.RemoveEmptyEntries);


            Regex rxTable = new Regex("(@)([\\w\\d]+)\\s+([\\w/]+)");

            // bool hasRange = false;
            SimpleTree tabul = null;
            StringBuilder item = new StringBuilder("");
            int blanks = 0;

            for (int i = 0; i < l.Length; i++)
            {
                string s = l[i].Trim();

                // ignore comments 
                if (s.IndexOfAny(new char[] { '#', '/', '*' }) == 0)
                    continue;

                if (s.isEmpty())
                {
                    blanks++;
                    if (blanks > 1)
                    {
                        log("blank add");
                        AddItemToTableDef(tabul, item);
                        tabul = null;
                    }
                    continue;
                }

                // match Page

                MatchCollection xPage = rxTable.Matches(s);

                if (xPage.Count > 0 && xPage[0].Groups.Count > 0 && tabul != null)
                {
                    log("pre add last table");
                    AddItemToTableDef(tabul, item);
                    tabul = null;
                }

                if (tabul == null)
                {
                    if (xPage.Count > 0 && xPage[0].Groups.Count > 0)
                    {
                        string TableName = xPage[0].Groups[2].Value;
                        string TablePath = xPage[0].Groups[3].Value;

                        log("Match Table def \"{0}\" @{1}", TableName, TablePath);
                        tabul = tx.Mul(TableName);

                        log("mul {0} ID={1}", TableName, tabul.ID);
                        tabul.Item = TablePath;

                        SimpleTree m = tabul.Mul("Def");
                        log("mul {0} ID={1}", m.Text, m.ID);
                        continue;
                    }
                }
                item.AppendLine(s);
            }

            if (tabul != null)
            {
                log("last add item");
                AddItemToTableDef(tabul, item);
            }

            return ret;
        }

        public void AddItemToTableDef(SimpleTree tabul, StringBuilder b)
        {
            if (tabul == null)
            {
                log("No Table yet");
                return;
            }
            SimpleTree d = tabul.Go("Def");
            if (d == null)
            {
                log( "Def not found @ #{0} {1}", tabul.ID, tabul.Text);
                return;
            }
            log("Add item to {0} {1}", tabul.ID, tabul.Text);
            d.Item = b.ToString();
            b.Clear();
        }

        public TableDef TableFromPath(string path)
        {
            TableDef r = null; 
            foreach (var t in db)
            {
                if (string.Compare(path, t.Path, true) == 0)
                {
                    r = t;
                    break;
                }
             }
            return (r);            
        }

        public TableDef TableFromName(string name)
        {
            TableDef r = null;
            foreach (var t in db)
            {
                if (string.Compare(t.Name,  name, true) == 0)
                {
                    r = t;
                    break;
                }
            }
            return (r);            
        }

        public List<string> Allowed = "set,db,tree,read,help,sql"
                .Split(new string[] { "," }, StringSplitOptions.None)
                .ToList();

        public SimpleTree ActiveNode = null;

        public static List<string> Actions = Enum.GetNames(typeof(XmlAct)).Select(a => a.ToLower()).ToList();
        public static int[] ActionNum = Enum.GetValues(typeof(XmlAct)).Cast<int>().ToArray();
        

        /// <summary>
        ///  refer to Hilfe.txt for commands and parameters
        /// </summary>
        /// <param name="cmd"></param>
        public void Do(string cmd = null)
        {
            int iAct;
            int iCmd = 0;
            try
            {
                SimpleTree t = ActiveNode;

                if (cmd.isEmpty())
                    return;
                
                string[] c = cmd.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (c.Length < 1)
                    return;
                
                iAct = -1;

                XmlAct xAct = XmlAct.Help;

                int j = 0;    
                while (iAct < 0 && j < Actions.Count)
                {
                    if (Actions[j].Equals(c[iCmd].ToLower()))
                    {
                        iAct = ActionNum[j];
                        break;
                    }
                    ++j;
                }

                if (iAct > 0 && Enum.IsDefined(typeof(XmlAct), ActionNum[iAct]))
                    xAct= (XmlAct) ActionNum[iAct];

                switch (xAct)
                {
                    case XmlAct.Set:
                        /// set templatefilename 
                        if (c.Length > iCmd+1)
                            SetTemplate(c[iCmd + 1]);
                        else
                            say("set <filename>");
                        break;
                    case XmlAct.Db:
                        // db <tablename> [def|list]
                        // db -- list all tables
                        // db <tablename> list  
                        if (c.Length > iCmd + 1)
                        {
                            TableDef td = TableFromName(c[iCmd + 1]);

                            if (td == null)
                                say(c[iCmd + 1] + " not found");
                            else
                            {
                                if ((c.Length > iCmd + 2 && c[iCmd + 2].ToLower().Equals("list")))
                                {
                                    say(td.TableAsText());
                                }
                                else if ((c.Length >= iCmd + 3) && (c[iCmd+2].ToLower().Equals("save")))
                                {
                                    string sql = (c.Length > 3) ? c[iCmd + 3] : null;
                                    td.Save(sql);
                                }
                                else
                                {
                                    say("@{0} {1}", td.Name, td.Path);
                                    foreach (ColumnDef co in td.Cols)
                                    {
                                        say("\t{0}\t{1}", co.FieldName, co.FieldType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (db.Count < 1)
                                say("no tables");
                            else
                                say(
                                    string.Join("\r\n", db.Select(a => a.Name).ToList())
                                );
                        }

                        break;

                    case XmlAct.Tree:
                        if (t == null)
                        {
                            t = Root;
                        }
                        if (c.Length > iCmd + 1)
                        {
                            /// list current tree 
                            if (c[iCmd + 1].Contains("/"))
                            {
                                if (c[iCmd + 1].StartsWith("/"))
                                {
                                    t = Root;
                                    if (c[iCmd + 1].Length > 1)
                                        t = t.Go(c[iCmd + 1]);
                                }
                                else
                                {
                                    t.Go(c[iCmd + 1]);
                                }
                                if (t == null)
                                    say(c[iCmd + 1] + " not found");
                            }
                            else
                            {
                                switch (c[iCmd + 1].ToLower())
                                {
                                    case "first":
                                    case "<<":
                                        if (t.First != null)
                                        {
                                            t = t.First;
                                        }
                                        else
                                            say("only one");
                                        break;
                                    case "prev":
                                    case "<":
                                        if (t.Prev != null)
                                        {
                                            t = t.Prev;
                                        }
                                        else
                                            say("no prev");
                                        break;
                                    case ">":
                                    case "next":
                                        if (t.Next != null)
                                            t = t.Next;
                                        else
                                            say("no next");
                                        break;
                                    case ">>":
                                    case "last":
                                        if (t.Last != null)
                                            t = t.Last;
                                        else
                                            say("only one");
                                        break;
                                    case "^":
                                    case "..":
                                    case "parent":
                                        if (t.Parent != null)
                                            t = t.Parent;
                                        else
                                            say("no parent");
                                        break;
                                    case ":":
                                    case "child":
                                        if (t.Child != null)
                                            t = t.Child;
                                        else
                                            say("no child");
                                        break;
                                    case "?":
                                    case "item":
                                        say("#{0,4} {1}\r\nDepth:{2}\r\n[{3},{4}]{5}\r\nItem:{6}",
                                            t.ID, t.Text,
                                            t.Depth()
                                            , ((t.Parent == null) ? "top" : (t.Depth().ToString()))
                                            , ((t.Next == null && t.Prev == null) ? "own" : t.Kins().ToString())
                                            , ((t.Child == null) ? " " : (":" + t.Childs()))
                                            , (t.Item as string));
                                        break;
                                    case "*":
                                    case "mul":
                                        if (c.Length > iCmd + 1)
                                        {
                                            int tid = 0;
                                            if (c.Length > iCmd + 3)
                                                if (!int.TryParse(c[iCmd + 3], out tid))
                                                    tid = 0;

                                            t = t.Mul(c[iCmd+ 2], tid, ((c.Length > iCmd+4) ? c[iCmd+4] : null));
                                        }
                                        else
                                            say("mul <nodename> <id> <item>");
                                        break;
                                    case "+":
                                    case "add":
                                        if (c.Length > iCmd+2)
                                        {
                                            int tid = 0;
                                            if (c.Length > iCmd+3)
                                                if (!int.TryParse(c[3], out tid))
                                                    tid = 0;

                                            t = t.Add(c[iCmd+2], tid, ((c.Length > iCmd+4) ? c[iCmd+4] : null));
                                        }
                                        else
                                            say("add <nodenaame> <id> <item>");
                                        break;
                                    case "del":
                                    case "-":
                                        t.Del();
                                        t = null;
                                        break;
                                    case "walk":
                                        if (t != null)
                                            t.Walk(Show);

                                        break;
                                    case "dir":
                                        if (t != null)
                                        {
                                            if (c.Length < 2)
                                                t.Bore(Show);
                                            else
                                                t.Bore(Show, 1);

                                        }

                                        break;
                                    case "ls":
                                        if (t != null)
                                        {
                                            SimpleTree a = t.First;
                                            
                                            while (a != null)
                                            {
                                                Show(a);
                                                a = a.Next;
                                            }
                                        }
                                        break;
                                    case "go":
                                        if (t != null)
                                        {
                                            if (c.Length > iCmd+2)
                                                t = t.Go(c[iCmd+2]);
                                        }
                                        break;
                                }
                            }
                        }
                        if (c.Length < 2 || "|walk|dir|ls|item|?|cd|".IndexOf("|" + c[1] + "|") < 0)
                            Show(t);

                        ActiveNode = t;
                        break;
                    case XmlAct.Read:
                        // read <filename>
                        if (c.Length > iCmd+1)
                        {
                            string rr = Read(c[iCmd+1]);
                            string fx = Path.ChangeExtension(c[iCmd+1], ".txt");
                            Util.ReMoveFile(fx);
                            File.WriteAllText(fx, rr);
                        }
                        else
                        {
                            say("read <filename>");
                        }
                        break;
                    case XmlAct.Run:
                        // run <filename> 
                        if (c.Length > iCmd + 1)
                        {
                            string cmds = File.ReadAllText(c[iCmd + 1]);
                            string[] rune = cmds.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            XmlLogic xx = new XmlLogic();
                            int xi = 0;
                            while (xi < rune.Length)
                            {
                                string x = rune[xi];
                                if (!string.IsNullOrEmpty(x))
                                    x = x.TrimStart();
                                
                                if (string.IsNullOrEmpty(x) || x.StartsWith("#") || x.StartsWith(";") || x.StartsWith("//"))
                                {
                                    xi++;
                                    continue;
                                }
                                xx.Do(x);
                                xi++;
                            }
                        }
                        break; 
                    case XmlAct.Help:
                        Argu.Help(2);
                        break;
                    case XmlAct.Sql:
                        if (c.Length > iCmd + 1)
                        {
                            say(Sing.Me.SQL[c[1]]);
                        }
                        else
                            say(string.Join("\r\n", Sing.Me.SQL.Keys));
                        break; 
                    case XmlAct.Exit:
                        break;
                    default:
                        say("wrong command, use 'help'");
                        Argu.Help(2);
                        break;
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("{0}", ex.Trace());
                
                Sing.Me.Text.Say("e", "{0}", ex.Trace());
                log("{0}", ex.Trace());
            }

        }

        public void Show(SimpleTree t)
        {
            if (t != null)
            {
                say(string.Format("{0,4} {1}{2}", t.ID, new string(' ', 2 * t.Depth()), t.Text));
            }
            else
                say("null");
        }
    }
}
