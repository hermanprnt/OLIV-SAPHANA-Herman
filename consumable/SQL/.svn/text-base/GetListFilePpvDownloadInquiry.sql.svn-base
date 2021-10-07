DECLARE @@sqlstate varchar(max);
DECLARE @@INVOICE_ID varchar(100);


SET @@sqlstate = '';
SET @@INVOICE_ID = @INVOICE_ID;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		[FILE_NAME]
		,FILE_NAME_SERVER
	FROM TB_R_PPV_ATTACHMENT
	WHERE 1=1';


IF(@@INVOICE_ID <> '' AND @@INVOICE_ID is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND INVOICE_ID = '''+ @@INVOICE_ID + ''' ';
END

print(@@sqlstate);

execute(@@sqlstate);
