DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	DELETE FROM 
		TB_R_RF_REGISTER_DTL				
	WHERE 
		RF_DTL_NO = ''' + @RfDtlNo + ''' '

execute(@@sqlstate);