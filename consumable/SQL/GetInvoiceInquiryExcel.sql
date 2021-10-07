DECLARE @@sqlstate varchar(max);
DECLARE @@INVOICE_NO varchar(100);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@HARDCOPY_STATUS  varchar(MAX);
DECLARE @@CREATED_DATE_FROM  varchar(50);
DECLARE @@CREATED_DATE_TO  varchar(50);
DECLARE @@INVOICE_DATE_FROM  varchar(50);
DECLARE @@INVOICE_DATE_TO  varchar(50);
DECLARE @@PLAN_PAYMENT_DATE_FROM  varchar(50);
DECLARE @@PLAN_PAYMENT_DATE_TO  varchar(50);
DECLARE @@SUBMISSION_DATE_FROM  varchar(50);
DECLARE @@SUBMISSION_DATE_TO  varchar(50);

SET @@sqlstate = '';
SET @@INVOICE_NO = @INVOICE_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@HARDCOPY_STATUS = @HARDCOPY_STATUS;
SET @@CREATED_DATE_FROM = @CREATED_DATE_FROM;
SET @@CREATED_DATE_TO = @CREATED_DATE_TO;
SET @@INVOICE_DATE_FROM = @INVOICE_DATE_FROM;
SET @@INVOICE_DATE_TO = @INVOICE_DATE_TO;
SET @@PLAN_PAYMENT_DATE_FROM = @PLAN_PAYMENT_DATE_FROM;
SET @@PLAN_PAYMENT_DATE_TO = @PLAN_PAYMENT_DATE_TO;
SET @@SUBMISSION_DATE_FROM = @SUBMISSION_DATE_FROM;
SET @@SUBMISSION_DATE_TO = @SUBMISSION_DATE_TO;


SET @@sqlstate = @@sqlstate + '
	SELECT 
		INVOICE_ID
		,INVOICE_NO
		,INVOICE_DATE
		,TAX_INVOICE_NO
		,CURRENCY
		,TURN_OVER
		,TAX_BASE
		,CALCULATE_TAX
		,CHECKBOX_STAMP
		,TOTAL_AMOUNT
		,inv.STATUS
		,inv.HARDCOPY_STATUS
		,inv.NOTICE_FLAG
		,PAYMENT_PLAN_DATE
		,NOTICE_BY
		,NOTICE_REMARK
		,NOTICE_DATE
		,CERTIFICATE_ID
		,SUBMIT_DT
		,SUBMIT_BY
		,RECEIVED_STATUS
		,RECEIVED_DT
		,TAX_INVOICE_AMOUNT
		,SAP_DOC_NO
		,POSTED_BY
		,POSTED_DT
		,SAP_YEAR
		,REVERSE_REASON
		,REVERSE_POST_DT
		,SUPPLIER_NAME
		,inv.SUPPLIER_CD
		,ptNew.INV_PAYMENT_ACTUAL_DATE as PAYMENT_ACTUAL_DATE
		,ptNew.CLEARING_NO as PAYMENT_DOC_NO
		,inv.CREATED_DT
		,inv.CREATED_BY
		,s.AUTO_POSTING_FLAG
		,CONVERT(VARCHAR(30), isiNew.BASE_DATE, 104) as BASE_DATE
	FROM TB_R_INVOICE inv
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = inv.SUPPLIER_CD

		OUTER APPLY (
			select top 1 BASE_DATE
			from TB_R_INVOICE_SAP_INPUT isi
			where inv.INVOICE_ID =  isi.INVOICE_ID 
			ORDER BY BASE_DATE DESC ) isiNew

		OUTER APPLY (
			select top 1 INV_PAYMENT_ACTUAL_DATE, CLEARING_NO ,AP_DOC_NO
			from TB_R_PROCUREMENT_TRACKING pt
			where inv.SAP_DOC_NO =  pt.AP_DOC_NO ) ptNew

	where 1=1
		';


IF(@@INVOICE_NO <> '' AND @@INVOICE_NO is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and INVOICE_NO like ''%'+ @@INVOICE_NO + '%'' ';
END

IF(@@SUPPLIER <> '' AND @@SUPPLIER is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and inv.SUPPLIER_CD in ('+@@SUPPLIER+') ';
END

IF(@@STATUS <> '' AND @@STATUS is not null)
BEGIN
	IF (@@STATUS = '''ON_PROCESS_SAP_POST''')
	BEGIN
		SET @@sqlstate = @@sqlstate + '
		and (inv.ON_PROCESS_SAP_POST = ''Y'' or inv.ON_PROCESS_SAP_REV = ''Y'')';
	END
    ELSE IF (@@STATUS = '''ERROR_POSTING_A''')
	BEGIN
	   SET @@sqlstate = @@sqlstate + '
		   and inv.status = ''ERROR_POSTING''  and s.AUTO_POSTING_FLAG = ''Y''';		
	END
    ELSE IF (@@STATUS = '''ERROR_POSTING_M''')
	BEGIN
		SET @@sqlstate = @@sqlstate + '
		   and inv.status = ''ERROR_POSTING''  and (s.AUTO_POSTING_FLAG IS NULL OR s.AUTO_POSTING_FLAG = ''N'')';	
	END
	-- update 19-12-2020 [START]
	ELSE IF (@@STATUS = '''NOTICE''')
	BEGIN
		SET @@sqlstate = @@sqlstate + '
		   and inv.status = ''CREATED''  and (inv.NOTICE_FLAG = ''Y'')';	
	END
	ELSE IF (@@STATUS = '''CREATED''')
	BEGIN
		SET @@sqlstate = @@sqlstate + '
		   and inv.status = ''CREATED''  and (inv.NOTICE_FLAG IS NULL OR inv.NOTICE_FLAG = ''N'')';	
	END
	-- update 19-12-2020 [END]
    ELSE
	BEGIN
		SET @@sqlstate = @@sqlstate + '
		and inv.status in ('+@@STATUS+') ';
    END
END
IF(@@HARDCOPY_STATUS <> '' AND @@HARDCOPY_STATUS is not null)
BEGIN
		SET @@sqlstate = @@sqlstate + '
	 	and inv.hardcopy_status = '''+@@HARDCOPY_STATUS+'''';		
END
IF(@@CREATED_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.CREATED_DT as DATE) >= CAST(CONVERT(datetime, ''' + @@CREATED_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@CREATED_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.CREATED_DT as DATE) <= CAST(CONVERT(datetime, ''' + @@CREATED_DATE_TO + ''' , 104) as DATE)';
END


IF(@@INVOICE_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.INVOICE_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@INVOICE_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@INVOICE_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.INVOICE_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@INVOICE_DATE_TO + ''' , 104) as DATE)';
END


IF(@@PLAN_PAYMENT_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(isiNew.BASE_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@PLAN_PAYMENT_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@PLAN_PAYMENT_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(isiNew.BASE_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@PLAN_PAYMENT_DATE_TO + ''' , 104) as DATE)';
END

IF(@@SUBMISSION_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.SUBMIT_DT as DATE) >= CAST(CONVERT(datetime, ''' + @@SUBMISSION_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@SUBMISSION_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.SUBMIT_DT as DATE) <= CAST(CONVERT(datetime, ''' + @@SUBMISSION_DATE_TO + ''' , 104) as DATE)';
END


SET @@sqlstate = @@sqlstate + '
	ORDER BY INVOICE_DATE desc, inv.SUPPLIER_CD, inv.INVOICE_NO ' ;


print(@@sqlstate);

execute(@@sqlstate);
