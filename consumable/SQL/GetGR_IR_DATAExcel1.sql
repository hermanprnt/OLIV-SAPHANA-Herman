DECLARE @@sqlstate varchar(max);
DECLARE @@DN_NO  varchar(50);
DECLARE @@PO_NO  varchar(50);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@PO_DATE_FROM  varchar(50);
DECLARE @@PO_DATE_TO  varchar(50);


SET @@sqlstate = '';
SET @@DN_NO = @DN_NO;
SET @@PO_NO = @PO_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@PO_DATE_FROM = @PO_DATE_FROM;
SET @@PO_DATE_TO = @PO_DATE_TO;


SET @@sqlstate = @@sqlstate + '
	select 
		gr.PO_NUMBER
		,gr.PO_ITEM
		,gr.DN_NO_ITEM
		,gr.DN_NO
		,gr.MATDOC_YEAR
		,gr.MATDOC_NUMBER
		,gr.MATDOC_ITEM
		,gr.MATDOC_DATE
		,gr.SPC_STOCK
		,gr.MATDOC_AMOUNT
		,gr.VEND_CODE
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
		,case when gr.PO_CATEGORY = ''GOODS''
			  then ''Goods''
			  when gr.PO_CATEGORY = ''SERVICE''
			  then ''Service''
			  else ''''
		 end [MATERIAL]
		,gr.HEADER_TEXT
		,gr.IR_NO
		,gr.MOV_TYPE
		,gr.REF_DOC
		,case when left(gr.MATDOC_NUMBER, 1) = ''5''
			  then ''PAS''
			  when left(gr.MATDOC_NUMBER, 1) = ''9''
			  then ''EWH''
			  else ''''
		 end [SYSTEM]
		,gr.CANCEL_DOC_NO
		,gr.INVOICE_ID
		, s.SUPPLIER_NAME
	from 
		TB_R_GR_IR_FROM_SAP gr
	left join TB_M_SUPPLIER s on s.SUPPLIER_CD = gr.VEND_CODE
	where 1=1 
		and MATDOC_QTY > 0
		and (GR_STATUS = ''APPROVED'')
		and INVOICE_ID IS NULL
		 ';

IF(@@DN_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.DN_NO like ''%'+ @@DN_NO + '%'' ';
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
ORDER BY PO_NUMBER, PO_ITEM, MATDOC_DATE, MATDOC_NUMBER desc ';

print(@@sqlstate);

execute(@@sqlstate);
