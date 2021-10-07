DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_M_SYSTEM				
	WHERE 
		SYSTEM_ID = ''' + @SYSTEM_ID + ''' '

execute(@@sqlstate);
