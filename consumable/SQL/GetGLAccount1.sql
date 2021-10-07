DECLARE @@sqlstate varchar(max);
DECLARE @@GL_ACCOUNT_NO varchar(100);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

SET @@sqlstate = '';
SET @@GL_ACCOUNT_NO = @GL_ACCOUNT_NO;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	SELECT 
		ROW_NUMBER () OVER (ORDER BY CATEGORY_CD asc, GL_ACCOUNT_NO asc) AS Number
	  ,GL_ACCOUNT_ID
	  ,GL_ACCOUNT_NO
	  ,CATEGORY_CD
      ,TYPE
      ,NAME
      ,CODE
      ,PERCENTAGE
	  FROM TB_M_GL_ACCOUNT
	where 1=1
		';

IF(@@GL_ACCOUNT_NO <> '' AND @@GL_ACCOUNT_NO is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and GL_ACCOUNT_NO like ''%'+ @@GL_ACCOUNT_NO + '%'' ';
END


SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;

execute(@@sqlstate);
