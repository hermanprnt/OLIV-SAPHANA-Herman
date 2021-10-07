using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using consumable.Logic;

namespace consumable.Common
{
    /// <summary>
    /// logging extension for any object, log to text file 
    /// </summary>
    public static class ObjectLogExtension
    {
        public static string LogFileName = null;
        public static DateTime LastLog = new DateTime(1753, 12, 31);

        public static void Log(this object o, string w, params object[] x)
        {
            if (string.IsNullOrEmpty(LogFileName))
            {
                LogFileName = GetLogFileName(o);

            }
            string what = string.Format(w, x);

            DateTime d = DateTime.Now;
            string ts = new string(' ', 9);
            bool mark = (string.IsNullOrEmpty(w) || w.StartsWith("\r\n"));

            if (Math.Floor((d - LastLog).TotalSeconds) > 0 || mark)
            {
                LastLog = d;
                ts = d.ToString("HH:mm:ss ");
            }
            try
            {
                File.AppendAllText(LogFileName, ts + string.Format(w, x) + "\r\n");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Get Log File based on under %ALLUSERSPROFILE% or Common Application Data 
        /// using asssemby company attribute or 'toyota' as default company 
        /// log resides in with current year, month and day sub directory 
        /// and name based on class calling these log
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetLogFileName(this object o)
        {
            DateTime n = DateTime.Now;
            AssemblyCompanyAttribute aca =
                AssemblyCompanyAttribute.GetCustomAttribute(
                    Assembly.GetExecutingAssembly()
                    , typeof(AssemblyCompanyAttribute))
                as AssemblyCompanyAttribute;

            string company = "toyota";
            if (aca != null)
            {
                company = aca.Company;
            }
            string logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                , company
                , System.AppDomain.CurrentDomain.FriendlyName.CleanNameOf()
                , "log"
                , n.Year.ToString()
                , n.Month.ToString()
                , n.Day.ToString());

            try
            {
                if (!Directory.Exists(logFile))
                {

                    Directory.CreateDirectory(logFile);
                }

                logFile = Path.Combine(logFile, o.GetType().Name + ".log");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                logFile = null;
            }
            return logFile;
        }

        /// <summary>
        /// for resetting log file with filename 'logFile' when rename existing file
        /// </summary>
        /// <param name="o">object</param>
        /// <param name="logFile">File name to Reset</param>
        /// <returns>operation success</returns>
        public static bool ResetLog(this object o, string logFile)
        {
            bool ok = true;
            LastLog = new DateTime(1753, 12, 31);

            if (!string.IsNullOrEmpty(logFile) && File.Exists(logFile))
            {
                string tmpFile = logFile;
                int j = tmpFile.LastIndexOf(".");

                try
                {
                    if (j > 0)
                    {
                        DateTime d = File.GetLastWriteTime(logFile);
                        int SEQ = 0;
                        do
                        {
                            int seq = SEQ + ((int)(DateTime.Now - DateTime.Today).TotalMilliseconds % 1000);
                            tmpFile = logFile.Insert(j, d.ToString(".HHmmss." + seq.ToString().PadRight(3, '0')));
                            SEQ++;
                        }
                        while (File.Exists(tmpFile));
                    }

                    File.Move(logFile, tmpFile);
                }
                catch (Exception e)
                {
                    ok = false;
                    Debug.WriteLine(e);
                }
            }
            else
            {
                ok = true;
            }
            if (ok)
                LogFileName = logFile;

            return ok;
        }

        public static void Err(this Exception ex)
        {
            Type t = ex.GetType();
            string xtype = (t != null) ? t.FullName : "?";

            Sing.Me.Text.Say(AppSetting.UID, "{0}\r\n{1}", xtype, ex.Trace());
        }
    } // class ObjectLogExtension
}
