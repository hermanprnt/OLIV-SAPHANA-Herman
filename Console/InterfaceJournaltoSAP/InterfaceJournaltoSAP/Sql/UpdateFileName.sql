﻿
UPDATE A
SET A.SYSTEM_DESC = @filename
FROM TB_M_SYSTEM A WHERE SYSTEM_TYPE = 'BH00021' and SYSTEM_CD = 'INTERFACE_NAME' 
