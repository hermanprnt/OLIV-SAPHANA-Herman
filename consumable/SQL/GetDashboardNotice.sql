DECLARE @@sqlstate varchar(max);
DECLARE @@NOTICE_FLAG  varchar(1);
DECLARE @@SUPPLIER_CD  varchar(50);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

SET @@sqlstate = '';
SET @@NOTICE_FLAG = @NOTICE_FLAG;
SET @@SUPPLIER_CD = @SUPPLIER_CD;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	(
		SELECT 
		ROW_NUMBER () OVER (ORDER BY INVOICE_DATE ASC) AS NUMBER,
		A.INVOICE_ID,
		A.SUPPLIER_CD,
		B.SUPPLIER_NAME,
		A.INVOICE_NO,
		A.INVOICE_DATE,
		A.TOTAL_AMOUNT,
		A.NOTICE_DATE,
		A.NOTICE_FLAG,
		A.CURRENCY
		FROM TB_R_INVOICE A
		JOIN TB_M_SUPPLIER B ON A.SUPPLIER_CD = B.SUPPLIER_CD
		WHERE A.NOTICE_FLAG = ''' + @@NOTICE_FLAG + ''' ';

IF(@@SUPPLIER_CD <> '' AND @@SUPPLIER_CD IS NOT NULL)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND A.SUPPLIER_CD = '''+ @@SUPPLIER_CD+''' ';
END

SET @@sqlstate = @@sqlstate + '
	) 
	REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo + 
	' ORDER BY INVOICE_DATE ASC';

EXECUTE(@@sqlstate);