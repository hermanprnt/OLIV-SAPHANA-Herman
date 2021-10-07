INSERT INTO [TB_L_CANCEL_GR_SAP]
           ([MATDOC_NUMBER_IN]
           ,[ENTRYSHEET_NUM_IN]
           ,[MATDOC_YEAR_IN]
           ,[MATDOC_DOCDATE_IN]
           ,[MATDOC_POSTDATE_IN]
           ,[MATDOC_TEXT_IN]
           ,[MATDOC_NUMBER_OUT]
           ,[MATDOC_YEAR_OUT]
           ,[REVMAT_NUMBER_OUT]
           ,[REVMAT_YEAR_OUT]
           ,[TYPE_OUT]
           ,[MESSAGE_OUT]
           ,[CREATED_DT]
           ,[CREATED_BY]
        )
     VALUES
           (@MATDOC_NUMBER_IN
           ,@ENTRYSHEET_NUM_IN
           ,@MATDOC_YEAR_IN
           ,@MATDOC_DOCDATE_IN
           ,@MATDOC_POSTDATE_IN
           ,@MATDOC_TEXT_IN
           ,@MATDOC_NUMBER_OUT
           ,@MATDOC_YEAR_OUT
           ,@REVMAT_NUMBER_OUT
           ,@REVMAT_YEAR_OUT
           ,@TYPE_OUT
           ,@MESSAGE_OUT
           ,getdate()
           ,@CREATED_BY
          );


