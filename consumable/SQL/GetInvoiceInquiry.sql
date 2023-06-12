
DECLARE @@sqlstate varchar(max);
DECLARE @@INVOICE_NO varchar(100);
DECLARE @@SUPPLIER  varchar(MAX);
DECLARE @@STATUS  varchar(MAX);
DECLARE @@HARDCOPY_STATUS  varchar(MAX);
DECLARE @@CREATED_DATE_FROM  varchar(50);
DECLARE @@CREATED_DATE_TO  varchar(50);
DECLARE @@SUBMISSION_DATE_FROM  varchar(50);
DECLARE @@SUBMISSION_DATE_TO  varchar(50);
DECLARE @@INVOICE_DATE_FROM  varchar(50);
DECLARE @@INVOICE_DATE_TO  varchar(50);
DECLARE @@PLAN_PAYMENT_DATE_FROM  varchar(50);
DECLARE @@PLAN_PAYMENT_DATE_TO  varchar(50);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

--update
DECLARE @@LinkedServer nvarchar(4000);
DECLARE @@LinkedServerPAS nvarchar(4000);

SET @@LinkedServer = '[' + @LinkedServer + ']';
SET @@LinkedServerPAS = '[' + (SELECT SYSTEM_VALUE_TEXT FROM TB_M_SYSTEM WHERE SYSTEM_CD = 'IP_ADDRESS' AND SYSTEM_TYPE = 'INVOICE_RECEIVING' ) + ']';
--SET @@LinkedServer = '[52.77.208.248]';
--SET @@LinkedServer = '[10.16.25.2]';

SET @@sqlstate = '';
SET @@INVOICE_NO = @INVOICE_NO;
SET @@SUPPLIER = @SUPPLIER;
SET @@STATUS = @STATUS;
SET @@HARDCOPY_STATUS = @HARDCOPY_STATUS;
SET @@CREATED_DATE_FROM = @CREATED_DATE_FROM;
SET @@CREATED_DATE_TO = @CREATED_DATE_TO;
SET @@SUBMISSION_DATE_FROM = @SUBMISSION_DATE_FROM;
SET @@SUBMISSION_DATE_TO = @SUBMISSION_DATE_TO;
SET @@INVOICE_DATE_FROM = @INVOICE_DATE_FROM;
SET @@INVOICE_DATE_TO = @INVOICE_DATE_TO;
SET @@PLAN_PAYMENT_DATE_FROM = @PLAN_PAYMENT_DATE_FROM;
SET @@PLAN_PAYMENT_DATE_TO = @PLAN_PAYMENT_DATE_TO;
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;


SET @@sqlstate = @@sqlstate + '
	SELECT DISTINCT NTABLE.*, EFAKTUR.TAX_INVOICE_DT 
	,PAS.SAP_VENDOR_ID 
	,DD_STATUS
	,DUE_DILLIGENCE
	,AGREEMENT_NO	
	FROM (
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	SELECT DISTINCT
		ROW_NUMBER () OVER (ORDER BY INVOICE_DATE desc, inv.SUPPLIER_CD, inv.INVOICE_NO) AS Number
		,INVOICE_ID
		,inv.INVOICE_NO
		,INVOICE_DATE
		,inv.TAX_INVOICE_NO
		,CURRENCY
		,TURN_OVER
		,TAX_BASE
		,CALCULATE_TAX
		,CHECKBOX_STAMP
		,TOTAL_AMOUNT
		,inv.STATUS
		,inv.HARDCOPY_STATUS
		,inv.NOTICE_FLAG
		,NOTICE_BY
		,NOTICE_REMARK
		,NOTICE_DATE
		,CERTIFICATE_ID	
		,SUBMIT_DT
		,SUBMIT_BY
		,RECEIVED_STATUS
		,RECEIVED_DT
		,TAX_INVOICE_AMOUNT
		,inv.PPV_AMOUNT
		,inv.SAP_DOC_NO
		,inv.LOG_DOC_NO
		,POSTED_BY
		,POSTED_DT
		,SAP_YEAR
		,REVERSE_REASON
		,REVERSE_POST_DT
		,SUPPLIER_NAME
		,inv.SUPPLIER_CD
		,s.PAY_METHOD
		,s.TERM_PAY
		,GR_CANCEL
		,inv.PAYMENT_PLAN_DATE
		,inv.PAYMENT_ACTUAL_DATE
		,inv.LOG_DOC_NO as PAYMENT_DOC_NO
		--,ptNew.INV_PAYMENT_ACTUAL_DATE as PAYMENT_ACTUAL_DATE
		--,ptNew.CLEARING_NO as PAYMENT_DOC_NO
		,inv.CREATED_DT
		,inv.CREATED_BY
		,ON_PROCESS_SAP_POST
        ,ON_PROCESS_SAP_REV
		,s.AUTO_POSTING_FLAG
		,CONVERT(VARCHAR(30), isiNew.BASE_DATE, 104) as BASE_DATE
		
	FROM TB_R_INVOICE inv  WITH (NOLOCK)
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = inv.SUPPLIER_CD
		
		OUTER APPLY (
			select top 1 BASE_DATE
			from TB_R_INVOICE_SAP_INPUT isi  WITH (NOLOCK)
			where inv.INVOICE_ID =  isi.INVOICE_ID 
			ORDER BY BASE_DATE DESC ) isiNew
		
		--remark FID.Ridwan: 20220719
		--OUTER APPLY (
		--	select top 1 INV_PAYMENT_ACTUAL_DATE, CLEARING_NO ,AP_DOC_NO
		--	from TB_R_PROCUREMENT_TRACKING pt WITH (NOLOCK)
		--	where inv.SAP_DOC_NO =  pt.AP_DOC_NO ) ptNew
	where 1=1
		';


IF(@@INVOICE_NO <> '' AND @@INVOICE_NO is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and inv.INVOICE_NO like ''%'+ @@INVOICE_NO + '%'' ';
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
	 	and inv.status = '+@@STATUS+'';		
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

IF(@@INVOICE_DATE_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.INVOICE_DATE as DATE) >= CAST(CONVERT(datetime, ''' + @@INVOICE_DATE_FROM + ''' , 104) as DATE)';
END

IF(@@INVOICE_DATE_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and CAST(inv.INVOICE_DATE as DATE) <= CAST(CONVERT(datetime, ''' + @@INVOICE_DATE_TO + ''' , 104) as DATE)';
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


SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo 
	+' ) NTABLE
	LEFT JOIN (
		SELECT TAX_INVOICE_NO, TAX_INVOICE_DT FROM OPENQUERY('+ @@LinkedServer + ', 
		''SELECT v1.TAX_INVOICE_NO AS TAX_INVOICE_NO, V1.TAX_INVOICE_DT AS TAX_INVOICE_DT from TMMIN_E_FAKTUR_SAPHANA.dbo.tb_r_vat_in_h v1 
			WHERE 
			v1.REPLACEMENT_FG = (SELECT MAX(v2.REPLACEMENT_FG) FROM TMMIN_E_FAKTUR_SAPHANA.dbo.TB_R_VAT_IN_H V2 
			WHERE V2.TAX_INVOICE_NO = v1.TAX_INVOICE_NO)'')
	) EFAKTUR
	ON RIGHT(NTABLE.TAX_INVOICE_NO, 13) = EFAKTUR.TAX_INVOICE_NO
	LEFT JOIN (
		SELECT DD_STATUS
		,DUE_DILLIGENCE
		,AGREEMENT_NO
		,SAP_VENDOR_ID

		FROM OPENQUERY('+ @@LinkedServerPAS + ', 
		''SELECT POH.DOC_DT AS CREATED_DT
				,DD.DD_STATUS AS DD_STATUS 
				,AN.EXP_DATE AS EXP_DATE 	
				,DD.VALID_DD_TO AS VALID_DD_TO 
				,DD.SAP_VENDOR_ID AS SAP_VENDOR_ID
				,CASE 
					WHEN POH.DOC_DT <= DD.VALID_DD_TO THEN ''''YES''''
					ELSE ''''NO''''
				END AS DUE_DILLIGENCE
				,CASE 
					WHEN POH.DOC_DT <= AN.EXP_DATE THEN ''''YES''''
					ELSE ''''NO''''
				END AS AGREEMENT_NO
			FROM PAS_DB_SAPHANA.dbo.TB_M_DUE_DILLIGENCE DD 
			LEFT JOIN PAS_DB_SAPHANA.dbo.TB_R_PO_H POH ON DD.VENDOR_CODE = POH.VENDOR_CD
			LEFT JOIN PAS_DB_SAPHANA.dbo.TB_M_AGREEMENT_NO AN ON AN.VENDOR_CODE = DD.VENDOR_CODE
			JOIN (SELECT MAX(EXP_DATE) As MAXDATE,VENDOR_CODE FROM TB_M_AGREEMENT_NO GROUP BY VENDOR_CODE) G
			ON AN.VENDOR_CODE = G.VENDOR_CODE AND AN.EXP_DATE = G.MAXDATE
		'')
	) PAS
	ON NTABLE.SUPPLIER_CD = PAS.SAP_VENDOR_ID

	ORDER BY Number ASC
	
	'

--print(@@sqlstate);

execute(@@sqlstate);
