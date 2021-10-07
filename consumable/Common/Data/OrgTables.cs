using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class OrgTables
    {   public List<OrgUnit> ORG_UNITS { get; set; }
        public List<OrgPers> PERSON_TAB { get; set; }
        public List<OrgUserPosition> ORG_PERS_REL { get; set; }

        public OrgTables()
        {
            ORG_UNITS = new List<OrgUnit>();
            PERSON_TAB = new List<OrgPers>();
            ORG_PERS_REL = new List<OrgUserPosition>();
        }
    }
}
