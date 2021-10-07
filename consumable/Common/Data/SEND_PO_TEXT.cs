using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class SEND_PO_TEXT
    {
        public string PO_NUMBER { get; set; }
        public string PO_ITEM { get; set; } 

        public string LINE_NO { get; set; }
        public string LINE_TEXT { get; set; }
    }
}
