DECLARE @@sqlstate varchar(max);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);
DECLARE @@cnt int = 1;
DECLARE @@max int = (select count(*) from fnSplitString(@Parameter,''));

SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = '
	SELECT * FROM 
	(
		SELECT 
			ROW_NUMBER () OVER (ORDER BY SUPPLIER_ID) AS Number, * 
		FROM 
			TB_M_SUPPLIER ';

IF @@max > 0
	set @@sqlstate = @@sqlstate + ' WHERE ';
	
WHILE @@cnt <= @@max
BEGIN
	DECLARE @@tempValueRow varchar(max) = (
        select splitdata 
        from (select *
            , ROW_NUMBER() OVER(ORDER BY (select 1)) as RowId 
            from fnSplitString(@Parameter,'')
        ) T1 
    where T1.RowId = @@cnt
    );
	
	set @@sqlstate = @@sqlstate +	
		'(SUPPLIER_CD LIKE ''%' + @@tempValueRow + '%'' OR SUPPLIER_NAME LIKE ''%' + @@tempValueRow + '%'') ';
	
	IF @@cnt < @@max
		set @@sqlstate = @@sqlstate + 'AND ';
			
set @@cnt = @@cnt+1;
END			
	
	set @@sqlstate = @@sqlstate + ') TB WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;

execute(@@sqlstate);
	