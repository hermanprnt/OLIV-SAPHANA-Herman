UPDATE 
	TB_R_INVOICE
SET 
	NOTICE_DATE = @NoticeDate
	,NOTICE_REMARK = @Remarks
	,NOTICE_BY = @NoticeBy
	,UPDATED_DT = getdate()
	,UPDATED_BY = @ChangeBy
WHERE 
	1 = 1
	AND INVOICE_ID = @InvoiceId

