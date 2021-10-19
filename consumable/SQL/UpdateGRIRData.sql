update 
	TB_R_GR_IR_FROM_SAP
set 
	INVOICE_ID = @INVOICE_ID,
	UPDATED_BY = @UPDATED_BY,
	UPDATED_DT = getdate()
where 
	1=1
	and PO_NUMBER = @PO_NUMBER
	and PO_ITEM = @PO_ITEM
	and VEND_CODE = @VEND_CODE
	and GR_STATUS = @GR_STATUS
	and INVOICE_ID is NULL