
declare @@SQL varchar(5000)

set @@SQL ='';

SET @@SQL = @@SQL + '
	SELECT * FROM 
	( ';

set @@SQL =@@SQL+
			'SELECT 
	ROW_NUMBER () OVER (ORDER BY h.TAX_INVOICE_NO) AS Number, * 
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

SET @@SQL = @@SQL + '
	) REFF WHERE Number BETWEEN ' + cast(@NumberFrom as varchar(3)) + ' AND ' + cast(@NumberTo as varchar(3));

print(@@SQL);

execute(@@SQL);