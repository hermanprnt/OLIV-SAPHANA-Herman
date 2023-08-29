using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using consumable.Models.InvoiceCreation;

namespace consumable.Models.InvoiceInquiry
{
    public class InvoiceInquiry
    {
        #region SEARCH PARAMETER
        public string createdDateSearch { set; get; }
        public string submissionDateSearch { set; get; }
        public string supplierSearch { set; get; }
        public string invoiceDateSearch { set; get; }
        public string statusSearch { set; get; }
        public string statusHardcopySearch { set; get; }
        public string planPaymentDateSearch { set; get; }
        public string invoiceNoSearch { set; get; }
        public string govRelateSearch { set; get; }
        public int page { set; get; }
        public int size { set; get; }
        #endregion


        public string Number { set; get; }
        public string DUE_DILLIGENCE { set; get; }
        public string GOVERNMENT_RELATED { set; get; }
        public string SUPPLIER_NAME { set; get; }
        public List<NoticeChat> HistoryChat { set; get; }
        public List<InvoiceAttachment> InvoiceAttachment { set; get; }

        public Double INVOICE_AMOUNT { get; set; }

        public DateTime? SUBMIT_DT { set; get; }
        public string SUBMIT_BY { set; get; }
        public string ROLE { set; get; }

        public string ROLE_SH { set; get; }
        public string RECEIVED_STATUS { set; get; }
        public string NOTICE { set; get; }

        public string PO_CATEGORY { set; get; }
        public string INVOICE_ID { set; get; }
        public string INVOICE_NO { set; get; }
        public DateTime? INVOICE_DATE { set; get; }
        public string INVOICE_DATE_STR { get; set; }
        public string TAX_INVOICE_NO { get; set; }
        public Double TAX_INVOICE_AMOUNT { get; set; }
        public Double TAX_AMOUNT { get; set; }
        public DateTime? TAX_INVOICE_DT { set; get; }
        public string TAX_INVOICE_DT_STR { set; get; }
        public string CURRENCY { get; set; }
        public int TOTAL_GR_ITEM { get; set; }
        public string WITHOLDING_TAX_CD { get; set; }
        public Double TURN_OVER { set; get; }
        public Double TAX_BASE { set; get; }
        public Double CALCULATE_TAX { set; get; }
        public string CHECKBOX_STAMP { set; get; }
        public Double TOTAL_AMOUNT { set; get; }
        public string STATUS { set; get; }
        public string HARDCOPY_STATUS { set; get; }
        public string NOTICE_FLAG { set; get; }
        public string SUPPLIER_CD { set; get; }
        public DateTime? PAYMENT_PLAN_DATE { set; get; }
        public DateTime? PAYMENT_ACTUAL_DATE { set; get; }
        public string PROGRESS_STATUS { set; get; }
        public string NOTICE_BY { set; get; }
        public string NOTICE_REMARK { set; get; }
        public DateTime? NOTICE_DATE { set; get; }
        public string CERTIFICATE_ID { set; get; }
        public String CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public String UPDATED_BY { get; set; }
        public DateTime? UPDATED_DT { get; set; }
        public String APPROVED_BY { get; set; }
        public DateTime? APPROVED_DT { get; set; }

        public String POSTED_BY { set; get; }
        public DateTime? POSTED_DT { set; get; }
        public String POSTED_DT_STR { set; get; }

        public String LOG_DOC_NO { set; get; }
        public String SAP_DOC_NO { set; get; }
        public String SAP_YEAR { set; get; }
        public String REVERSE_REASON { set; get; }
        public DateTime? REVERSE_POST_DT { set; get; }
        public string REVERSE_POST_DT_STRING { set; get; }
        public String REVERSE_REMARKS { set; get; }

        public String PAYMENT_DOC_NO { set; get; }
        public String ON_PROCESS_SAP_POST { set; get; }
        public String ON_PROCESS_SAP_REV { set; get; }
        public String AUTO_POSTING_FLAG { set; get; }

        //temp
        public String PROCESS_TYPE { set; get; }
        public String PARTNER_BANK { set; get; }
        public String BASE_DATE_STR { set; get; }
        public String PAY_METHOD { set; get; }
        public String TERM_PAY { set; get; }
        public String TAX_CODE { set; get; }

        public String GL_ACCOUNT { set; get; }
        public String GL_AMOUNT { set; get; }
        public string PPV_AMOUNT { set; get; }

        public String WITHHOLDING_TAX { set; get; }

        public String ASSIGNMENT { set; get; }
        public String INVOICE_NOTE { set; get; }

        public String GR_CANCEL { set; get; }

        public IList<PpvAttachment> PpvAttachmentList { get; set; }

        // update 19-12-2020 [START]
        public String BASE_DATE { set; get; }
        // update 19-12-2020 [END]

        //ADD BY HERMAN. TORA PROJECT ENHANCEMENT
        public String DD_STATUS { set; get; }
        public String AGREEMENT_NO { set; get; }
        public String EXP_DATE { set; get; }
        //END ADD BY HERMAN
    }
}