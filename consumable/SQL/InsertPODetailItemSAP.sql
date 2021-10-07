
INSERT INTO [TB_R_PO_DETAIL_ITEM]
           ([PO_NUMBER]
           ,[PO_ITEM]
           ,[COMP_CODE]
           ,[COMP_RATE]
           ,[CREATED_DT]
           ,[CREATED_BY]
           )
     VALUES
           (@PO_NUMBER
           ,@PO_ITEM
           ,@COMP_CODE
           ,@COMP_RATE
           ,getdate()
           ,@CREATED_BY
           );