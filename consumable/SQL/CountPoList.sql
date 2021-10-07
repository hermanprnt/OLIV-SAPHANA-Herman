DECLARE @@sqlstate nvarchar(max);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@CREATED_DATE_FROM  varchar(50);
DECLARE @@CREATED_DATE_TO  varchar(50);
DECLARE @@RELEASED_DATE_FROM  varchar(50);
DECLARE @@RELEASED_DATE_TO  varchar(50);
DECLARE @@PO_NO  varchar(50);
DECLARE @@PO_DESCRIPTION  varchar(50);
DECLARE @@PO_AMOUNT_FROM varchar(100);
DECLARE @@PO_AMOUNT_TO varchar(100);

SET @@sqlstate = '';
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@CREATED_DATE_FROM = @CREATED_DATE_FROM;
SET @@CREATED_DATE_TO = @CREATED_DATE_TO;
SET @@RELEASED_DATE_FROM = @RELEASED_DATE_FROM;
SET @@RELEASED_DATE_TO = @RELEASED_DATE_TO;
SET @@PO_NO = @PO_NO;
SET @@PO_DESCRIPTION = @PO_DESCRIPTION;
SET @@PO_AMOUNT_FROM = @PO_AMOUNT_FROM;
SET @@PO_AMOUNT_TO = @PO_AMOUNT_TO;

DECLARE @@PO_NO_TEMP varchar(max), @@PO_DESCS VARCHAR(MAX);
 
DECLARE @@t PO_ITEM_TEMP;
 
INSERT @@t(PO_NUMBER, PO_DESC)
  SELECT PO_NUMBER, MAT_DESCR FROM TB_R_PO_ITEM
  ORDER BY PO_NUMBER, MAT_DESCR;
 
 
UPDATE @@t SET @@PO_DESCS = PO_DESCS = COALESCE(
    CASE COALESCE(@@PO_NO_TEMP, N'') 
      WHEN PO_NUMBER THEN @@PO_DESCS + N'; ' + PO_DESC
      ELSE PO_DESC END, N''), 
	@@PO_NO_TEMP = PO_NUMBER;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM TB_R_PO_HEADER poh
	inner join (SELECT PO_NUMBER, PO_DESCS = MAX(PO_DESCS)
		  FROM @@t 
		  GROUP BY PO_NUMBER
		  ) temp on temp.PO_NUMBER = poh.PO_NUMBER
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = poh.VEND_CODE
	where 1=1
	  ';

IF(@@PO_NO <> '' AND @@PO_NO is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and poh.PO_NUMBER like ''%'+ @@PO_NO + '%'' ';
END

IF(@@SUPPLIER <> '' AND @@SUPPLIER is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and poh.VEND_CODE in ('+@@SUPPLIER+') ';
END

IF((@@STATUS <> '' AND @@STATUS is not null) AND @@STATUS <> 'All')
BEGIN
	IF(@@STATUS = 'Not Yet')
	BEGIN
		SET @@sqlstate = @@sqlstate + '
			and APPROVAL_STATUS is NULL ';
	END
	ELSE
	BEGIN
		--SET @@sqlstate = @@sqlstate + '
		--	and APPROVAL_STATUS in ('+@@STATUS+') ';
		SET @@sqlstate = @@sqlstate + '
			and APPROVAL_STATUS = '''+@@STATUS+''' '; 
	END
END
--ELSE
--BEGIN
--	SET @@sqlstate = @@sqlstate + '
--	and APPROVAL_STATUS is NULL ';
--END



IF(@@CREATED_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(poh.CREATED_DT as DATE) >= CAST(CONVERT(datetime, ''' + @@CREATED_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@CREATED_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(poh.CREATED_DT as DATE) <= CAST(CONVERT(datetime, ''' + @@CREATED_DATE_TO + ''' , 104) as DATE)';
END


IF(@@RELEASED_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(APP_SH_DT as DATE) <= CAST(CONVERT(datetime, ''' + @@RELEASED_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@RELEASED_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(APP_SH_DT as DATE) >= CAST(CONVERT(datetime, ''' + @@RELEASED_DATE_TO + ''' , 104) as DATE)';
END

IF(@@PO_AMOUNT_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and PO_AMOUNT >= ''' + @@PO_AMOUNT_FROM + '';
END

IF(@@PO_AMOUNT_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and PO_AMOUNT <= ''' + @@PO_AMOUNT_TO + '';
END

IF(@@PO_DESCRIPTION <> '' AND @@PO_DESCRIPTION is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and temp.PO_DESCS like ''%'+ @@PO_DESCRIPTION + '%'' ';
END


--print(@@sqlstate);

--execute(@@sqlstate);

exec sp_executesql @@sqlstate, N'@@t PO_ITEM_TEMP readonly', @@t