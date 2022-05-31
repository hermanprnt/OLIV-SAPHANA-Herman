
declare @@SQL varchar(5000)

declare @@val varchar(3)
SELECT @@val = SYSTEM_VALUE_NUM FROM PROCUREMENT.dbo.TB_M_SYSTEM WHERE SYSTEM_CD = 'EFAKTUR_EXPIRED' and SYSTEM_TYPE = 'CREATE_INV'
set @@SQL ='';


set @@SQL =@@SQL+
			'SELECT 
	COUNT(*) 
FROM 
	TB_R_VAT_IN_H h
	WHERE ISNULL(USED, ''N'') != ''Y''
		AND DATEADD(MONTH, '+@@val+', TAX_INVOICE_DT) >= GETDATE()';

		
IF(@Parameter <> '')
	BEGIN
		--SET @@SQL = @@SQL + ' and h.TAX_INVOICE_NO = '''+@Parameter+''' '; /*2021-12-07*/
		SET @@SQL = @@SQL + ' and (h.TAX_INVOICE_NO = '''+@Parameter+''' or h.SAP_TAX_INVOICE_NO = '''+@Parameter+''') ';
	END	
ELSE
	BEGIN
		SET @@SQL = @@SQL + ' and h.SUPPLIER_CODE = '''+substring(@Supplier_cd, patindex('%[^0]%',@Supplier_cd), 10)+''' ';
	END

print(@@SQL);

execute(@@SQL);