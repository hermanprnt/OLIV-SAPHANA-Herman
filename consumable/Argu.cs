using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using consumable.Common;

namespace consumable
{
    public class Argu
    {
        public List<string> Numbers = new List<string>();
        public List<string> Params = new List<string>();
        public List<string> Options = new List<string>();
        public static Ini Iniku = null;

        public static List<string> Actions = Enum.GetNames(typeof(Doing)).Select(a => a.ToLower()).ToList();
        public static int[] ActionNum = Enum.GetValues(typeof(Doing)).Cast<int>().ToArray();

        public Doing Work = Doing.Help;

        public void Read(string[] args)
        {
            Regex rx = new Regex("[0-9]+");
            int actionIndex = -1;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLower().Trim();
                if (((Work == Doing.Get) || (Work == Doing.Put) || (Work == Doing.PiAction)) 
                    && rx.IsMatch(args[i])) /// Numbers 
                {
                    Numbers.Add(args[i]);
                }
                else if (arg.StartsWith("--")) /// options 
                {
                    arg = arg.Substring(2);
                    Options.Add(arg);
                }
                else if (actionIndex < 0 && Actions.Contains(arg.ToLower())) // actions 
                {
                    for (int ai = 0; ai < Actions.Count; ai++)
                    {
                        if (Actions[ai].StartsWith(arg.ToLower()))
                        {
                            actionIndex = ai;
                            if (actionIndex > 0 && Enum.IsDefined(typeof(Doing), ActionNum[actionIndex]))
                                Work = (Doing)ActionNum[actionIndex];
                            break;
                        }
                    }
                }
                else
                {
                    Params.Add(args[i]);
                }
            }
            //if (actionIndex > 0 && Enum.IsDefined(typeof(Doing), ActionNum[actionIndex]))
            //    Work = (Doing) ActionNum[actionIndex];
        }

        public static string GetResource(string filename)
        {
            string result = null;
            Regex r = new Regex("[\\w]+"); 
            MatchCollection mc = r.Matches(filename);

            if (mc.Count == 1 && !filename.Contains("."))
            {
                filename = filename + ".txt"; 
            }
            
            using (Stream stream = Assembly.GetEntryAssembly()
                            .GetManifestResourceStream(
                                AppSetting.GetNamespace() + ".Res." + filename))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();                    
                }
            }
            return result; 
        }

        public static void GetHilfe(Ini hilfe)
        {
            hilfe.Parse(GetResource("Hilfe"));
        }

        public static void Help(int page = 1)
        {
            Ini hilfe = new Ini();
            GetHilfe(hilfe);

            Console.WriteLine(hilfe.Section[hilfe.Sections[page]]);
        }

        public static void Help(string page)
        {
            Ini hil = new Ini();
            GetHilfe(hil);

            if (!string.IsNullOrEmpty(page) && hil.Sections.Contains(page.ToUpper()))
                Console.WriteLine(hil.Section[page.ToUpper()]);
            else if (!string.IsNullOrEmpty(page))

                Console.WriteLine(hil.Section["help"]);
            else
                Console.WriteLine(hil.Section["ARGS"]);

        }

    }


}
