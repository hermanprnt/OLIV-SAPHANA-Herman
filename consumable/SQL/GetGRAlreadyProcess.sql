SELECT *
  FROM TB_R_GR_IR_FROM_SAP
  where 1=1 
  and (GR_STATUS <> 'CANCEL' AND  GR_STATUS <> '' )
  and PO_NUMBER = @PO_NUMBER
  and PO_ITEM = @PO_ITEM
  and VEND_CODE = @VEND_CODE
  and MATDOC_NUMBER <> @MATDOC_NUMBER