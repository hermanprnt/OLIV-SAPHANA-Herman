UPDATE 
	TB_M_SYSTEM
SET 
    SYSTEM_VALUE_TEXT = @SYSTEM_VALUE_TEXT
	,SYSTEM_VALUE_NUM = @SYSTEM_VALUE_NUM
	,SYSTEM_VALUE_DT = @SYSTEM_VALUE_DT
	,SYSTEM_DESC = @SYSTEM_DESC
	,[UPDATED_BY] = @CHANGED_BY
	,[UPDATED_DT] = getdate()
WHERE 
	1 = 1
	and SYSTEM_ID = @SYSTEM_ID
