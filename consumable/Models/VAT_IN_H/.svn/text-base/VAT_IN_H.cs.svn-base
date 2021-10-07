using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using consumable.Models.SUPPLIER;
using consumable.Models.SystemProperty;

namespace consumable.Models.VAT_IN_H
{
    public class VAT_IN_H
    {
        public Int32 ROW_NUM { get; set; }

        public int Number { get; set; }
        public string TAX_INVOICE_NO { get; set; }
        public string REPLACEMENT_FG { get; set; }
        public string SAP_TAX_INVOICE_NO { get; set; }
        public string TRANS_TYPE_CODE { get; set; }
        public decimal DPP_PRICE { get; set; }
        public decimal VAT_PRICE { get; set; }
        public decimal LUXURY_TAX_PRICE { get; set; }
        public string STATUS { get; set; }
        //public SystemProperty SYSTEM
        //{
        //    get
        //    {
        //        //var result = SystemRepository.Instance.GetStatus(STATUS);
        //        //return result;

        //        SystemProperty system = new SystemProperty();
        //        system.SYSTEM_VALUE_TEXT = "STATUS";

        //        return system;
        //    }
        //}
        public string INV_STATUS { get; set; }
        public string TAX_INVOICE_MONTH { get; set; }
        public string TAX_INVOICE_YEAR { get; set; }
        public DateTime TAX_INVOICE_DT { get; set; }
        public decimal IS_CREDITABLE { get; set; }
        public string INVOICE_NO { get; set; }
        public string SAP_DOC_NO { get; set; }
        public DateTime SAP_POST_DT { get; set; }
        public DateTime SCAN_DT { get; set; }
        public DateTime? INTERFACE_DT { get; set; }
        public DateTime DOWNLOAD_DT { get; set; }
        public string SCAN_BY { get; set; }
        public string QR_CODE_PATH { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public DateTime? CHANGED_DT { get; set; }
        public string CREATED_BY { get; set; }
        public string CHANGED_BY { get; set; }
        public Supplier SUPPLIER
        {
            get
            {
                //var result = SupplierRepository.Instance.GetDataByID(TAX_INVOICE_NO, REPLACEMENT_FG);
                //return result;

                Supplier supplier = new Supplier();
                supplier.NPWP = "51W0R4H1";
                supplier.NAME = "Dummy Supplier";

                return supplier;
            }
        }

        public string InvStatus
        {
            get
            {
                string result = SystemPropertyRepository.Instance.GetInvStatus(INV_STATUS);
                return result;
            }
        }
    }
}