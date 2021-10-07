DECLARE @@sqlstate varchar(max);

SET @@sqlstate = '';

SET @@sqlstate = @@sqlstate + '
	SELECT 
		count(*)
	FROM TB_R_INVOICE inv
		left join TB_M_SUPPLIER s on s.SUPPLIER_CD = inv.SUPPLIER_CD
	where 1=1
	and cast(RECEIVED_DT as DATE) = cast(getDate() as DATE)
	  ';

print(@@sqlstate);

execute(@@sqlstate);
