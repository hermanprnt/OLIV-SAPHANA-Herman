/*updating vat_in_h in E_FAKTUR DATABASE*/
DECLARE @@sql NVARCHAR(MAX);
		
SET @@sql = '
			UPDATE
				OPENQUERY([IPPCS_DB], ''
				select V1.USED AS USED, V1.CHANGED_BY, V1.CHANGED_DT from TMMIN_E_FAKTUR.dbo.tb_r_vat_in_h v1 
				where v1.TRANS_TYPE_CODE + '''' + v1.REPLACEMENT_FG + '''' + v1.TAX_INVOICE_NO = '''''+@TAX_INVOICE_NO+'''''
					and v1.REPLACEMENT_FG = (SELECT MAX(v2.REPLACEMENT_FG) FROM TMMIN_E_FAKTUR.dbo.TB_R_VAT_IN_H V2 
					WHERE V2.TAX_INVOICE_NO = v1.TAX_INVOICE_NO)'')
				SET USED = ''N'',
					CHANGED_BY = '''+@UPDATED_BY+''',
					CHANGED_DT = GETDATE()'

exec sp_executesql @@sql