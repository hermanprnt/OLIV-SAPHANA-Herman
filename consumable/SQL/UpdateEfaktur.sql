
exec dbo.UPDATE_USED_FLAG_EFAKTUR
	 @@TAX_INVOICE_NO = @TAX_INVOICE_NO,	
	 @@IS_USED = @IS_USED,
	 @@CHANGED_BY = @CHANGED_BY,
	 @@INVOICE_NO = @INVOICE_NO