﻿INSERT INTO TB_M_BASELINE_DATE
(          
     [HOLIDAY_DATE]
    ,[BASELINE_DATE]
	,[CREATED_BY]
    ,[CREATED_DT]
) 
VALUES 
(
	@HOLIDAY_DATE,
    @BASELINE_DATE,
	@CREATED_BY,    
    GETDATE()
);