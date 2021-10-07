DECLARE @@sqlstate varchar(max);
DECLARE @@PO_NUMBER varchar(100);


SET @@sqlstate = '';
SET @@PO_NUMBER = @PO_NUMBER;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		[FILE_NAME]
		,FILE_NAME_SERVER
	FROM TB_R_PO_ATTACHMENT
	WHERE 1=1';


IF(@@PO_NUMBER <> '' AND @@PO_NUMBER is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND PO_NUMBER = '''+ @@PO_NUMBER + ''' ';
END

print(@@sqlstate);

execute(@@sqlstate);
