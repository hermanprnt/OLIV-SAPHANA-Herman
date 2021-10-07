DECLARE @@sqlstate varchar(max);
DECLARE @@SYSTEM_CD  varchar(50);
DECLARE @@SYSTEM_TYPE  varchar(50);
DECLARE @@SYSTEM_VALUE_TEXT  varchar(50);

SET @@sqlstate = '';
SET @@SYSTEM_CD = @SYSTEM_CD;
SET @@SYSTEM_TYPE = @SYSTEM_TYPE;
SET @@SYSTEM_VALUE_TEXT = @SYSTEM_VALUE_TEXT;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM 
		TB_M_SYSTEM
	where 1=1 ';
	
IF(@@SYSTEM_CD <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and SYSTEM_CD like ''%'+ @@SYSTEM_CD + '%'' ';
END

IF(@@SYSTEM_TYPE <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and SYSTEM_TYPE like ''%'+ @@SYSTEM_TYPE + '%'' ';
END

IF(@@SYSTEM_VALUE_TEXT <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and SYSTEM_VALUE_TEXT like ''%'+ @@SYSTEM_VALUE_TEXT + '%'' ';
END

print(@@sqlstate);

execute(@@sqlstate);

