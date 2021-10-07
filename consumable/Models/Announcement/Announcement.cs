using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.Announcement
{
    public class Announcement
    {
        public string NUMBER { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string ANNOUNCEMENT_TITLE { get; set; }
        public string FILE_NAME { get; set; }
        public string FILE_LOCATION { get; set; }
        public DateTime? DATE_RELEASE { get; set; }
        public String DATE_RELEASE_STR { get; set; }
        public string STATUS { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public String CREATED_DT_STR { get; set; }
        public DateTime? CHANGED_DT { get; set; }
        public String CHANGED_DT_STR { get; set; }
        public string CREATED_BY { get; set; }
        public string CHANGED_BY { get; set; }
    }
}