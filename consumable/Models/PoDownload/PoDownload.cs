using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.PoDownload
{
    public class PoDownload
    {
        public Int32 ROW_NUM { get; set; }
        public String PRODUCTION_MONTH { get; set; }
        public DateTime PO_RELEASE_DATE { get; set; }
        public DateTime? PO_DATE { get; set; }
        //public String SUPPLIER_CD { get; set; }
        //public String SUPPLIER_NAME { get; set; }
        public String VEND_CODE { get; set; }
        public String VEND_NAME { get; set; }
        public String REQUESTER { get; set; }
        public String PO_TYPE { get; set; }
        public String PO_NUMBER { get; set; }
        public String PO_DESCRIPTION { get; set; }
        public String PO_DESCS { get; set; }
        public String PO_CURRENCY { get; set; }
        public Double PO_AMOUNT { get; set; }
        public String PO_FILE { get; set; }
        public String CONFIRMATION { get; set; }
        public String UNLOCK { get; set; }
        public String DOWNLOAD_STATUS { get; set; }
        public String DOWNLOAD_BY { get; set; }
        public DateTime? DOWNLOAD_DT { get; set; }

        public String APP_SH_STATUS { get; set; }
        public String APP_SH_BY { get; set; }
        public DateTime? APP_SH_DT { get; set; }
        public String APP_DPH_STATUS { get; set; }
        public String APP_DPH_BY { get; set; }
        public DateTime? APP_DPH_DT { get; set; }
        public String APP_DH_STATUS { get; set; }
        public String APP_DH_BY { get; set; }
        public DateTime? APP_DH_DT { get; set; }

        public string NOTICE_BY { set; get; }
        public string NOTICE_REMARK { set; get; }
        public DateTime? NOTICE_DATE { set; get; }
      
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }

        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }

        public string APPROVAL_STATUS { set; get; }

        public IList<PoAttachment> PoAttachmentList { get; set; }
    }
}