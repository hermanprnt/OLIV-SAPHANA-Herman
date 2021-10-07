DECLARE @@sqlstate varchar(max);
DECLARE @@PO_NUMBER varchar(50);
DECLARE @@PO_ITEM  varchar(50);
DECLARE @@PO_DATE  varchar(50);
DECLARE @@VEND_CODE varchar(100);
DECLARE @@GR_STATUS  varchar(100);
DECLARE @@INVOICE_ID  varchar(100);

SET @@sqlstate = '';
SET @@PO_NUMBER = @PO_NUMBER;
SET @@PO_ITEM = @PO_ITEM;
SET @@PO_DATE = @PO_DATE;
SET @@VEND_CODE = @VEND_CODE;
SET @@GR_STATUS = @GR_STATUS;
SET @@INVOICE_ID = @INVOICE_ID;

SET @@sqlstate = @@sqlstate + '
	select 
		*
	from 
		TB_R_GR_IR_FROM_SAP 
	where 1=1 ';

IF(@@PO_NUMBER <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and PO_NUMBER = '''+ @@PO_NUMBER + ''' ';
END

IF(@@PO_ITEM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and PO_ITEM = '''+ @@PO_ITEM + ''' ';
END

IF(@@PO_DATE is not NULL)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(PO_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@PO_DATE + ''' , 104) as DATE) ';
END

IF(@@VEND_CODE <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and VEND_CODE = '''+ @@VEND_CODE + ''' ';
END

IF(@@GR_STATUS <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and GR_STATUS = '''+ @@GR_STATUS + ''' ';
END

IF(@@INVOICE_ID <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and INVOICE_ID = '''+ @@INVOICE_ID + ''' ';
END

IF(@@INVOICE_ID = '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and INVOICE_ID is NULL ' ;
END



SET @@sqlstate = @@sqlstate + '
	order by MATDOC_DATE asc, MATDOC_NUMBER ';


print(@@sqlstate);

execute(@@sqlstate);
