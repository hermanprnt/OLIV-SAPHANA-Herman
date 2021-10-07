IF EXISTS ( 
    SELECT * FROM TB_T_INVOICE_ATTACHMENT 
    WHERE ATTACHMENT_TYPE = @ATTACHMENT_TYPE 
    AND CREATED_BY = @CREATED_BY
    AND INVOICE_ID = @INVOICE_ID
    )
    UPDATE 
        TB_T_INVOICE_ATTACHMENT 
    SET 
        [FILE_NAME] = @FILE_NAME,
        FILE_NAME_SERVER = @FILE_NAME_SERVER,
        CREATED_DT = getdate(),
        CREATED_BY = @CREATED_BY
    WHERE 
        ATTACHMENT_TYPE = @ATTACHMENT_TYPE
        AND INVOICE_ID = @INVOICE_ID
ELSE
    INSERT INTO TB_T_INVOICE_ATTACHMENT
    (          
        ATTACHMENT_TYPE,
        INVOICE_ID,
	    [FILE_NAME],
        FILE_NAME_SERVER,
	    CREATED_DT,
	    CREATED_BY 
    ) 
        VALUES 
    (
        @ATTACHMENT_TYPE,
        @INVOICE_ID,
	    @FILE_NAME,  
        @FILE_NAME_SERVER,
	    getdate(),
	    @CREATED_BY
    );