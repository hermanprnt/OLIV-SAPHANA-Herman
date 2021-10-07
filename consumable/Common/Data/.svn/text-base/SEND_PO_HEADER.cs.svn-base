using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class SEND_PO_HEADER
    {
        public string PO_NUMBER { get; set; }
        public string VEND_CODE { get; set; }
        public string VEND_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string POST_CODE { get; set; }
        public string CITY { get; set; }
        public string ATTENTION { get; set; }
        public string TEL_NUMBER { get; set; }
        public string FAX_NUMBER { get; set; }
        public string DELIV_NAME { get; set; }
        public string DELIV_ADDR { get; set; }
        public string DELIV_POST { get; set; }
        public string DELIV_CITY { get; set; }
        public string CONTACT_PER { get; set; }
        public DateTime? PO_DATE { get; set; }
        public string PO_PAYTERM { get; set; }
        public string PO_TYPE { get; set; }
        public string PO_CAT { get; set; }
        public string PO_PURCH_GRP { get; set; }
        public string PO_CURRENCY { get; set; }
        public Double? PO_AMOUNT { get; set; }
        public string PO_XCHG_RATE { get; set; }
        public string PO_DELETE_FLG { get; set; }
        public string PO_INCOTERM1 { get; set; }
        public string PO_INCOTERM2 { get; set; }


        //TEMP
        public Double PO_AMOUNT_TEMP { get; set; }
        public string SH_NAME { get; set; }
        public string DPH_NAME { get; set; }
        public string DH_NAME { get; set; }

        public Double QTY { get; set; }
        public string ITEM { get; set; }
        public Double AMOUNT { get; set; }
        public Double PRICE { get; set; }
        public Double TOTAL_AMOUNT_ITEM { get; set; }
        public string DESCRIPTION { get; set; }
        public string UNIT { get; set; }


        public DateTime? SH_DATE { get; set; }
        public DateTime? DPH_DATE { get; set; }
        public DateTime? DH_DATE { get; set; }

        public List<SEND_PO_ITEM> PO_ITEM { get; set; }

        public SEND_PO_HEADER()
        {
            PO_ITEM = new List<SEND_PO_ITEM>();
        }
    }
}
