DECLARE @@sqlstate varchar(max);
DECLARE @@NOTICE_FLAG  varchar(1);
DECLARE @@SUPPLIER_CD  varchar(50);

SET @@sqlstate = '';
SET @@NOTICE_FLAG = @NOTICE_FLAG;
SET @@SUPPLIER_CD = @SUPPLIER_CD;

SET @@sqlstate = @@sqlstate + '
	SELECT 
	COUNT(*)
	FROM TB_R_INVOICE
	WHERE NOTICE_FLAG = ''' + @@NOTICE_FLAG + ''' ';

IF(@@SUPPLIER_CD <> '' AND @@SUPPLIER_CD IS NOT NULL)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND SUPPLIER_CD = '''+ @@SUPPLIER_CD+''' ';
END

EXECUTE(@@sqlstate);