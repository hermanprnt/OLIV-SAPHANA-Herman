using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class SEND_PO_DETAIL_ITEM
    {
        public string PO_NUMBER { get; set; }
        public string PO_ITEM { get; set; }        
        public string COMP_CODE { get; set; }
        public string COMP_RATE { get; set; }
    }
}
