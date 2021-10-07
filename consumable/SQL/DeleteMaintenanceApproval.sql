DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_M_MAINTENANCE_APPROVAL				
	WHERE 
		MAINTENANCE_APPROVAL_ID = ''' + @MAINTENANCE_APPROVAL_ID + ''' '

execute(@@sqlstate);
