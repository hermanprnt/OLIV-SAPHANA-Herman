﻿DECLARE @@PLANT_CD VARCHAR(MAX)
	,@@PARAM VARCHAR(MAX)

SET @@PLANT_CD = @PLANT_CD
SET @@PARAM = @PARAM

SELECT COUNT(*)
FROM TB_M_SLOC
WHERE 1 = 1
	AND PLANT_CD = @@PLANT_CD
	AND (
		SLOC_CD LIKE '%' + @@PARAM + '%'
		OR SLOC_NAME LIKE '%' + @@PARAM + '%'
		)