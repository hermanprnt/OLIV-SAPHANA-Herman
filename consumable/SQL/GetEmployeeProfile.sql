DECLARE @@sqlstate varchar(max);
DECLARE @@Name varchar(100);
DECLARE @@Position varchar(100);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

SET @@sqlstate = '';
SET @@Name = @Name;
SET @@Position = @Position;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	SELECT 
		ROW_NUMBER () OVER (ORDER BY NOREG,NAME) AS Number, 
		NOREG
		,NAME
		,DIRECTORATE
		,DIVISION
		,DEPARTMENT
		,SECTION
		,LINE
		,[GROUP]
		,UNIT_CODE
		,MAIN_LOCATION
		,SUB_LOCATION
		,EMAIL
		,PHONE_EXT
		, POSITION
	FROM 
		vw_employee_profile
	where 1=1
		';

IF(@@Name <> '' AND @@Name is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and NAME like ''%'+ @Name + '%'' ';
END

IF(@@Position <> '' AND @@Position is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and POSITION IN (''Section Head'',''Line Head'')';
END



SET @@sqlstate = @@sqlstate + '
	) TB WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;


execute(@@sqlstate);