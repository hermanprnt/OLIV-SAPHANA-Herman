﻿UPDATE 
	TB_M_BASELINE_DATE
SET 
	 [HOLIDAY_DATE] = @HOLIDAY_DATE
	,[BASELINE_DATE] = @BASELINE_DATE
	,[UPDATED_BY] = @CHANGED_BY
	,[UPDATED_DT] = getdate()
WHERE 
	1 = 1
	AND HOLIDAY_DATE = @HOLIDAY_DATE_OLD