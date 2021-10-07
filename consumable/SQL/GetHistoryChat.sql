
SELECT CHAT_MESSAGE,
CHAT_FROM,
CHAT_TO,
CHAT_DATETIME,
FORMAT (CHAT_DATETIME, 'dd MMM yyyy hh:mm:ss ') as CHAT_DATETIME_STR
FROM TB_R_NOTICE_CHAT
WHERE INVOICE_ID = @INVOICE_ID
ORDER BY CHAT_DATETIME 