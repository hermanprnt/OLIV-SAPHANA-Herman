if exists (select b.AUTO_POSTING_FLAG from tb_r_invoice a
				join tb_m_supplier b on a.supplier_cd = b.supplier_cd
				where a.certificate_id= @CERTIFICATE_ID
				and b.auto_posting_flag='Y')
begin
	select 1
end
else
begin
	if exists (select 0 from tb_r_invoice a
				where a.CERTIFICATE_ID = @CERTIFICATE_ID
				and a.RECEIVED_STATUS = 'ACCEPT'
				and a.[STATUS] = 'READY_TO_POST')
	begin
		select 1
	end
	else
	begin
		select 0
	end
end