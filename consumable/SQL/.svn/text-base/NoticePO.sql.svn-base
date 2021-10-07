UPDATE 
	TB_R_PO_HEADER
SET 
	NOTICE_DATE = @NoticeDate
	,NOTICE_REMARK = @Remarks
	,NOTICE_BY = @NoticeBy
	,UPDATED_DT = getdate()
	,UPDATED_BY = @ChangeBy
WHERE 
	1 = 1
	and PO_NUMBER = @PoNumber
	and VEND_CODE = @VendCode

