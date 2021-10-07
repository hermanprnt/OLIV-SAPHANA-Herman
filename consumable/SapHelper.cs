using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using consumable.Common.Data;
using consumable.Logic;
using consumable.Common; 

namespace consumable
{
    public class SapHelper
    {
        /*public SapHelper() {
            Do(new Argu());
        }*/

        public void Do(Argu a)
        {
            a.Numbers = a.Numbers.Distinct().ToList();

            //ServiceLogic pi = new ServiceLogic();
            SAPLogic sap = null;
            if ((a.Work == Doing.Help || a.Work == Doing.Xml || a.Options.Contains("i"))) 
            {
                sap = new SAPLogic();
                sap.ToText = a.Options.Contains("text");                
            }
            DateTime D = DateTime.Now;
            DateTime D2 = D;
            string NOREG = null;
            if (a.Params.Count > 0)
                D = a.Params[0].Date();
            if (a.Params.Count > 1)
                D2 = a.Params[1].Date();
            if (a.Params.Count > 2)
                NOREG = a.Params[2];

            
            switch (a.Work)
            {
                /// zeroth 
                /*case Doing.Help:
                    Argu.Help((a.Params.Count > 0) ? a.Params[0] : null);
                    break;*/

                case Doing.Help: /// ZHRTFM_GET_EMPLOYEE_PROFILE
                    GR_IR_TABLE e = new GR_IR_TABLE();

                    e.DOC_NO = new List<GR_IR_DOC_NO>();// {LOW = "5000059225", HIGH = "5000059225" };
                    GR_IR_DOC_NO dn = new GR_IR_DOC_NO();
                    dn.SIGN = "I";
                    dn.OPTION = "EQ";
                    dn.LOW = "5000059225";
                    dn.HIGH = "5000059225";

                    e.DOC_DATE = new List<GR_IR_DOC_DATE>();// {LOW = "5000059225", HIGH = "5000059225" };
                    GR_IR_DOC_DATE dc = new GR_IR_DOC_DATE();
                    dc.SIGN = "I";
                    dc.OPTION = "EQ";
                    dc.LOW = null;
                    dc.HIGH = null;

                    e.DOC_NO.Add(dn);
                    e.DOC_DATE.Add(dc);
                    sap.GetGR(e);
                    //sap.GetEmployeeProfile(DateTime.Now, a.Numbers.ToArray(), (a.Options.Count > 0) ? string.Join(";",a.Options) : "");
                    break;

            

              
            }
        }
    }
}
