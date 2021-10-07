DECLARE @@sqlstate nvarchar(max);
DECLARE @@PO_NUMBER  varchar(50);

SET @@sqlstate = '';
SET @@PO_NUMBER = @PO_NUMBER;

SET @@sqlstate = @@sqlstate + '
	SELECT poi.[PO_NUMBER]
		  ,poi.MAT_NUMBER
		  ,poi.PO_QTY_NEW
		  ,poi.MAT_DESCR
		  ,poi.PO_UNIT 
		  ,poi.PO_ITEM 
		  ,poi.PRICE_PER_UNIT
		  ,(poi.PO_QTY_NEW  * poi.PRICE_PER_UNIT) as TOTAL_AMOUNT_ITEM
	FROM TB_R_PO_ITEM poi
	where 1=1
		';

IF(@@PO_NUMBER <> '' AND @@PO_NUMBER is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and poi.PO_NUMBER = '''+ @@PO_NUMBER + ''' ';
END

SET @@sqlstate = @@sqlstate + '
	order by PO_ITEM
		';


execute(@@sqlstate);
