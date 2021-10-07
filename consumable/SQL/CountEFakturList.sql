DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)      
	FROM 
		TB_R_VAT_IN_H
	WHERE 1=1 ';

IF(@TAX_INVOICE_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND TAX_INVOICE_NO like ''%'+ @TAX_INVOICE_NO + '%'' ';
END

IF(@TAX_INVOICE_DT_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(TAX_INVOICE_DT as DATE) >= CAST(CONVERT(datetime, ''' + @TAX_INVOICE_DT_FROM + ''' , 104) as DATE) ';
END

IF(@TAX_INVOICE_DT_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(TAX_INVOICE_DT as DATE) <= CAST(CONVERT(datetime, ''' + @TAX_INVOICE_DT_TO + ''' , 104) as DATE) ';
END

IF(@VEND_CODE <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND SUPPLIER_CODE = '''+ @VEND_CODE + ''' ';
END


print(@@sqlstate);

execute(@@sqlstate);