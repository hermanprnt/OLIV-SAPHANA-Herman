SELECT NOREG
      ,NAME
      ,POSITION_CODE
      ,[LEVEL]
      ,[TYPE]
      ,POSITION_NAME
  FROM TB_M_PO_APPROVAL
  where NOREG = @NOREG