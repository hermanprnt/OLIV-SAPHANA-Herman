using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class DirOrgStrucGetData
    {
        public OrgStrucImports Imports { get; set; }
        public OrgStrucTables Tables { get; set; }

        public DirOrgStrucGetData()
        {
            Imports = new OrgStrucImports();
            Tables = new OrgStrucTables();
        }
    }
}   
