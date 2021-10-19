

INSERT INTO [TB_R_INVOICE_SAP_INPUT]
		([INVOICE_ID]
		,[INV_DATE]
		,[POST_DATE]
		,[REF_INV]
		,[AMOUNT]
		,[TAX_AMT]
		,[ITEM_TEXT]
		,[PO_NUMBER]
		,[PO_ITEM]
		,[BASE_DATE]
		,[PAY_METHOD]
		,[HEAD_TEXT]
		,[TERM_PAY]
		,[TAX_CODE]
		,[BVTYP]
		,[TAX_DATE]
		,[ON_PROCESS]
		,[CREATED_DT]
		,[CREATED_BY]
		)
	VALUES
		(@INVOICE_ID, 
		@INV_DATE, 
		@POST_DATE, 
		@REF_INV,
		@AMOUNT, 
		@TAX_AMT, 
		@ITEM_TEXT,
		@PO_NUMBER,
		@PO_ITEM, 
		@BASE_DATE, 
		@PAY_METHOD, 
		@HEAD_TEXT, 
		@TERM_PAY, 
		@TAX_CODE,
		@BVTYP,
		@TAX_DATE,
		'Y',
		getdate(),
		@CREATED_BY
		)