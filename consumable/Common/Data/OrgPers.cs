using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace consumable.Common.Data
{
    public class OrgPers
    {
        public string OTYPE { get; set; } //  C 2 Object Type 
        public string OBJID { get; set; } // C 12 AgentID in Organizational Management 
        public string EDITNAME { get; set; } // C 40 Formatted Name of Employee or Applicant 
        public string LASTNAME { get; set; } // C 40 
        public string FIRSTNAME { get; set; } // C 40 
        public string MIDDLENAME { get; set; } // C 40 
        public string DESCRIPTION { get; set; } // C 40 
        public string TITLE { get; set; } // C 40 
        public string DEPARTMENT { get; set; } // C 40 
        public string OFFICE_BLDG { get; set; } // C 6 Building Number
        public string OFFICE_ROOM { get; set; } // C 6 Room Numbr
        public string FAX { get; set; } // C 241 
        public string MAIL { get; set; } // C 241
        public string PHONE_NUMBER { get; set; } // C 241 
        public string MOBILE { get; set; } // C 241 
        public string PAGER { get; set; } // C 241
        public string HOMEPHONE { get; set; } // C 14 
        public string STREETADDRESS { get; set; } // C 60 Street and House Number 
        public string LOCALITY { get; set; } // C 40 City
        public string DISTRICT { get; set; } // C 40 District 
        public string POSTALCODE { get; set; } // C 10 
        public string STATE { get; set; } // C 3 Region (State, Province, Country) 
        public string COUNTRY { get; set; } // C 3 Country Key

    }
}
