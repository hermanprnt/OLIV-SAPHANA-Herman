DECLARE @@sqlstate varchar(max);
DECLARE @@INVOICE_ID varchar(100);

SET @@sqlstate = '';
SET @@INVOICE_ID = @INVOICE_ID;

SET @@sqlstate = @@sqlstate + '
	SELECT 
      gr.[INVOICE_ID]
      ,I.[INVOICE_NO]
	  ,convert(varchar(15),gr.CREATED_DT,104)[POSTED_DT_STR]
      ,gr.[MESSAGE]
	  ,gr.CREATED_DT
FROM [TB_R_INVOICE_SAP_OUTPUT] gr JOIN TB_R_INVOICE I
ON 	gr.INVOICE_ID = I.INVOICE_ID
	where 1=1
		';

IF(@@INVOICE_ID <> '' AND @@INVOICE_ID is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.INVOICE_ID = '''+ @@INVOICE_ID + ''' ';
END


SET @@sqlstate = @@sqlstate + '
	order by [CREATED_DT] DESC ';

execute(@@sqlstate);
