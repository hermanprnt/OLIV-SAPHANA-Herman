using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace consumable.Models.EFaktur
{
    public class EFaktur
    {
        public Int32 ROW_NUM { get; set; }
        public String TAX_INVOICE_NO { get; set; }
        public DateTime? TAX_INVOICE_DT { get; set; }
        public Double VAT_PRICE { get; set; }
        public Double IS_USED { get; set; }
        public String SUPPLIER_CODE { get; set; }
        public String SUPPLIER_CD { get; set; }
        public String SUPPLIER_NAME { get; set; }
        
        public String CREATED_BY { get; set; }
        public String CREATED_DT { get; set; }

        public String CHANGED_BY { get; set; }
        public String CHANGED_DT { get; set; }
    }

    public class EFakturConfigDTO
    {
        public String TargetHeaderTable { get; set; }
        public String TargetDetailTable { get; set; }
        public String HeaderName { get; set; }
        public String DetailName { get; set; }
        public List<FieldMapDTO> HeaderFields { get; set; }
        public List<FieldMapDTO> DetailFields { get; set; }
    }

    public class FieldMapDTO
    {
        public string fieldName { get; set; }
        public string fieldMap { get; set; }
    }

}