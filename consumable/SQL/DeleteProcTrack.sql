DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_R_PROCUREMENT_TRACKING				
	WHERE 
		TAX_INVOICE_NO = ''' + @TaxInvoiceNo + ''' '

execute(@@sqlstate);