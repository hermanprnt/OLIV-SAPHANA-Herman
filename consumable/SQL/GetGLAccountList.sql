SELECT GL_ACCOUNT_ID
      ,[GL_ACCOUNT_NO]
      ,[CATEGORY_CD]
      ,[TYPE]
      ,[NAME]
      ,[CODE]
      ,[PERCENTAGE]
  FROM TB_M_GL_ACCOUNT  WITH (NOLOCK)
  where CATEGORY_CD = @CATEGORY_CD