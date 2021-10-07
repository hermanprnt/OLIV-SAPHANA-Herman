DECLARE @@sqlstate varchar(max);
DECLARE @@NumberFrom  varchar(4);
DECLARE @@NumberTo  varchar(4);

SET @@sqlstate = '';
SET @@NumberFrom = @NumberFrom;
SET @@NumberTo = @NumberTo;

SET @@sqlstate = @@sqlstate + '
	SELECT * FROM 
	(
		SELECT 
			ROW_NUMBER () OVER (ORDER BY TAX_INVOICE_NO) AS Number
		  ,TAX_INVOICE_NO
		  ,REPLACEMENT_FG
		  ,SAP_TAX_INVOICE_NO
		  ,TRANS_TYPE_CODE
		  ,SUPPLIER_CODE
		  ,DPP_PRICE
		  ,VAT_PRICE
		  ,LUXURY_TAX_PRICE
		  ,STATUS
		  ,INV_STATUS
		  ,TAX_INVOICE_MONTH
		  ,TAX_INVOICE_YEAR
		  ,TAX_INVOICE_DT
		  ,IS_CREDITABLE
		  ,SAP_DOC_NO
		  ,SAP_POST_DT
		  ,INVOICE_NO
		  ,SCAN_DT
		  ,INTERFACE_DT
		  ,DOWNLOAD_DT
		  ,SCAN_BY
		  ,QR_CODE_PATH
		  ,h.CREATED_DT
		  ,h.CHANGED_DT
		  ,h.CREATED_BY
		  ,h.CHANGED_BY
		  ,VENDOR_CD
		  ,USED
		  ,BYPASS_FLAG
		  ,IS_USED 
		  ,s.SUPPLIER_NAME
		  ,s.SUPPLIER_CD
		FROM 
			TB_R_VAT_IN_H h
			left join TB_M_SUPPLIER s on s.SUPPLIER_CD = h.SUPPLIER_CODE
		WHERE 1=1 '

IF(@TAX_INVOICE_NO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
		AND TAX_INVOICE_NO like ''%'+ @TAX_INVOICE_NO + '%'' ';
END

IF(@TAX_INVOICE_DT_FROM <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
		AND CAST(TAX_INVOICE_DT as DATE) >= CAST(CONVERT(datetime, ''' + @TAX_INVOICE_DT_FROM + ''' , 104) as DATE) ';
END

IF(@TAX_INVOICE_DT_TO <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
		AND CAST(TAX_INVOICE_DT as DATE) <= CAST(CONVERT(datetime, ''' + @TAX_INVOICE_DT_TO + ''' , 104) as DATE) ';
END

IF(@VEND_CODE <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	AND SUPPLIER_CODE = '''+ @VEND_CODE + ''' ';
END

SET @@sqlstate = @@sqlstate + '
	) TB WHERE Number BETWEEN ' + @@NumberFrom + ' AND ' + @@NumberTo;

execute(@@sqlstate);	