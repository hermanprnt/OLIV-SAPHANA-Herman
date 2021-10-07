using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    [Serializable]
    public class ProfileFamily: ProfileData
    {
        public string RELATIONSHIP { get; set; }
        public string CHILD_TO { get; set; }
        public string NAME { get; set; }
        public string GENDER { get; set; }
        public string PLACE_OF_BIRTH { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public string NATIONALITY { get; set; }
        public string COMPLETE_OF_DATA { get; set; }
        public string STATUS_STUDY { get; set; }
    }
}
