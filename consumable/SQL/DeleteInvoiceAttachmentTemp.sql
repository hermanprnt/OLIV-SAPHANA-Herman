﻿DELETE FROM TB_T_INVOICE_ATTACHMENT			
WHERE ATTACHMENT_TYPE = @ATTACHMENT_TYPE
AND [FILE_NAME] = @FILE_NAME
AND CREATED_BY = @CREATED_BY