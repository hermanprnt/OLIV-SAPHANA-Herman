DECLARE @@sqlstate nvarchar(max);
DECLARE @@VEND_CODE  varchar(MAX);
DECLARE @@PO_NUMBER  varchar(50);

SET @@sqlstate = '';
SET @@VEND_CODE = @VEND_CODE;
SET @@PO_NUMBER = @PO_NUMBER;

SET @@sqlstate = @@sqlstate + '
	SELECT top 1 
	   poh.[PO_NUMBER]
      ,[VEND_CODE]
      ,[VEND_NAME]
      ,[ADDRESS]
      ,[POST_CODE]
      ,[CITY]
      ,[ATTENTION]
      ,[TEL_NUMBER]
      ,[FAX_NUMBER]
      ,[DELIV_NAME]
      ,[DELIV_ADDR]
      ,[DELIV_POST]
      ,[DELIV_CITY]
      ,[CONTACT_PER]
      ,[PO_DATE]
      ,[PO_PAYTERM]
      ,[PO_TYPE]
      ,[PO_CAT]
      ,[PO_PURCH_GRP]
      ,[PO_CURRENCY]
      ,coalesce([PO_AMOUNT],0) as PO_AMOUNT_TEMP
      ,[PO_XCHG_RATE]
      ,[PO_DELETE_FLG]
      ,[PO_INCOTERM1]
      ,[PO_INCOTERM2]
      ,poh.[CREATED_DT]
      ,poh.[CREATED_BY]
      ,poh.[UPDATED_DT]
      ,poh.[UPDATED_BY]
      ,[APP_SH_DT] as SH_DATE
      ,[APP_SH_BY]
      ,[APP_SH_STATUS]
      ,[APP_DPH_DT] as DPH_DATE
      ,[APP_DPH_BY]
      ,[APP_DPH_STATUS]
      ,[APP_DH_DT] as DH_DATE
      ,[APP_DH_BY]
      ,[APP_DH_STATUS]
	  ,[NOTICE_BY]
      ,[NOTICE_REMARK]
      ,[NOTICE_DATE]
	  ,sh.NAME as SH_NAME
	  ,dph.NAME as DPH_NAME
	  ,dh.NAME as DH_NAME
	  
	  ,poi.PO_QTY_NEW as Qty
	  ,poi.MAT_DESCR as Description
	  ,poi.PO_UNIT as Unit
	  ,poi.PO_ITEM as Item
	  ,poi.PRICE_PER_UNIT as Price

	FROM TB_R_PO_HEADER poh
	left join TB_M_PO_APPROVAL sh on sh.NOREG = poh.APP_SH_BY
	left join TB_M_PO_APPROVAL dph on dph.NOREG = poh.APP_DPH_BY
	left join TB_M_PO_APPROVAL dh on dh.NOREG = poh.APP_DH_BY
	left join TB_R_PO_ITEM poi on poi.PO_NUMBER = poh.PO_NUMBER
	where 1=1
		';

IF(@@PO_NUMBER <> '' AND @@PO_NUMBER is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and poh.PO_NUMBER = '''+ @@PO_NUMBER + ''' ';
END

IF(@@VEND_CODE <> '' AND @@VEND_CODE is not null)
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and poh.VEND_CODE = '''+ @@VEND_CODE +''' ';
END

--print(@@sqlstate);

execute(@@sqlstate);
