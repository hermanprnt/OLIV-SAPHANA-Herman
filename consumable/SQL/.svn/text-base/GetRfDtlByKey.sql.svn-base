SELECT rDtl.[RF_REGISTER_DTL_ID]
      ,rDtl.PR_CREATOR
	  ,rDtl.[DESCRIPTION]
	  ,rDtl.AMOUNT
	  ,left(CONVERT(varchar, CAST([AMOUNT] AS money), 1), len(CONVERT(varchar, CAST([AMOUNT] AS money), 1)) - 3) As S_AMOUNT
FROM 
	TB_R_RF_REGISTER_DTL rDtl
WHERE 
	1 = 1
	and rDtl.[RF_REGISTER_ID] = @RfRegisterId	