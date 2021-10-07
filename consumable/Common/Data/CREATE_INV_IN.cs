using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class CREATE_INV_IN
    {
        public DateTime? INV_DATE { get; set; }
        public DateTime? POST_DATE { get; set; }
        public string REF_INV { get; set; }
        public string AMOUNT { get; set; }
        public string TAX_AMT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string PO_NUMBER { get; set; }
        public string PO_ITEM { get; set; }
        public DateTime? BASE_DATE { get; set; }
        public string PAY_METHOD { get; set; }
        public string HEAD_TEXT { get; set; }
        public string TERM_PAY { get; set; }
        public string TAX_CODE { get; set; }
        public string BVTYP { get; set; }
        public DateTime? TAX_DATE { get; set; }
    }
}
