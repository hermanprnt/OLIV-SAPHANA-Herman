using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class RelationAct : RelationBase
    {
        public string FCODE { get; set; } /// INSE, AEND 

        public string PLVAR { get; set; }
        public string OTYPE { get; set; }
        public int OBJID { get; set; }

        public string PRIOX { get; set; }
        public int PROZT { get; set; }
        public string ADATA { get; set; }
        public string GDATE { get; set; }
    }
}
