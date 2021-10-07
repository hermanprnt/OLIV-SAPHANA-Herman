using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Common.Data
{
    [Serializable]
    public class GR_IR_TEMP_DATA
    {
        public string GR_IR_ID { get; set; }
        public string PO_NUMBER { get; set; }
        public string PO_ITEM { get; set; }
        public string DN_NO_ITEM { get; set; }
        public string DN_NO { get; set; }
        public string MAT_DESCR { get; set; }
        public DateTime? PO_DATE { get; set; }
        public DateTime? MATDOC_DATE { get; set; }
        public string VEND_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string MATDOC_NUMBER { get; set; }
        public Double MATDOC_QTY { get; set; }
        public Double MATDOC_AMOUNT { get; set; }
        public string HEADER_TEXT { get; set; }
        public string PLANT_CODE { get; set; }
        public string SLOC_CODE { get; set; }
        public string MATDOC_CURRENCY { get; set; }
        public string USRID { get; set; }
        public string GR_STATUS { get; set; }
        public string PROCESS_STATUS { get; set; }
        public string SUPPLIER_STATUS { get; set; }
        public DateTime? PROCESS_STATUS_DT { get; set; }
        public string PROCESS_BY { get; set; }
        public String CURRENCY { get; set; }
        public int GRID_STATUS { get; set; }
    }
}