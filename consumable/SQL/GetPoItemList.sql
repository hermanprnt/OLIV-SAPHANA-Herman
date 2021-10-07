SELECT [PO_ITEM]
      ,[MAT_NUMBER]
	  ,[MAT_DESCR]
	  ,[PO_QTY_NEW]
      ,[PO_UNIT]
      ,left(CONVERT(varchar, CAST([PRICE_PER_UNIT] AS money), 1), len(CONVERT(varchar, CAST([PRICE_PER_UNIT] AS money), 1)) - 3) As PRICE_PER_UNIT
FROM TB_R_PO_ITEM
WHERE 1 = 1
	and [PO_NUMBER] = @PO_NO	