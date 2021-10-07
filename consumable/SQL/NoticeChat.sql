
INSERT INTO [dbo].[TB_R_NOTICE_CHAT]
           ([ID]
           ,[INVOICE_ID]
           ,[CHAT_FROM]
           ,[CHAT_TO]
           ,[CHAT_MESSAGE]
           ,[CHAT_DATETIME])
     VALUES
           (newid()
           ,@INVOICE_ID
           ,@CHAT_FROM
           ,@CHAT_TO
           ,@CHAT_MESSAGE
           ,getdate())

IF @CHAT_FROM = 'Finance'
BEGIN
  UPDATE TB_R_INVOICE
   SET 
   NOTICE_FLAG = 'Y',
   NOTICE_DATE = GETDATE(),
   UPDATED_BY = @CREATED_BY,
   UPDATED_DT = GETDATE()
   WHERE INVOICE_ID = @INVOICE_ID
END
