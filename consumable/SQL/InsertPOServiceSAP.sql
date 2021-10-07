
INSERT INTO [TB_R_PO_SERVICE]
           ([PO_NUMBER]
           ,[PO_ITEM]
           ,[SERV_ITEM]
           ,[SERV_TEXT]
           ,[SERV_UNIT]
           ,[SERV_QTY]
           ,[SERV_GPRICE]
           ,[CREATED_DT]
           ,[CREATED_BY]
          )
     VALUES
           (@PO_NUMBER
           ,@PO_ITEM
           ,@SERV_ITEM
           ,@SERV_TEXT
           ,@SERV_UNIT
           ,@SERV_QTY
           ,@SERV_GPRICE
           ,getdate()
           ,@CREATED_BY
         );


