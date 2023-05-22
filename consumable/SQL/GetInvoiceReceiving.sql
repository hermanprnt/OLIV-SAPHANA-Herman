
DECLARE @@sqlstate varchar(max);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

DECLARE @@LinkedServer nvarchar(4000);
SET @@LinkedServer = '[' + @LinkedServer + ']';

SET @@sqlstate = '';
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT NTABLE.*, DUEDILLIGENCE.DD_STATUS FROM (
	SELECT * FROM 
	( ';

SET @@sqlstate = @@sqlstate + '
	SELECT 
		ROW_NUMBER () OVER (ORDER BY RECEIVED_DT desc) AS Number
		,INVOICE_ID
		,INVOICE_NO
		,INVOICE_DATE
		,TAX_INVOICE_NO
		,CURRENCY
		,TURN_OVER
		,TAX_BASE
		,CALCULATE_TAX
		,CHECKBOX_STAMP
		,TOTAL_AMOUNT
		,STATUS
		,inv.SUPPLIER_CD
		,PAYMENT_PLAN_DATE
		,PAYMENT_ACTUAL_DATE
		,NOTICE_BY
		,NOTICE_REMARK
		,NOTICE_DATE
		,CERTIFICATE_ID
		,SUBMIT_DT
		,SUBMIT_BY
		,RECEIVED_STATUS
		,RECEIVED_DT
		,TAX_INVOICE_AMOUNT
		,SUPPLIER_NAME
		,VENDOR_SAP_ID
	FROM TB_R_INVOICE inv
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = inv.SUPPLIER_CD
	where 1=1
	--and cast(RECEIVED_DT as DATE) = cast(getDate() as DATE)
		';


SET @@sqlstate = @@sqlstate + '
	) REFF WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo + ' ) NTABLE

	LEFT JOIN (
		SELECT DD_STATUS,SAP_VENDOR_ID FROM OPENQUERY('+ @@LinkedServer + ', 
		''
			SELECT  DD_STATUS AS DD_STATUS,SAP_VENDOR_ID FROM PAS_DB_QA.DBO.TB_M_DUE_DILLIGENCE			
			'' )
	) DUEDILLIGENCE
	ON NTABLE.VENDOR_SAP_ID = DUEDILLIGENCE.SAP_VENDOR_ID
	ORDER BY Number ASC
	
	'




print(@@sqlstate);

execute(@@sqlstate);
