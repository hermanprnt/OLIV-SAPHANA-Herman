
declare @@SQL varchar(5000)

set @@SQL ='';


set @@SQL =@@SQL+
			'SELECT 
	COUNT(*) 
FROM 
	TB_R_VAT_IN_H h
	WHERE 1=1';

		
IF(@Parameter <> '')
	BEGIN
		SET @@SQL = @@SQL + ' and h.TAX_INVOICE_NO = '''+@Parameter+''' ';
	END	
ELSE
	BEGIN
		SET @@SQL = @@SQL + ' and h.SUPPLIER_CODE = '''+@Supplier_cd+''' ';
	END

print(@@SQL);

execute(@@SQL);