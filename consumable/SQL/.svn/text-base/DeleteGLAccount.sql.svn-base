DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_M_GL_ACCOUNT			
	WHERE 
		GL_ACCOUNT_ID = ''' + @GL_ACCOUNT_ID + ''' '

execute(@@sqlstate);
