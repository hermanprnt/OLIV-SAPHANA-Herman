DECLARE @@sqlstate varchar(max);
DECLARE @@INVOICE_ID varchar(100);


SET @@sqlstate = '';
SET @@INVOICE_ID = @INVOICE_ID;

SET @@sqlstate = @@sqlstate + '
	SELECT 
	[PO_NUMBER]
      ,[PO_ITEM]
      ,[MATDOC_YEAR]
      ,[MATDOC_NUMBER]
      ,[MATDOC_ITEM]
      ,[MATDOC_DATE]
      ,[SPC_STOCK]
      ,[MATDOC_AMOUNT]
      ,[VEND_CODE]
      ,[PURCHDOC_PRICE]
      ,[MAT_NUMBER]
      ,[MAT_DESCR] as PO_TEXT
      ,[PLANT_CODE]
      ,[SLOC_CODE]
      ,[MATDOC_UNIT]
      ,[CANCEL]
      ,[ORI_MAT_NUMBER]
      ,[MATDOC_CURRENCY]
      ,[MATDOC_QTY]
      ,gr.[TAX_CODE]
      ,[PO_DATE]
      ,[HEADER_TEXT]
      ,[IR_NO]
      ,[MOV_TYPE]
      ,[REF_DOC]
      ,[USRID]
      ,[GR_STATUS]
      ,[CANCEL_DOC_NO]
      ,[LAST_SYNC_FLAG]
      ,[NOTIF_FLAG]
      ,inv.INVOICE_ID
	  ,CALCULATE_TAX
	  ,STAMP_AMOUNT	
	FROM TB_R_GR_IR_FROM_SAP gr
	left join TB_R_INVOICE inv on inv.INVOICE_ID = gr.INVOICE_ID
	where 1=1
		';


IF(@@INVOICE_ID <> '' AND @@INVOICE_ID is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.INVOICE_ID = '''+ @@INVOICE_ID + ''' ';
END


SET @@sqlstate = @@sqlstate + '
	order by [PO_NUMBER], [PO_ITEM], [MATDOC_NUMBER] ';

print(@@sqlstate);

execute(@@sqlstate);
