INSERT INTO TB_M_GL_ACCOUNT
(          
    [GL_ACCOUNT_NO]
    ,[CATEGORY_CD]
    ,[TYPE]
    ,[NAME]
    ,[CODE]
    ,[PERCENTAGE]
	,[CREATED_BY]
    ,[CREATED_DT]
) 
VALUES 
(
    @GL_ACCOUNT_NO,  
    @CATEGORY_CD,  
    @TYPE, 
	@NAME,
	@CODE, 
	@PERCENTAGE,
	@CREATED_BY,    
    GETDATE()
);

