using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class SEND_PO_ITEM
    {
        public string PO_NUMBER { get; set; }
        
        public string PO_ITEM { get; set; }
        public string INVOICE_FLG { get; set; }
        public string PR_NUMBER { get; set; }
        public string PR_ITEM { get; set; }
        public string MAT_NUMBER { get; set; }
        public string MAT_DESCR { get; set; }
        public string VAL_CLASS { get; set; }
        public DateTime? DELIV_PLAN_DT { get; set; }
        public string PLANT_CODE { get; set; }
        public string SLOC_CODE { get; set; }
        public string WBS_NUMBER { get; set; }
        public string COST_CTR { get; set; }
        public Double PO_QTY_ORI { get; set; }
        public Double PO_QTY_NEW { get; set; }
        public Double PO_QTY_USED { get; set; }
        public string PO_UNIT { get; set; }
        public Double PRICE_PER_UNIT { get; set; }
        public string ITM_DEL_FLG { get; set; }
        public string TAX_CODE { get; set; }

        public List<SEND_PO_DETAIL_ITEM> PO_DETAIL_ITEM { get; set; }
        public List<SEND_PO_SERVICE> PO_SERVICE { get; set; }
        public List<SEND_PO_TEXT> PO_TEXT { get; set; }

        //temp for print PO
        public Double TOTAL_AMOUNT_ITEM { get; set; }

        public SEND_PO_ITEM()
        {
            PO_DETAIL_ITEM = new List<SEND_PO_DETAIL_ITEM>();
            PO_SERVICE = new List<SEND_PO_SERVICE>();
            PO_TEXT = new List<SEND_PO_TEXT>();
        }

    }
}
