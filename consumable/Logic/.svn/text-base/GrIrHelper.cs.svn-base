using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consumable.Common.Data;
using consumable.Common;

namespace consumable.Logic
{
    public static class GrIrHelper
    {
        public static GR_IR_DOC_NO DocNo(this LogHelper l)
        {
            return new GR_IR_DOC_NO()
            {
                SIGN = l["SIGN"],
                OPTION = l["OPTION"],
                LOW = l["LOW"],
                HIGH = l["HIGH"]
            };
        }

        public static GR_IR_DOC_DATE DocDate(this LogHelper l)
        {
            return new GR_IR_DOC_DATE()
            {
                SIGN = l["SIGN"],
                OPTION = l["OPTION"],
                LOW = l["LOW"].dateSQL(),
                HIGH = l["HIGH"].dateSQL()
            };
        }

        public static GR_IR_DATA Data(this LogHelper l)
        {
            return new GR_IR_DATA()
            {
                PO_NUMBER = l["PO_NUMBER"],
                PO_ITEM = l["PO_ITEM"],
                MATDOC_YEAR = l["MATDOC_YEAR"],
                MATDOC_NUMBER = l["MATDOC_NUMBER"],
                MATDOC_ITEM = l["MATDOC_ITEM"],
                MATDOC_DATE = DateTime.ParseExact(l["MATDOC_DATE"], "yyyy-MM-dd", null),
                SPC_STOCK = l["SPC_STOCK"],
                //MATDOC_AMOUNT = Double.Parse(l["MATDOC_AMOUNT"]), 
                VEND_CODE = l["VEND_CODE"], 
                PURCHDOC_PRICE = l["PURCHDOC_PRICE"], 
                MAT_NUMBER = l["MAT_NUMBER"], 
                MAT_DESCR= l["MAT_DESCR"], 
                PLANT_CODE= l["PLANT_CODE"], 
                SLOC_CODE= l["SLOC_CODE"], 
                MATDOC_UNIT= l["MATDOC_UNIT"], 
                CANCEL= l["CANCEL"], 
                ORI_MAT_NUMBER= l["ORI_MAT_NUMBER"], 
                MATDOC_CURRENCY= l["MATDOC_CURRENCY"],
                MATDOC_QTY = Double.Parse(l["MATDOC_QTY"]), 
                TAX_CODE = l["TAX_CODE"]
            };
        }

        public static GR_IR_RETURN Ret(this LogHelper l) 
        {
            return new GR_IR_RETURN()
            {
                TYPE = l["TYPE"],
                MESSAGE = l["MESSAGE"]
            };        
        }
         
        public static int PutObj<T>(this T u)
        {
            int rowcount =0;
            string q = "Put" + u.GetType().Name;
            Sing.Me.Text.Say("DB", "{0} {1}", q, Formator.Prop(u));
            try
            {
                rowcount = Sing.Me.Do(q, u);                
            }
            catch (Exception ex)
            {
                Sing.Me.Text.Say("DB", "{0}", ex.Trace());
            }
            return rowcount;
            
        }

        public static void Put(this GR_IR_TABLE d)
        {
            int Datas = 0, Returns = 0;

            foreach (GR_IR_DATA p in d.DATA)
            {
                Datas += PutObj<GR_IR_DATA>(p);
            }

            foreach (GR_IR_RETURN u in d.RETURN)
            {
                Returns += u.PutObj<GR_IR_RETURN>();
            }

            Sing.Me.Text.Say("DB", "Updated {0} datas {1} returns", Datas, Returns);
        }

        public static string Tag(this OrgUnit o, string op)
        {
            return string.Format("{0,8}\t{1}\t{2}", op, o.ORGID.ToString("00000000") , o.PARENTID.ToString("00000000"));
        }
    }
}
