﻿DECLARE @@ERR VARCHAR(100);

SET @@ERR = '';

IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(COUNTRY_CD, '') = '') > 0
BEGIN
	SET @@ERR = 'COUNTRY_CD';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(COMPANY_CD, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',COMPANY_CD';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(LEGACY_SYSTEM, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',LEGACY_SYSTEM';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(PART_CATEGORY, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',PART_CATEGORY';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(DOC_NO, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',DOC_NO';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(DOC_TYPE, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',DOC_TYPE';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(DOC_DT, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',DOC_DT';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(DOC_CURR, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',DOC_CURR';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(TERM_OF_PAYMENT, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',TERM_OF_PAYMENT';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(SUPP_CD, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',SUPP_CD';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(SUPP_PLANT_CD, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',SUPP_PLANT_CD';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(PART_RECEIVED_DT, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',PART_RECEIVED_DT';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(PO_NO, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',PO_NO';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(ACT_TTL_AMT, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',ACT_TTL_AMT';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(TAX_AMT, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',TAX_AMT';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(GRAND_AMT, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',GRAND_AMT';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(MARK_CD, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',MARK_CD';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(SIGN_CD, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',SIGN_CD';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(STOCK_MOVEMENT_FLAG, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',STOCK_MOVEMENT_FLAG';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(CANCEL_FLAG, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',CANCEL_FLAG';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(INTERFACE_TYPE_FLAG, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',INTERFACE_TYPE_FLAG';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(COA_FLAG, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',COA_FLAG';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(HEADER_RESERVED_3, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',HEADER_RESERVED_3';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(DETAIL_RESERVED_3, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',DETAIL_RESERVED_3';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(DETAIL_RESERVED_4, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',DETAIL_RESERVED_4';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(GR_NUMBER, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',GR_NUMBER';
END
IF (SELECT COUNT(1) FROM TB_T_BH00021 WHERE ISNULL(MATERIAL_INDICATOR, '') = '') > 0
BEGIN
	SET @@ERR = @@ERR + ',MATERIAL_INDICATOR';
END

SELECT @@ERR ERR