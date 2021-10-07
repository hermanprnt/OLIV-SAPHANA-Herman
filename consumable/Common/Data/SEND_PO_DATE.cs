using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class SEND_PO_DATE
    {
        public string SIGN { get; set; }
        public string OPTION { get; set; }
        public DateTime? LOW { get; set; }
        public DateTime? HIGH { get; set; }
    }
}
