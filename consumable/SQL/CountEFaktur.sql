DECLARE @@val INT = 0

SELECT @@val = SYSTEM_VALUE_NUM FROM PROCUREMENT.dbo.TB_M_SYSTEM WHERE SYSTEM_CD = 'EFAKTUR_EXPIRED' and SYSTEM_TYPE = 'CREATE_INV'


SELECT COUNT(1) FROM 
(
	SELECT 
		ROW_NUMBER () OVER (ORDER BY h.TAX_INVOICE_NO) AS Number, TRANS_TYPE_CODE + '' + REPLACEMENT_FG + '' + TAX_INVOICE_NO TAX_INVOICE_NO
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
				,CREATED_DT
				,CHANGED_DT
				,CREATED_BY
				,CHANGED_BY
				,VENDOR_CD
				,USED
				,MANUAL_ENTRY
				,BYPASS_FLAG 
	FROM 
		TB_R_VAT_IN_H h
	WHERE
		--h.SUPPLIER_CODE = substring(@SUPPLIER_CD, patindex('%[^0]%',@SUPPLIER_CD), 10)
		(REPLICATE('0', 10 - LEN(h.SUPPLIER_CODE))+ h.SUPPLIER_CODE) = @SUPPLIER_CD --20200916
		AND 
		h.TAX_INVOICE_NO like '%' + ISNULL(@Parameter,'') + '%'
		
		AND ISNULL(USED, 'N') != 'Y'
		AND DATEADD(MONTH, @@val, TAX_INVOICE_DT) >= GETDATE()

	-- FID.Ridwan:20211102 --> new logic
	UNION ALL

	SELECT 
		ROW_NUMBER () OVER (ORDER BY h.TAX_INVOICE_NO) AS Number, TRANS_TYPE_CODE + '' + REPLACEMENT_FG + '' + TAX_INVOICE_NO TAX_INVOICE_NO
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
				,MANUAL_ENTRY
				,BYPASS_FLAG 
	FROM 
		TB_R_VAT_IN_H h
	JOIN [PROCUREMENT_DB].[PROCUREMENT].[dbo].[TB_M_SUPPLIER_MAPPING] b ON H.SUPPLIER_CODE = B.SUPP_CD_SAP
	WHERE B.SUPP_CD_LEGACY = @SUPPLIER_CD --20200916
		AND  h.TAX_INVOICE_NO like '%' + ISNULL(@Parameter,'') + '%'
		AND ISNULL(USED, 'N') != 'Y'
		AND DATEADD(MONTH, @@val, TAX_INVOICE_DT) >= GETDATE()
) TB

