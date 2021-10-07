DECLARE @@sqlstate varchar(max);
DECLARE @@SYSTEM_CD varchar(50);
DECLARE @@SYSTEM_TYPE varchar(50);
DECLARE @@SYSTEM_VALUE_TEXT varchar(50);
DECLARE @@NumberFrom varchar(4);
DECLARE @@NumberTo varchar(4);

SET @@sqlstate = '';
SET @@SYSTEM_CD = @SYSTEM_CD;
SET @@SYSTEM_TYPE = @SYSTEM_TYPE;
SET @@SYSTEM_VALUE_TEXT = @SYSTEM_VALUE_TEXT;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	select 
		ROW_NUMBER () OVER (ORDER BY SYSTEM_ID) AS Number
		,[SYSTEM_ID]
		,[SYSTEM_CD]
		,[SYSTEM_TYPE]
		,[SYSTEM_VALUE_TEXT]
		,[SYSTEM_VALUE_DT]
		,[SYSTEM_VALUE_NUM]
		,[SYSTEM_DESC]
		,[CREATED_DT]
		,[CREATED_BY]
		,[UPDATED_DT]
		,[UPDATED_BY]
	from 
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

SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo ;
	

print(@@sqlstate);

execute(@@sqlstate);
