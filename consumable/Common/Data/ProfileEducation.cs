using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class ProfileEducation : ProfileData
    {
        public string LEVEL { get; set; }
        public string INSTITUTE_NAME { get; set; }
        public string COUNTRY { get; set; }
        public string FINAL_GRADE { get; set; }
        public string MAJOR { get; set; }
        public string FINAL_CERTIFICATE { get; set; }
        public DateTime START_DATE { get; set; }
    }
}
