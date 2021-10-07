
exec dbo.SP_PO_TEMPORARY_PROCESS 
	@@UserLogin = @LOGIN_USER_NAME,
	@@selectedKeys = @SELECTED_KEYS