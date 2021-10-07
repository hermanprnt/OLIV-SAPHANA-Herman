select holiday_date
	  ,convert(varchar, holiday_date, 104) holiday_date_str
	  ,baseline_date
	  ,convert(varchar, baseline_date, 104) baseline_date_str
	  ,created_by
	  ,created_dt
	  ,convert(varchar, created_dt, 104) created_dt_str
	  ,updated_by [changed_by]
	  ,updated_dt [changed_dt]
	  ,convert(varchar, updated_dt, 104) changed_dt_str
	  from tb_m_baseline_date
	where 1=1
		and holiday_date = @HOLIDAY_DATE