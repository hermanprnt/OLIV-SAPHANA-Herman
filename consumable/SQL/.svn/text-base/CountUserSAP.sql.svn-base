DECLARE @@sqlstate varchar(max);
DECLARE @@USERNAME  varchar(50);
DECLARE @@COMPLETE_NAME  varchar(50);
DECLARE @@NOREG  varchar(50);
DECLARE @@GROUP  varchar(50);

SET @@sqlstate = '';
SET @@USERNAME = @USERNAME;
SET @@COMPLETE_NAME = @COMPLETE_NAME;
SET @@NOREG = @NOREG;
SET @@GROUP = @GROUP;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM 
		TB_M_USER_SAP
	where 1=1 ';
	
IF(@@USERNAME <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and USERNAME like ''%'+ @@USERNAME + '%'' ';
END

IF(@@COMPLETE_NAME <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and COMPLETE_NAME like ''%'+ @@COMPLETE_NAME + '%'' ';
END

IF(@@NOREG <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and NOREG like ''%'+ @@NOREG + '%'' ';
END

IF(@@GROUP <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and [GROUP] like ''%'+ @@GROUP + '%'' ';
END

print(@@sqlstate);

execute(@@sqlstate);

