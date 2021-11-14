﻿DECLARE @@OLD_SEQ INT;

SELECT @@OLD_SEQ = SYSTEM_VALUE_TEXT FROM TB_M_SYSTEM WHERE SYSTEM_TYPE = 'BH00021' and SYSTEM_CD = 'SEQ_INTERFACE_FILE' 

UPDATE A
SET A.SYSTEM_VALUE_TEXT = @@OLD_SEQ + 1
FROM TB_M_SYSTEM A WHERE SYSTEM_TYPE = 'BH00021' and SYSTEM_CD = 'SEQ_INTERFACE_FILE' 

UPDATE A
SET A.SAP_STATUS = 'Y',
	A.CREATED_BY = 'SYSTEM',
	A.CREATED_DT = GETDATE()
FROM TB_R_INVOICE_SAP_INPUT A
JOIN TB_T_BH00021 B ON A.INVOICE_ID = B.INVOICE_ID