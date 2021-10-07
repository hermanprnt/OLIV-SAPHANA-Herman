using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace consumable.Models.Message
{
    public class Message
    {
        public string MSG_ID { get; set; }
        public string MSG_TYPE { get; set; }
        public string MSG_TEXT { get; set; }
        public string MSG_ICON { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public DateTime? CHANGED_DT { get; set; }
        public string CREATED_BY { get; set; }
        public string CHANGED_BY { get; set; }
    }
}