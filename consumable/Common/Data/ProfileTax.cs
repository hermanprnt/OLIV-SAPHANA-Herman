using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class ProfileTax: ProfileData
    {
        public string NPWP_ID { get; set; }
        public DateTime? NPWP_REG_DATE { get; set; }
        public int DEPENDENT_QTY { get; set; }
    }
}
