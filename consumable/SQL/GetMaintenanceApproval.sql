DECLARE @@sqlstate varchar(max);
DECLARE @@SUPPLIER_CD  varchar(50);
DECLARE @@SUPPLIER_NAME  varchar(50);
DECLARE @@APPROVER_UNIT_CD  varchar(50);
DECLARE @@NumberFrom varchar(4);
DECLARE @@NumberTo varchar(4);

SET @@sqlstate = '';
SET @@SUPPLIER_CD = @SUPPLIER_CD;
SET @@SUPPLIER_NAME = @SUPPLIER_NAME;
SET @@APPROVER_UNIT_CD = @APPROVER_UNIT_CD;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	select 
		ROW_NUMBER () OVER (ORDER BY SUPPLIER_NAME) AS Number
		,MAINTENANCE_APPROVAL_ID
		,ma.SUPPLIER_CD
		,ma.APPROVER_UNIT_CD
		,s.SUPPLIER_NAME
		,emp.NAME as APPROVER_NAME
	from 
		TB_M_MAINTENANCE_APPROVAL ma
	left join TB_M_SUPPLIER s on s.SUPPLIER_CD = ma.SUPPLIER_CD
	left join VW_EMPLOYEE_PROFILE emp on emp.UNIT_CODE = ma.APPROVER_UNIT_CD and emp.POSITION = ''Section Head''
	where 1=1 ';
	
IF(@@SUPPLIER_CD <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and ma.SUPPLIER_CD like ''%'+ @@SUPPLIER_CD + '%'' ';
END

IF(@@SUPPLIER_NAME <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and SUPPLIER_NAME like ''%'+ @@SUPPLIER_NAME + '%'' ';
END

IF(@@APPROVER_UNIT_CD <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and APPROVER_UNIT_CD like ''%'+ @@APPROVER_UNIT_CD + '%'' ';
END


SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo ;
	

print(@@sqlstate);

execute(@@sqlstate);
