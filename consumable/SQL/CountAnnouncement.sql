DECLARE @@sqlstate varchar(max);
DECLARE @@DOC_TITLE varchar(100)
		,@@STATUS VARCHAR(MAX)
		,@@RELEASE_DATE_FROM VARCHAR(50)
		,@@RELEASE_DATE_TO VARCHAR(50)


SET @@sqlstate = '';
SET @@DOC_TITLE = @DOC_TITLE;
SET @@RELEASE_DATE_FROM = @RELEASE_DATE_FROM
SET @@RELEASE_DATE_TO = @RELEASE_DATE_TO;
SET @@STATUS = @STATUS


SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM TB_M_ANNOUNCEMENT
	where 1=1
	  ';

IF(@@DOC_TITLE <> '' AND @@DOC_TITLE is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND ANNOUNCEMENT_TITLE IN ('+@@DOC_TITLE+') '
END

IF(@@RELEASE_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND CAST(DATE_RELEASE AS DATE) >= CAST(CONVERT(DATETIME, ''' + @@RELEASE_DATE_FROM + ''' , 104) AS DATE)'
END

IF(@@RELEASE_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND CAST(DATE_RELEASE AS DATE) <= CAST(CONVERT(DATETIME, ''' + @@RELEASE_DATE_TO + ''' , 104) AS DATE)'
END

IF(@@STATUS <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND STATUS IN ('+@@STATUS+') '
END


execute(@@sqlstate);
