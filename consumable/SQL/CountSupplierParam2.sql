SELECT 
	count(*)
FROM 
	TB_M_SUPPLIER
WHERE 
	SUPPLIER_CD like '%' + ISNULL(@Parameter1,'') + '%'
	AND SUPPLIER_NAME like '%' + ISNULL(@Parameter2,'') + '%'
