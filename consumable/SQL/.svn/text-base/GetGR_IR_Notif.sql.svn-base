DECLARE @@sqlstate varchar(max);
DECLARE @@VEND_CODE  varchar(50);

SET @@sqlstate = '';
SET @@VEND_CODE = @VEND_CODE;

SET @@sqlstate = @@sqlstate + '
	select 
		ROW_NUMBER () OVER (ORDER BY MATDOC_DATE) AS Number
		,gr.PO_NUMBER
		,gr.PO_ITEM
		,gr.MATDOC_YEAR
		,gr.MATDOC_NUMBER
		,gr.MATDOC_ITEM
		,gr.MATDOC_DATE
		,gr.SPC_STOCK
		,gr.MATDOC_AMOUNT
		,gr.VEND_CODE
		,s.SUPPLIER_NAME
		,gr.PURCHDOC_PRICE
		,gr.MAT_NUMBER
		,gr.MAT_DESCR
		,gr.PLANT_CODE
		,gr.SLOC_CODE
		,gr.MATDOC_UNIT
		,gr.CANCEL
		,gr.ORI_MAT_NUMBER
		,gr.MATDOC_CURRENCY
		,gr.MATDOC_QTY
		,gr.TAX_CODE
		,gr.PO_DATE
		,gr.GR_STATUS
		,gr.HEADER_TEXT
		,gr.IR_NO
		,gr.MOV_TYPE
		,gr.REF_DOC
		,gr.USRID
		,gr.CANCEL_DOC_NO
	from 
		TB_R_GR_IR_FROM_SAP gr
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = gr.VEND_CODE
	where 1=1 
		and NOTIF_FLAG = ''Y'' ';

IF(@@VEND_CODE <> '')
BEGIN
	SET @@sqlstate = @@sqlstate + '
	and gr.VEND_CODE = '''+ @@VEND_CODE + ''' ';
END

print(@@sqlstate);

execute(@@sqlstate);
