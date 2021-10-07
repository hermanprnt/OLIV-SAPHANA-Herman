UPDATE 
	TB_R_RF_REGISTER_DTL
SET 
	[PR_CREATOR] = @PrCreator
	,[DESCRIPTION] = @Description
	,[AMOUNT] = @Amount
	,[UPDATED_BY] = @UpdatedBy
	,[UPDATED_DT] = getdate()
WHERE 
	1 = 1
	and RF_REGISTER_DTL_ID = @RegDtlId

