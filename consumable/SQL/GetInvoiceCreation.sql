DECLARE @@sqlstate varchar(max);
DECLARE @@PO_NO  varchar(50);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@PO_DATE_FROM  varchar(50);
DECLARE @@PO_DATE_TO  varchar(50);
DECLARE @@PO_TEXT varchar(MAX);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

SET @@sqlstate = '';
SET @@PO_NO = @PO_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@PO_DATE_FROM = @PO_DATE_FROM;
SET @@PO_DATE_TO = @PO_DATE_TO;
SET @@PO_TEXT = @PO_TEXT;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;


SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	SELECT 
		ROW_NUMBER () OVER (ORDER BY PO_NUMBER, PO_ITEM ) AS Number
		, sum(MATDOC_QTY) AS TOTAL_QTY
		, sum(MATDOC_AMOUNT) AS TOTAL_AMOUNT		
		, PO_NUMBER
		, PO_ITEM
		, PO_DATE
		, MAT_DESCR as PO_TEXT
		, gr.VEND_CODE
		, s.SUPPLIER_NAME
		, GR_STATUS
		, MATDOC_CURRENCY
		, MATDOC_UNIT	
		, s.EDIT_AMOUNT_FLAG
		, INVOICE_ID	
	FROM TB_R_GR_IR_FROM_SAP gr
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = gr.VEND_CODE
	where 1=1
		and (GR_STATUS = ''APPROVED'' ) 
		and INVOICE_ID IS NULL ';

IF(@@PO_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.PO_NUMBER like ''%'+ @@PO_NO + '%'' ';
END

IF(@@SUPPLIER <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.VEND_CODE in ('+@@SUPPLIER+') ';
END

IF(@@STATUS <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.GR_STATUS in ('+@@STATUS+') ';
END

IF(@@PO_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.PO_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@PO_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@PO_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.PO_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@PO_DATE_TO + ''' , 104) as DATE)';
END

IF(@@PO_TEXT <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and MAT_DESCR like ''%'+ @@PO_TEXT + '%'' ';
END

SET @@sqlstate = @@sqlstate + '
	group by 
	PO_NUMBER, PO_ITEM,  PO_DATE, MAT_DESCR, gr.VEND_CODE
	, s.SUPPLIER_NAME, GR_STATUS, MATDOC_CURRENCY, MATDOC_UNIT
	, s.EDIT_AMOUNT_FLAG , INVOICE_ID 
	having sum(MATDOC_QTY) > 0
	';

SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;


print(@@sqlstate);

execute(@@sqlstate);
