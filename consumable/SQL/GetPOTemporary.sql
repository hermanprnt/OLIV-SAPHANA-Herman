
exec dbo.SP_GET_PO_TEMPORARY
	 @@PO_DATE_FROM = @PO_DATE_FROM,
	 @@PO_DATE_TO = @PO_DATE_TO,
	 @@PO_NO = @PO_NO,
	 @@SUPPLIER = @SUPPLIER_CD,
	 @@SUPPLIER_STS = @SUPPLIER_STS,
	 @@STATUS = @PROCESS_STS,
	 @@MATDOC_NO = @MATDOC_NO,
	 @@NumberFrom = @NumberFrom,
	 @@NumberTo = @NumberTo