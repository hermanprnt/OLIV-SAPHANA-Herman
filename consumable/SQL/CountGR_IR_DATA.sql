DECLARE @@sqlstate varchar(max);
DECLARE @@MATDOC_NO  varchar(50);
DECLARE @@PO_NO  varchar(50);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@RECEIVING_DATE_FROM  varchar(50);
DECLARE @@RECEIVING_DATE_TO  varchar(50);
DECLARE @@PO_DATE_FROM  varchar(50);
DECLARE @@PO_DATE_TO  varchar(50);

SET @@sqlstate = '';
SET @@MATDOC_NO = @MATDOC_NO;
SET @@PO_NO = @PO_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@RECEIVING_DATE_FROM = @RECEIVING_DATE_FROM;
SET @@RECEIVING_DATE_TO = @RECEIVING_DATE_TO;
SET @@PO_DATE_FROM = @PO_DATE_FROM;
SET @@PO_DATE_TO = @PO_DATE_TO;


SET @@sqlstate = @@sqlstate + '
	select 
		count(*)
	from 
		TB_R_GR_IR_FROM_SAP gr
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


print(@@sqlstate);

execute(@@sqlstate);
