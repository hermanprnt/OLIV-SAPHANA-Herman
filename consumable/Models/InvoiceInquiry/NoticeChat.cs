using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.InvoiceInquiry
{
    public class NoticeChat
   {
        public string ID { set; get; }

        public string INVOICE_ID { get; set; }
        public string ROLE { get; set; }

        public DateTime? CHAT_DATETIME { set; get; }
        public string CHAT_DATETIME_STR { set; get; }
      public string CHAT_MESSAGE { set; get; }
        public string CHAT_FROM { set; get; }
        public string CHAT_TO { set; get; }
   }
}