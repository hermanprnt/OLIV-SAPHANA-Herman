﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toyota.Common.Web.Platform;

namespace consumable.Commons.Constants
{
    public class CommonConstant
    {
        // COMMON CONSTANTS
        public const string SCREEN_MODE_ADD = "ADD";
        public const string SCREEN_MODE_EDIT = "EDIT";

        public const string TABLEAU_IP = "10.33.1.108";

        // STATUS GR
        public const string STATUS_GR_RECEIVED = "RECEIVED";
        public const string STATUS_GR_APPROVED = "APPROVED";
        public const string STATUS_GR_CANCEL = "CANCEL";
        public const string STATUS_GR_POSTED = "POSTED";

        // STATUS INVOICE
        public const string STATUS_INVOICE_CREATED = "CREATED";
        public const string STATUS_INVOICE_REJECTED = "REJECTED";
        public const string STATUS_INVOICE_SUBMITTED = "SUBMITTED";
        public const string STATUS_INVOICE_POSTED = "POSTED";
        public const string STATUS_INVOICE_PAID = "PAID";
        public const string STATUS_INVOICE_CANCELLED = "CANCELLED";
        public const string STATUS_INVOICE_ERROR_POSTING = "ERROR_POSTING";
        public const string STATUS_INVOICE_ERROR_POSTING_NO_UNDERLINE = "ERROR POSTING";
        public const string STATUS_INVOICE_READY_TO_POST = "READY_TO_POST";
        public const string STATUS_INVOICE_READY_TO_POST_NO_UNDERLINE = "READY TO POST";         

        // RECEIVING STATUS INVOICE
        public const string RECEIVING_STATUS_INVOICE_ACCEPT = "ACCEPT";
        public const string RECEIVING_STATUS_INVOICE_REJECT = "REJECT";

        // SYSTEM PROPERTY CODE ATTR
        public const string SYSTEM_CD_UPLOAD_PATH_ANNOUNCEMENT = "UPLOAD_PATH_ANNOUNCEMENT";
        public const string SYSTEM_CD_UPLOAD_PATH_INVOICE = "UPLOAD_PATH_INVOICE";
        public const string SYSTEM_CD_UPLOAD_PATH_PPV = "UPLOAD_PATH_PPV";
        public const string SYSTEM_CD_UPLOAD_PATH_PO = "UPLOAD_PATH_PO";

        public const string SYSTEM_CD_EFAKTUR_EXPIRED = "EFAKTUR_EXPIRED";

        // SYSTEM PROPERTY TYPE ATTR
        public const string SYSTEM_TYPE_STATUS_GR = "STATUS_GR";
        public const string SYSTEM_TYPE_UPLOAD_PATH = "UPLOAD_PATH";
        public const string SYSTEM_TYPE_TAX_CODE = "TAX_CODE";
        public const string SYSTEM_TYPE_REVERSE_REASON = "REVERSE_REASON";

        public const string SYSTEM_TYPE_EFAKTUR_PATH = "UPLOAD_PATH_EFAKTUR";
        public const string SYSTEM_CD_EFAKTUR_PATH = "UPLOAD_PATH_EFAKTUR";

        public const string SYSTEM_TYPE_STATUS_INVOICE = "STATUS_INVOICE";
        public const string SYSTEM_TYPE_STATUS_HARDCOPY = "STATUS_HARDCOPY";

        public const string SYSTEM_TYPE_CREATE_INV = "CREATE_INV";

        // PROCESS TYPE INVOICE INQUIRY
        public const string PROCESS_TYPE_CANCEL_INVOICE = "CANCEL_INVOICE";
        public const string PROCESS_TYPE_POST_INVOICE = "POST_INVOICE";
        public const string PROCESS_TYPE_REVERSE_INVOICE = "REVERSE_INVOICE";
        
        public const string TERM_PAY = "TERM_PAY";
        public const string PAY_METHOD = "PAY_METHOD";

        public const string WITHOLDING_TAX = "WITHOLDING_TAX";
        public const string GL_ACCOUNT = "PPV";
        public const string STAMP = "STAMP";

        public const string STATUS_SUCCESS_SAP = "S";
        public const string STATUS_ERROR_SAP = "E";

        public const string TAX_CODE = "TAX_CODE";

        public const string DEFAULT_TAX_CODE = "DEFAULT_TAX_CODE";// add by fid.ahmad 16-03-2022

        public const string SYSTEM_TAX_CODE_V1 = "V1";

        public const string SYSTEM_CD_SUBMITTED_HARDCOPY = "SUBMITTED_HARDCOPY";
        public const string SYSTEM_TYPE_CREATE_INVOICE = "CREATE_INV";
        
        public const string SYSTEM_TYPE_ATTACHMENT = "ATTACHMENT";
        public const string SYSTEM_TYPE_ATTACHMENT_FILE_FORMAT = "ATTACHMENT_FILE_FORMAT";
        public const string SYSTEM_TYPE_ATTACHMENT_MAX_SIZE = "ATTACHMENT_MAX_SIZE";
        public const string SYSTEM_TYPE_ATTACHMENT_FORMAT_FILENAME = "ATTACHMENT_FORMAT_FILENAME";

        public const string SYSTEM_TYPE_STATUS_ANNOUNCEMENT = "STATUS_ANNOUNCEMENT";
        public const string SYSTEM_CODE_STATUS_ACTIVE = "ACTIVE";

        public const string AMOUNT_STAMP = "AMOUNT_STAMP";
        public const string AMOUNT_INVOICE = "AMOUNT_INVOICE";

        public const string SYSTEM_CD_RANGE_SUBMISSION_DATE_SEARCH = "RANGE_SUBMISSION_DATE_SEARCH";
        public const string SYSTEM_TYPE_INVOICE_INQUIRY = "INVOICE_INQUIRY";


    }
}
