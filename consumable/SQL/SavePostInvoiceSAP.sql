update 
	TB_R_INVOICE
set 
	ON_PROCESS_SAP_POST = 'Y',
	TOTAL_AMOUNT = @TOTAL_AMOUNT,
	PPV_GL_ACCOUNT = @PPV_ACCOUNT,
	PPV_AMOUNT = @PPV_AMOUNT,
	POSTED_BY = @POSTED_BY,
	POSTED_DT = @POSTED_DT,
	UPDATED_BY = @UPDATED_BY,
	UPDATED_DT = getdate()
where 
	1=1
and INVOICE_ID = @INVOICE_ID

