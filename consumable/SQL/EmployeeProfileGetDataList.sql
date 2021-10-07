DECLARE @@PARAM VARCHAR(MAX)
	,@@StartData INT
	,@@EndData INT

SET @@PARAM = @PARAM
SET @@StartData = @StartData
SET @@EndData = @EndData

SELECT A.*
FROM (
	SELECT ROW_NUMBER() OVER (
			ORDER BY NOREG ASC
				,[NAME] ASC
			) AS ROWNO
		,NOREG
		,[NAME]
	FROM VW_EMPLOYEE_PROFILE
	WHERE 1 = 1
		AND (
			NOREG LIKE '%' + @@PARAM + '%'
			OR [NAME] LIKE '%' + @@PARAM + '%'
			)
	) A
WHERE A.ROWNO BETWEEN @@StartData
		AND @@EndData