
UPDATE 
	TB_R_INVOICE
SET 
	[STATUS] = 'CANCELLED'
	,GR_CANCEL = NULL
	,UPDATED_DT = getdate()
	,UPDATED_BY = @UPDATED_BY
WHERE
	1 = 1
and INVOICE_ID = @INVOICE_ID



update 
	TB_R_GR_IR_FROM_SAP
set 
	INVOICE_ID = NULL
	,UPDATED_BY = @UPDATED_BY
	,UPDATED_DT = getdate()
where 
	1=1
and 
	INVOICE_ID = @INVOICE_ID

/* old
update 
	TB_R_VAT_IN_H
set 
	IS_USED = '0'
	,CHANGED_BY = @UPDATED_BY
	,CHANGED_DT = getdate()
where 
	1=1
and 
	TAX_INVOICE_NO = @TAX_INVOICE_NO
*/

