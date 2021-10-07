DECLARE @@sqlstate varchar(max);
DECLARE @@GL_ACCOUNT_NO varchar(100);


SET @@sqlstate = '';
SET @@GL_ACCOUNT_NO = @GL_ACCOUNT_NO;


SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM TB_M_GL_ACCOUNT 
	where 1=1
	  ';

IF(@@GL_ACCOUNT_NO <> '' AND @@GL_ACCOUNT_NO is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and GL_ACCOUNT_NO like ''%'+ @@GL_ACCOUNT_NO + '%'' ';
END


execute(@@sqlstate);
