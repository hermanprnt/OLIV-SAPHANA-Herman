﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.InvoiceCreation
{
    public class InvoiceAttachment
    {
        public string INVOICE_ATTACHMENT_ID { set; get; }
        public string INVOICE_ID { set; get; }
        public string FILE_NAME { get; set; }
        public string FILE_NAME_SERVER { get; set; }

        //new
        public string ATTACHMENT_TYPE { set; get; }

    }
}