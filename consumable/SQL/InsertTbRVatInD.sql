INSERT INTO TB_R_VAT_IN_D
(         
	REPLACEMENT_FG, 
    TAX_INVOICE_NO,
	PRODUCT_NM,
	UNIT_PRICE,
	PRODUCT_QTY,
	TOTAL_PRICE,
	DISC_PRICE,
	DPP_PRICE,
	VAT_PRICE,
	LUXURY_TAX_PERC,
	LUXURY_TAX_PRICE,
    CREATED_BY,
	CREATED_DT 
) 
 VALUES 
(
	@REPLACEMENT_FG, 
	@TAX_INVOICE_NO, 
    @PRODUCT_NM,  
    @UNIT_PRICE,  
	@PRODUCT_QTY,
	@TOTAL_PRICE,
	@DISC_PRICE,
	@DPP_PRICE,
	@VAT_PRICE,
	@LUXURY_TAX_PERC,
	@LUXURY_TAX_PRICE,
	@CREATED_BY,   
    GETDATE()
);

