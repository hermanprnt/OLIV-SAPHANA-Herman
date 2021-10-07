DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_M_SUPPLIER				
	WHERE 
		SUPPLIER_CD = ''' + @SUPPLIER_CD + ''' '

execute(@@sqlstate);