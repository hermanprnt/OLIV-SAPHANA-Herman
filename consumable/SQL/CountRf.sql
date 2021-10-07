DECLARE @@sqlstate varchar(max);
DECLARE @@RF_NO  varchar(50);
DECLARE @@DIVISI  varchar(50);
DECLARE @@PIC_USER  varchar(50);
DECLARE @@WBS_NO  varchar(50);
DECLARE @@PR_NO  varchar(50);
DECLARE @@RF_DT_FROM  varchar(50);
DECLARE @@RF_DT_TO  varchar(50);

SET @@sqlstate = '';
SET @@RF_NO = @RF_NO;
SET @@DIVISI = @DIVISI;
SET @@PIC_USER = @PIC_USER;
SET @@WBS_NO = @WBS_NO;
SET @@PR_NO = @PR_NO;
SET @@RF_DT_FROM = @RF_DT_FROM;
SET @@RF_DT_TO = @RF_DT_TO;

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM 
		TB_R_RF_REGISTER rf 
		left join TB_R_RF_REGISTER_DTL rfd on rfd.RF_REGISTER_ID = rf.RF_REGISTER_ID
		left join VW_R_PR pr on pr.RF_NO = rf.RF_NO
		left join VW_R_PO po on po.PR_NO = pr.PR_NO
	where 1=1 ';
	
IF(@@RF_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and rf.RF_NO like ''%'+ @@RF_NO + '%'' ';
END

IF(@@DIVISI <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and rf.DIVISION like ''%'+ @@DIVISI + '%'' ';
END

IF(@@PIC_USER <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and rf.PIC_USER like ''%'+ @@PIC_USER + '%'' ';
END

IF(@@WBS_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and rf.WBS_NO like ''%'+ @@WBS_NO + '%'' ';
END

IF(@@PR_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and rf.PR_NO like ''%'+ @@PR_NO + '%'' ';
END

IF(@@RF_DT_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(rf.RF_DT as DATE) >= CAST(CONVERT(datetime, ''' + @@RF_DT_FROM + ''' , 104) as DATE)';
END

IF(@@RF_DT_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(rf.RF_DT as DATE) <= CAST(CONVERT(datetime, ''' + @@RF_DT_TO + ''' , 104) as DATE)';
END


print(@@sqlstate);

execute(@@sqlstate);

