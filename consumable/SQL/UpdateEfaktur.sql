DECLARE @@TAX_13 VARCHAR(20);

SET @@TAX_13 = RIGHT(@TAX_INVOICE_NO, 13);

exec dbo.UPDATE_USED_FLAG_EFAKTUR
	 @@TAX_INVOICE_NO = @TAX_INVOICE_NO,	
	 @@IS_USED = @IS_USED,
	 @@CHANGED_BY = @CHANGED_BY,
	 @@INVOICE_NO = @INVOICE_NO