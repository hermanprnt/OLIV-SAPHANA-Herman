DECLARE @@sqlstate varchar(max);
DECLARE @@Name varchar(100);
DECLARE @@Position varchar(100);

SET @@sqlstate = '';
SET @@Name = @Name;
SET @@Position = @Position;

SET @@sqlstate = @@sqlstate + '
SELECT 
	count(*)
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

execute(@@sqlstate);