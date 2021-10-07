DECLARE @@sqlstate varchar(max);
DECLARE @@MATDOC_NO  varchar(50);
DECLARE @@PO_NO  varchar(50);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@RECEIVING_DATE_FROM  varchar(50);
DECLARE @@RECEIVING_DATE_TO  varchar(50);
DECLARE @@PO_DATE_FROM  varchar(50);
DECLARE @@PO_DATE_TO  varchar(50);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

SET @@sqlstate = '';
SET @@MATDOC_NO = @MATDOC_NO;
SET @@PO_NO = @PO_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@RECEIVING_DATE_FROM = @RECEIVING_DATE_FROM;
SET @@RECEIVING_DATE_TO = @RECEIVING_DATE_TO;
SET @@PO_DATE_FROM = @PO_DATE_FROM;
SET @@PO_DATE_TO = @PO_DATE_TO;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;


SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	select 
		ROW_NUMBER () OVER (ORDER BY MATDOC_DATE) AS Number
		,gr.PO_NUMBER
		,gr.PO_ITEM
		,gr.MATDOC_YEAR
		,gr.MATDOC_NUMBER
		,gr.MATDOC_ITEM
		,gr.MATDOC_DATE
		,gr.SPC_STOCK
		,gr.MATDOC_AMOUNT
		,gr.VEND_CODE
		,s.SUPPLIER_NAME
		,gr.PURCHDOC_PRICE
		,gr.MAT_NUMBER
		,gr.MAT_DESCR
		,gr.PLANT_CODE
		,gr.SLOC_CODE
		,gr.MATDOC_UNIT
		,gr.CANCEL
		,gr.ORI_MAT_NUMBER
		,gr.MATDOC_CURRENCY
		,gr.MATDOC_QTY
		,gr.TAX_CODE
		,gr.PO_DATE
		,gr.GR_STATUS
		,gr.HEADER_TEXT
		,gr.IR_NO
		,gr.MOV_TYPE
		,gr.REF_DOC
		,gr.USRID
		,gr.CANCEL_DOC_NO
	from 
		TB_R_GR_IR_FROM_SAP gr
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = gr.VEND_CODE
	where 1=1 
		and MOV_TYPE = ''101''
		and GR_STATUS <> ''CANCEL'' ';

IF(@@MATDOC_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.MATDOC_NUMBER like ''%'+ @@MATDOC_NO + '%'' ';
END

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

IF(@@RECEIVING_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.MATDOC_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@RECEIVING_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@RECEIVING_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.MATDOC_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@RECEIVING_DATE_TO + ''' , 104) as DATE)';
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

SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;


print(@@sqlstate);

execute(@@sqlstate);
