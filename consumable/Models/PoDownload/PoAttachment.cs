using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.PoDownload
{
    public class PoAttachment
    {
        public string PO_ATTACHMENT_ID { set; get; }
        public string PO_NUMBER { set; get; }
        public string FILE_NAME { get; set; }
        public string FILE_NAME_SERVER { get; set; }

    }
}