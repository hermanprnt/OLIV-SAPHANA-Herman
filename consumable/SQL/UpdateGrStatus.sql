update 
	TB_R_GR_IR_FROM_SAP
set 
	GR_STATUS = @GR_STATUS,
	UPDATED_BY = @UPDATED_BY,
	UPDATED_DT = getdate()
where 
	1=1
and 
	MATDOC_NUMBER = @MATDOC_NUMBER