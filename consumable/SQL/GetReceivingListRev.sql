/*DECLARE @@sqlstate varchar(max);
DECLARE @@MATDOC_NO  varchar(50);
DECLARE @@PO_NO  varchar(50);
DECLARE @@DN_NO  varchar(50);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@RECEIVING_DATE_FROM  varchar(50);
DECLARE @@RECEIVING_DATE_TO  varchar(50);
DECLARE @@PO_DATE_FROM  varchar(50);
DECLARE @@PO_DATE_TO  varchar(50);
DECLARE @@PO_TEXT varchar(MAX);
DECLARE @@UNIT_CODE  varchar(50);
DECLARE @@NumberFrom  varchar(MAX);
DECLARE @@NumberTo  varchar(MAX);

SET @@sqlstate = '';
SET @@MATDOC_NO = @MATDOC_NO;
SET @@PO_NO = @PO_NO;
SET @@DN_NO = @DN_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@RECEIVING_DATE_FROM = @RECEIVING_DATE_FROM;
SET @@RECEIVING_DATE_TO = @RECEIVING_DATE_TO;
SET @@PO_DATE_FROM = @PO_DATE_FROM;
SET @@PO_DATE_TO = @PO_DATE_TO;
SET @@PO_TEXT = @PO_TEXT;
SET @@UNIT_CODE = @UNIT_CODE;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;


SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	select 
		ROW_NUMBER () OVER (ORDER BY PO_NUMBER, PO_ITEM, MATDOC_DATE, MATDOC_NUMBER desc) AS Number
		,gr.GR_IR_ID
		,gr.PO_NUMBER
		,gr.DN_NO
		,gr.DN_NO_ITEM
		,gr.PO_ITEM
		,gr.MATDOC_YEAR
		,gr.MATDOC_NUMBER
		,gr.MATDOC_ITEM
		,gr.MATDOC_DATE
		,gr.SPC_STOCK
		,gr.MATDOC_AMOUNT
		,gr.VEND_CODE
		,gr.PURCHDOC_PRICE
		,gr.MAT_NUMBER
		,gr.MAT_DESCR
		,gr.PLANT_CODE
		,gr.SLOC_CODE
		,gr.MATDOC_UNIT
		,gr.CANCEL
		,gr.ORI_MAT_NUMBER
		,gr.MATDOC_CURRENCY
		,gr.MATDOC_QTY
		,gr.TAX_CODE
		,gr.PO_DATE
		,gr.GR_STATUS
		,gr.HEADER_TEXT
		,gr.IR_NO
		,gr.MOV_TYPE
		,gr.REF_DOC
		,gr.USRID
		,gr.CANCEL_DOC_NO
		,s.SUPPLIER_NAME
	from 
		TB_R_GR_IR_FROM_SAP gr
	left join TB_M_SUPPLIER s on s.SUPPLIER_CD = gr.VEND_CODE
	where 1=1 
		and MATDOC_QTY > 0
	 ';

IF(@@MATDOC_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.MATDOC_NUMBER like ''%'+ @@MATDOC_NO + '%'' ';
END

IF(@@PO_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.PO_NUMBER like ''%'+ @@PO_NO + '%'' ';
END

IF(@@SUPPLIER <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.VEND_CODE in ('+@@SUPPLIER+') ';
END

IF(@@STATUS <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.GR_STATUS in ('+@@STATUS+') ';
END

IF(@@RECEIVING_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.MATDOC_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@RECEIVING_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@RECEIVING_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.MATDOC_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@RECEIVING_DATE_TO + ''' , 104) as DATE)';
END

IF(@@PO_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.PO_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@PO_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@PO_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(gr.PO_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@PO_DATE_TO + ''' , 104) as DATE)';
END

IF(@@UNIT_CODE <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and VEND_CODE in (SELECT
      SUPPLIER_CD
  FROM TB_M_MAINTENANCE_APPROVAL
  where APPROVER_UNIT_CD = ''' + @@UNIT_CODE + ''') ';
END

SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;


print(@@sqlstate);

execute(@@sqlstate);*/

DECLARE @@MATDOC_NO VARCHAR(50)
	,@@PO_NO VARCHAR(50)
	,@@DN_NO  varchar(50)
	,@@SUPPLIER VARCHAR(MAX)
	,@@STATUS VARCHAR(MAX)
	,@@RECEIVING_DATE_FROM VARCHAR(50)
	,@@RECEIVING_DATE_TO VARCHAR(50)
	,@@PO_DATE_FROM VARCHAR(50)
	,@@PO_DATE_TO VARCHAR(50)
	,@@PO_TEXT VARCHAR(MAX)
	,@@UNIT_CODE VARCHAR(50)
	,@@NOREG VARCHAR(50)
	,@@NumberFrom VARCHAR(max)
	,@@NumberTo VARCHAR(max)
	,@@sqlstate varchar(max)

SET @@MATDOC_NO = @MATDOC_NO;
SET @@PO_NO = @PO_NO;
SET @@DN_NO = @DN_NO;
SET @@SUPPLIER = ISNULL(@SUPPLIER, '');
SET @@STATUS = @STATUS
SET @@RECEIVING_DATE_FROM = @RECEIVING_DATE_FROM
SET @@RECEIVING_DATE_TO = @RECEIVING_DATE_TO;
SET @@PO_DATE_FROM = @PO_DATE_FROM;
SET @@PO_DATE_TO = @PO_DATE_TO;
SET @@PO_TEXT = @PO_TEXT;
SET @@UNIT_CODE = @UNIT_CODE;
SET @@NOREG = @NOREG;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

DECLARE @@countunitapprover INT
SELECT @@countunitapprover = COUNT(1) FROM TB_M_UNIT_APPROVER
WHERE PIC = @@NOREG
AND DELETION_FLAG = 'N'

SET @@sqlstate = '
SELECT *
FROM (
	SELECT ROW_NUMBER() OVER (
			ORDER BY GR.PO_NUMBER
				,GR.PO_ITEM
				,GR.MATDOC_DATE
				,GR.MATDOC_NUMBER DESC
			) AS NUMBER
		,*
	FROM (
		SELECT GR.GR_IR_ID
			,GR.PO_NUMBER
			,GR.DN_NO
			,GR.DN_NO_ITEM
			,GR.PO_ITEM
			,GR.MATDOC_YEAR
			,GR.MATDOC_NUMBER
			,GR.MATDOC_ITEM
			,GR.MATDOC_DATE
			,GR.SPC_STOCK
			,GR.MATDOC_AMOUNT
			,GR.VEND_CODE
			,GR.PURCHDOC_PRICE
			,GR.MAT_NUMBER
			,GR.MAT_DESCR
			,GR.PLANT_CODE
			,GR.SLOC_CODE
			,GR.MATDOC_UNIT
			,GR.CANCEL
			,GR.ORI_MAT_NUMBER
			,GR.MATDOC_CURRENCY
			,GR.MATDOC_QTY
			,GR.TAX_CODE
			,GR.PO_DATE
			,GR.GR_STATUS
			,GR.HEADER_TEXT
			,GR.IR_NO
			,GR.MOV_TYPE
			,GR.REF_DOC
			,GR.USRID
			,GR.CANCEL_DOC_NO
			,S.SUPPLIER_NAME
		FROM TB_R_GR_IR_FROM_SAP GR
		LEFT JOIN TB_M_SUPPLIER S ON S.SUPPLIER_CD = GR.VEND_CODE
		WHERE 1 = 1
			AND GR.MATDOC_QTY > 0 '

			IF(@@MATDOC_NO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.MATDOC_NUMBER LIKE ''%'+ @@MATDOC_NO + '%'' '
			END

			IF(@@DN_NO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.DN_NO LIKE ''%'+ @@DN_NO + '%'' ';
			END

			IF(@@PO_NO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.PO_NUMBER LIKE ''%'+ @@PO_NO + '%'' '
			END

			IF(@@SUPPLIER <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.VEND_CODE IN ('+@@SUPPLIER+') '
			END

			IF(@@STATUS <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.GR_STATUS IN (' + @@STATUS + ') '
			END

			IF(@@RECEIVING_DATE_FROM <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.MATDOC_DATE AS DATE) >= CAST(CONVERT(DATETIME, ''' + @@RECEIVING_DATE_FROM + ''' , 104) AS DATE)'
			END

			IF(@@RECEIVING_DATE_TO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.MATDOC_DATE AS DATE) <= CAST(CONVERT(DATETIME, ''' + @@RECEIVING_DATE_TO + ''' , 104) AS DATE)'
			END

			IF(@@PO_DATE_FROM <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.PO_DATE AS DATE) >= CAST(CONVERT(DATETIME, ''' + @@PO_DATE_FROM + ''' , 104) AS DATE)'
			END

			IF(@@PO_DATE_TO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.PO_DATE AS DATE) <= CAST(CONVERT(DATETIME, ''' + @@PO_DATE_TO + ''' , 104) AS DATE)'
			END

			IF(@@UNIT_CODE <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND VEND_CODE IN (
					SELECT SUPPLIER_CD
					FROM TB_M_MAINTENANCE_APPROVAL
					WHERE APPROVER_UNIT_CD = ''' + @@UNIT_CODE + ''')
				
				AND LEFT(GR.PO_NUMBER, 2) = (
					SELECT SYSTEM_VALUE_TEXT
					FROM TB_M_SYSTEM
					WHERE SYSTEM_TYPE = ''PO_PATTERN''
						AND SYSTEM_CD = ''RECEIVING_LIST_APPROVAL''
					) '
			END

SET @@sqlstate = @@sqlstate + '		
		UNION ALL
		
		SELECT DISTINCT GR.GR_IR_ID
			,GR.PO_NUMBER
			,GR.DN_NO
			,GR.DN_NO_ITEM
			,GR.PO_ITEM
			,GR.MATDOC_YEAR
			,GR.MATDOC_NUMBER
			,GR.MATDOC_ITEM
			,GR.MATDOC_DATE
			,GR.SPC_STOCK
			,GR.MATDOC_AMOUNT
			,GR.VEND_CODE
			,GR.PURCHDOC_PRICE
			,GR.MAT_NUMBER
			,GR.MAT_DESCR
			,GR.PLANT_CODE
			,GR.SLOC_CODE
			,GR.MATDOC_UNIT
			,GR.CANCEL
			,GR.ORI_MAT_NUMBER
			,GR.MATDOC_CURRENCY
			,GR.MATDOC_QTY
			,GR.TAX_CODE
			,GR.PO_DATE
			,GR.GR_STATUS
			,GR.HEADER_TEXT
			,GR.IR_NO
			,GR.MOV_TYPE
			,GR.REF_DOC
			,GR.USRID
			,GR.CANCEL_DOC_NO
			,S.SUPPLIER_NAME
		FROM TB_R_GR_IR_FROM_SAP GR
		JOIN TB_M_UNIT_APPROVER UA ON UA.PLANT_CD = GR.PLANT_CODE
			AND UA.SLOC_CD = CASE WHEN ISNULL(GR.SLOC_CODE, '''') = '''' THEN UA.SLOC_CD ELSE GR.SLOC_CODE END
			AND UA.DELETION_FLAG = ''N''
		LEFT JOIN TB_M_SUPPLIER S ON S.SUPPLIER_CD = GR.VEND_CODE
		WHERE 1 = 1
			AND GR.MATDOC_QTY > 0
			AND LEFT(GR.PO_NUMBER, 2) <> (
				SELECT SYSTEM_VALUE_TEXT
				FROM TB_M_SYSTEM
				WHERE SYSTEM_TYPE = ''PO_PATTERN''
					AND SYSTEM_CD = ''RECEIVING_LIST_APPROVAL''
				)
			AND UA.PIC = ''' + @@NOREG + ''' '

			IF(@@MATDOC_NO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.MATDOC_NUMBER LIKE ''%'+ @@MATDOC_NO + '%'' '
			END

			IF(@@DN_NO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.DN_NO LIKE ''%'+ @@DN_NO + '%'' ';
			END

			IF(@@PO_NO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.PO_NUMBER LIKE ''%'+ @@PO_NO + '%'' '
			END

			IF(@@SUPPLIER <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.VEND_CODE IN ('+@@SUPPLIER+') '
			END

			IF(@@STATUS <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND GR.GR_STATUS IN (' + @@STATUS + ') '
			END

			IF(@@RECEIVING_DATE_FROM <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.MATDOC_DATE AS DATE) >= CAST(CONVERT(DATETIME, ''' + @@RECEIVING_DATE_FROM + ''' , 104) AS DATE)'
			END

			IF(@@RECEIVING_DATE_TO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.MATDOC_DATE AS DATE) <= CAST(CONVERT(DATETIME, ''' + @@RECEIVING_DATE_TO + ''' , 104) AS DATE)'
			END

			IF(@@PO_DATE_FROM <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.PO_DATE AS DATE) >= CAST(CONVERT(DATETIME, ''' + @@PO_DATE_FROM + ''' , 104) AS DATE)'
			END

			IF(@@PO_DATE_TO <> '')
			BEGIN
				SET @@sqlstate = @@sqlstate + '
				AND CAST(GR.PO_DATE AS DATE) <= CAST(CONVERT(DATETIME, ''' + @@PO_DATE_TO + ''' , 104) AS DATE)'
			END

SET @@sqlstate = @@sqlstate + '	
		) GR 
		WHERE 1 = 1 '

IF ISNULL (@@NOREG,'') != '' AND ISNULL(@@UNIT_CODE,'') =''
BEGIN
IF @@countunitapprover > 0
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND LEFT(GR.PO_NUMBER, 2) <> (
				SELECT SYSTEM_VALUE_TEXT
				FROM TB_M_SYSTEM
				WHERE SYSTEM_TYPE = ''PO_PATTERN''
					AND SYSTEM_CD = ''RECEIVING_LIST_APPROVAL''
				)'
END
ELSE
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND LEFT(GR.PO_NUMBER, 2) = (
				SELECT SYSTEM_VALUE_TEXT
				FROM TB_M_SYSTEM
				WHERE SYSTEM_TYPE = ''PO_PATTERN''
					AND SYSTEM_CD = ''RECEIVING_LIST_APPROVAL''
				)'
END
END

SET @@sqlstate = @@sqlstate + '	
	) REFF
WHERE NUMBER BETWEEN ' + @@NumberFrom + '
		AND ' + @@NumberTo

execute(@@sqlstate)