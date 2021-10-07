SELECT r.[RF_REGISTER_ID]
      ,r.[RF_NO]
	  ,r.[RF_DT]
	  ,convert(varchar, convert(datetime, r.[RF_DT]), 104) S_RF_DT
      ,r.[PIC_USER]
      ,r.[DIVISION]
      ,r.[TOTAL_AMOUNT]
	  ,left(CONVERT(varchar, CAST([TOTAL_AMOUNT] AS money), 1), len(CONVERT(varchar, CAST([TOTAL_AMOUNT] AS money), 1)) - 3) As S_TOTAL_AMOUNT
      ,r.[WBS_NO]
      ,r.[CREATED_DT]
      ,r.[CREATED_BY]
      ,r.[UPDATED_DT]
      ,r.[UPDATED_BY]
FROM TB_R_RF_REGISTER r
WHERE 1 = 1
	and r.[RF_REGISTER_ID] = @RfRegisterId	