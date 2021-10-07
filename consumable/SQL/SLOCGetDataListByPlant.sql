﻿DECLARE @@PLANT_CD VARCHAR(MAX)
	,@@PARAM VARCHAR(MAX)
	,@@StartData INT
	,@@EndData INT

SET @@PLANT_CD = @PLANT_CD
SET @@PARAM = @PARAM
SET @@StartData = @StartData
SET @@EndData = @EndData

SELECT A.*
FROM (
	SELECT ROW_NUMBER() OVER (
			ORDER BY PLANT_CD ASC
				,SLOC_CD ASC
				,WAREHOUSE_CD ASC
			) AS ROWNO
		,PLANT_CD
		,SLOC_CD
		,SLOC_NAME
		,WAREHOUSE_CD
		,CREATED_BY
		,CONVERT(VARCHAR, CREATED_DT, 121) AS CREATED_DT
		,CHANGED_BY
		,CONVERT(VARCHAR, CHANGED_DT, 121) AS CHANGED_DT
	FROM TB_M_SLOC
	WHERE 1 = 1
		AND PLANT_CD = @@PLANT_CD
		AND (
			SLOC_CD LIKE '%' + @@PARAM + '%'
			OR SLOC_NAME LIKE '%' + @@PARAM + '%'
			)
	) A
WHERE A.ROWNO BETWEEN @@StartData
		AND @@EndData