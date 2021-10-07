using System;
using consumable.Logic;
using consumable.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace consumable
{
    class Program
    {
        public delegate void DoSay(string what, params object[] args);

        public static DoSay Say = null;

        static void Main(string[] args)
        {
            string cmd = null;
            bool interactive = (args.Length > 0)
                && (args.Where(a => "-i".Equals(a.ToLower()))
                        .Count() > 0);
            
            
            Argu ar = new Argu();

            ar.Read(args);
            if (!ar.Options.Contains("i"))
            {
                SapHelper sap = new SapHelper();
                try
                {
                    sap.Do(ar);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("{0}", ex.Trace());
                    
                    Sing.Me.Text.Say("e", "{0}", ex.Trace());

                    Console.WriteLine("{0}", ex.Trace());
                }

            }
            else
            {
                XmlLogic x = new XmlLogic();
                TextLogger t = new TextLogger("xml"); 
                if (ar.Options.Contains("debug"))
                {
                    x.Debug = 1;
                    x.OnLog += t.say;
                }
                ConsoleWriter w = new ConsoleWriter(); 
                x.OnLog += w.say;

                if (ar.Params.Count > 1)
                    x.Do(string.Join("\t", ar.Params));
                do
                {
                    cmd = "!";
                    
                    Console.Write(">");
                    cmd = Console.ReadLine();

                    x.Do(cmd);
                }
                while (!string.IsNullOrEmpty(cmd));
            }
        }

    }
}
