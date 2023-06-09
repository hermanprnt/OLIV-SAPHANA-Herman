﻿DECLARE @@WHT VARCHAR(10) = ''

IF ((SELECT WITHOLDING_TAX_CD from TB_R_INVOICE WHERE INVOICE_NO = @INVOICE_NO) IS NOT NULL)
BEGIN 
	SELECT @@WHT = WITHOLDING_TAX_CD from TB_R_INVOICE WHERE INVOICE_NO = @INVOICE_NO
END
ELSE
BEGIN
	SELECT @@WHT = B.WITHOLDING_CODE from TB_R_INVOICE A JOIN TB_M_SUPPLIER B 
	ON A.SUPPLIER_CD = A.SUPPLIER_CD
	WHERE A.INVOICE_NO = @INVOICE_NO AND A.SUPPLIER_CD = B.SUPPLIER_CD
END

SELECT @@WHT