﻿select * from
(
select SYSTEM_VALUE_TEXT, SYSTEM_VALUE_NUM from TB_M_SYSTEM
where SYSTEM_TYPE = 'SCAN_BARCODE_PROXY' and SYSTEM_CD = 'NEED_PROXY'

union

select SYSTEM_VALUE_TEXT, SYSTEM_VALUE_NUM from TB_M_SYSTEM
where SYSTEM_TYPE = 'SCAN_BARCODE_PROXY' and SYSTEM_CD = 'HOST_STRING'

union

select SYSTEM_VALUE_TEXT, SYSTEM_VALUE_NUM from TB_M_SYSTEM
where SYSTEM_TYPE = 'SCAN_BARCODE_PROXY' and SYSTEM_CD = 'USER'

union

select SYSTEM_VALUE_TEXT, SYSTEM_VALUE_NUM from TB_M_SYSTEM
where SYSTEM_TYPE = 'SCAN_BARCODE_PROXY' and SYSTEM_CD = 'PASSWORD'

union

select SYSTEM_VALUE_TEXT, SYSTEM_VALUE_NUM from TB_M_SYSTEM
where SYSTEM_TYPE = 'SCAN_BARCODE_PROXY' and SYSTEM_CD = 'DOMAIN'

union

select SYSTEM_VALUE_TEXT, SYSTEM_VALUE_NUM from TB_M_SYSTEM
where SYSTEM_TYPE = 'SCAN_BARCODE_PROXY' and SYSTEM_CD = 'PORT'

) as TB
order by TB.SYSTEM_VALUE_NUM