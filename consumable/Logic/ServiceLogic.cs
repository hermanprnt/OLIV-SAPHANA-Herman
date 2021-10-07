using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consumable.Common.Data;
using System.Net;
using consumable.Common;
using System.Diagnostics;

using System.IO;

namespace consumable.Logic
{
    public class ServiceLogic
    {
        StringBuilder bi = null, bo = null;

        public ServiceLogic()
        {
            Reset();
        }

        private void Reset()
        {
            if (bi != null) bi.Clear();
            if (bo != null) bo.Clear();
            bi = new StringBuilder("");
            bo = new StringBuilder("");
        }

        

     

     

        public void UpdateAction(List<TAction> l)
        {
            if (l == null || l.Count < 1)
                return;

            OrganizationManagement.MIOS_Upd_ActionService service = new OrganizationManagement.MIOS_Upd_ActionService(); 
            
            service.Credentials = new NetworkCredential(AppSetting.SAPPI_UID, AppSetting.SAPPI_PWD);
            OrganizationManagement.DT_Upd_Action_Req req = new OrganizationManagement.DT_Upd_Action_Req();
            OrganizationManagement.DT_Upd_Action_ReqItem[] rit = l.Select(a => a.Sappi()).ToArray();

            bi.AppendLine("PERNR|BEGDA|MASSN|MASSG|ORGEH|STELL|PLANS|SHORT|STEXT|WERKS|BTRTL|SCHKZ|MESSAGE".Replace("|", "\t"));
            foreach (var t in l)
            {
                bi.AppendLine(Tab(t.PERNR, t.BEGDA, t.MASSN, t.MASSG, t.ORGEH, t.STELL, t.PLANS, t.SHORT, t.STEXT, t.WERKS, t.BTRTL, t.SCHKZ, t.MESSAGE));
            }
            req.T_Action = rit;
            OrganizationManagement.DT_Upd_Action_Resp ret = null;
            try
            {
                ret = service.MIOS_Upd_Action(req);
                bo.AppendLine("PERNR|BEGDA|MASSN|MASSG|ORGEH|STELL|PLANS|SHORT|STEXT|WERKS|BTRTL|SCHKZ|MESSAGE".Replace("|", "\t"));
                
                foreach (var o in ret.T_Action)
                {
                    TAction a = o.Sapo();
                    bo.AppendLine(Tab(a.PERNR, a.BEGDA, a.MASSN, a.MASSG, a.ORGEH, a.STELL, a.PLANS, a.SHORT, a.STEXT, a.WERKS, a.BTRTL, a.SCHKZ, a.MESSAGE));                        
                }
            }
            catch (Exception ex)
            {
                ex.Err();
                throw;
            }
            finally
            {
                Util.WriteLog("ACTION", bi, bo);
            }
        }

      

        public static string Tab(params object[] x)
        {
            StringBuilder b = new StringBuilder("");
            if (x==null)
                return null; 

            for (int i = 0; i < x.Length; i++)
            {
                if (i>0)
                    b.Append("\t");
                b.Append(x[i]);
            }            
            return b.ToString();
        }
        

    }
}
