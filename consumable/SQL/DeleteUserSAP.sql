DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_M_USER_SAP				
	WHERE 
		USER_ID = ''' + @USER_ID + ''' '

execute(@@sqlstate);
