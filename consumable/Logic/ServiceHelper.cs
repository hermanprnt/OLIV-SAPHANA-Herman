using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consumable.Common;
using consumable.Common.Data;
using Dapper;

namespace consumable.Logic
{
    public static class ServiceHelper
    {
        #region CICO

        public static DT_CICO_reqItem Sappi(this TCICO c)
        {
            return new DT_CICO_reqItem()
           {
               PERNR = c.PERNR,
               LDATE = c.LDATE,
               LTIME = c.LTIME,
               SATZA = c.SATZA,
               DALLF = c.DALLF,
               TERID = c.TERID,
               STATS = c.STATS
           };
        }

        public static TCICO Sapo(this TimeManagement.DT_CICO_RespItem r)
        {
            return new TCICO()
            {
                PERNR = r.PERNR,
                LDATE = r.LDATE,
                LTIME = r.LTIME,
                SATZA = r.SATZA,
                DALLF = r.DALLF,
                TERID = r.TERID,
                STATS = r.STATS
            };
        }

        public static List<TCICO> GetCico(DateTime dateFrom, DateTime dateTo, string Noreg)
        {
            return Sing.Me.Qx<TCICO>("GetCico", new { dateFrom = dateFrom.ToString(StrTo.SQL_DATUM), dateTo = dateTo.ToString(StrTo.SQL_DATUM), Noreg = Noreg }).ToList();
        }
        #endregion

        #region Shift

        public static List<TShift> GetShift(DateTime dateFrom, DateTime dateTo, string Noreg)
        {
            List<TShift> r = null;
            r = Sing.Me.Qx<TShift>("GetShift", new { dateFrom = dateFrom.ToString(StrTo.SQL_DATUM), dateTo = dateTo.ToString(StrTo.SQL_DATUM), Noreg = Noreg }).ToList();
            return r;
        }

        public static dt_update_ShiftData_reqItem Sappi(this TShift t)
        {
            return new dt_update_ShiftData_reqItem()
            {
                Personnel_Number = t.PERNR,
                Complete_Name = t.CNAME,
                Start_Date = t.BEGDA.DateSAP(),
                Start_DateSpecified = true,
                End_Date = t.ENDDA.DateSAP(),
                End_DateSpecified = true,
                Daily_Work_Schedule = t.TPROG,
                Daily_Work_Schedule_Variant = t.VARIA,
                Delete_Indicator = t.DELET,
                Message_Text = t.MESSAGE
            };
        }

        public static dt_update_Overtime_reqItem Sappi(this TOvertime t)
        {
            return new dt_update_Overtime_reqItem()
            {
                Personnel_Number = t.PERNR,
                Complete_Name = t.CNAME,
                Category = t.CAT,
                Attendance_Quota_Type = t.KTART,
                Attendance_Quota_Type_Texts = t.KTEXT,
                Start_Date = t.BEGDA.Date(),
                Start_DateSpecified = true,
                End_Date = t.ENDDA.Date(),
                End_DateSpecified = true,
                Start_Time = t.BEGUZ.Time(),
                Start_TimeSpecified = true,
                End_Time = t.ENDUZ.Time(),
                End_TimeSpecified = true,
                Delete_Indicator = t.DELET,
                Message_Text = t.MESSAGE
            };
        }
        #endregion


        #region Overtime

        public static List<TOvertime> GetOvertime(DateTime dateFrom, DateTime dateTo, string Noreg )
        {
            List<TOvertime> r;
            r = Sing.Me.Qx<TOvertime>("GetOvertime", new { dateFrom = dateFrom.ToString(StrTo.SQL_DATUM), dateTo = dateTo.ToString(StrTo.SQL_DATUM), Noreg = Noreg }).ToList();
            return r;
        }

        public static TOvertime Sapo(dt_update_Overtime_respItem ri)
        {
            return new TOvertime()
            {
                PERNR = ri.Personnel_Number,
                CNAME = ri.Complete_Name,
                CAT = ri.Category,
                KTART = ri.Attendance_Quota_Type,
                KTEXT = ri.Attendance_Quota_Type_Texts,
                BEGDA = ri.Start_Date.ToString(StrTo.XLS_DATE),

                ENDDA = ri.End_Date.ToString(StrTo.XLS_DATE),
                BEGUZ = ri.Start_Time.ToString(StrTo.TIME_SEC),
                ENDUZ = ri.End_Time.ToString(StrTo.TIME_SEC),
                DELET = ri.Delete_Indicator,
                MESSAGE = ri.Message_Text
            };
        }

        #endregion


        #region Bdjk

        public static List<TBdjk> GetBdjk(DateTime dateFrom, DateTime dateTo, string Noreg = "")
        {
            List<TBdjk> r;
            r = Sing.Me.Qx<TBdjk>("GetBdjk", new { dateFrom = dateFrom.ToString(StrTo.SQL_DATUM), dateTo = dateTo.ToString(StrTo.SQL_DATUM), Noreg = Noreg }).ToList();
            return r;
        }


      

        #endregion

        #region Action

        public static List<TAction> GetAction(List<string> noregs)
        {
            List<TAction> r;
            string Noregs = string.Join(",", noregs);
            r = Sing.Me.Qx<TAction>("GetAction", new { Noregs = Noregs }).ToList();
            return r;
        }

        public static OrganizationManagement.DT_Upd_Action_ReqItem Sappi(this TAction a)
        {
            return new OrganizationManagement.DT_Upd_Action_ReqItem()
            {
                Personnel_Number = a.PERNR,
                Date = a.BEGDA.Date(),
                DateSpecified = true,
                Action_Type = a.MASSN,
                Reason_for_Action = a.MASSG,
                Organizational_Unit = a.ORGEH,
                Job = a.STELL,
                Position = a.PLANS,
                Object_Abbreviation = a.SHORT,
                Object_Name = a.STEXT,
                Personnel_Area = a.WERKS,
                Personnel_SubArea = a.BTRTL,
                Work_Schedule_Rule = a.SCHKZ,
                Message_Text = a.MESSAGE
            };
        }

        public static TAction Sapo(this OrganizationManagement.DT_Upd_Action_RespItem r)
        {
            return new TAction()
            {
                PERNR = r.Personnel_Number,
                BEGDA = r.Date.ToString(StrTo.XLS_DATE),
                MASSN = r.Action_Type,
                MASSG = r.Reason_for_Action,
                ORGEH = r.Organizational_Unit,
                STELL = r.Job,
                PLANS = r.Position,
                SHORT = r.Object_Abbreviation,
                STEXT = r.Object_Name,
                WERKS = r.Personnel_Area,
                BTRTL = r.Personnel_SubArea,
                SCHKZ = r.Work_Schedule_Rule,
                MESSAGE = r.Message_Text,
            };
        }
        #endregion

        #region Leave


        public static List<TLeave> GetLeave(DateTime dateFrom, DateTime dateTo, string Noreg = null)
        {
            List<TLeave> r;
            r = Sing.Me.Qx<TLeave>("GetLeave", new { dateFrom = dateFrom.ToString(StrTo.SQL_DATUM), dateTo = dateTo.ToString(StrTo.SQL_DATUM), Noreg = Noreg }).ToList();
            return r;
        }

    
        #endregion
    }
}
