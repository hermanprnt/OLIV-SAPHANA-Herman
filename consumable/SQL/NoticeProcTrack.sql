UPDATE 
	TB_R_PROCUREMENT_TRACKING
SET 
	NOTICE_DATE = @NoticeDate
	,REMARKS = @Remarks
	,NOTICE_BY = @NoticeBy
	,UPDATED_DT = getdate()
	,UPDATED_BY = @ChangeBy
WHERE 
	1 = 1
	and TAX_INVOICE_NO = @TaxInvoiceNo

