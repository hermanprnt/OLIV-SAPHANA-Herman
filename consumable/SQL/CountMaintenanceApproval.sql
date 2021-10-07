DECLARE @@sqlstate varchar(max);
DECLARE @@SUPPLIER_CD  varchar(50);
DECLARE @@SUPPLIER_NAME  varchar(50);
DECLARE @@APPROVER_UNIT_CD  varchar(50);

SET @@sqlstate = '';
SET @@SUPPLIER_CD = @SUPPLIER_CD;
SET @@SUPPLIER_NAME = @SUPPLIER_NAME;
SET @@APPROVER_UNIT_CD = @APPROVER_UNIT_CD;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM 
		TB_M_MAINTENANCE_APPROVAL ma
	left join TB_M_SUPPLIER s on s.SUPPLIER_CD = ma.SUPPLIER_CD
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

print(@@sqlstate);

execute(@@sqlstate);

