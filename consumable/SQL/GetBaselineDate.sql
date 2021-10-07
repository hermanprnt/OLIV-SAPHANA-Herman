select * from 
( 
	select 
		row_number () over (order by holiday_date asc) as number
	  ,holiday_date
	  ,convert(varchar, holiday_date, 104) holiday_date_str
	  ,baseline_date
	  ,convert(varchar, baseline_date, 104) baseline_date_str
	  ,created_by
	  ,created_dt
	  ,convert(varchar, created_dt, 104) created_dt_str
	  ,updated_by
	  ,updated_dt
	  ,convert(varchar, updated_dt, 104) updated_dt_str
	  from tb_m_baseline_date
	where 1=1
		and (nullif(@HOLIDAY_DATE_FROM,'') is null and nullif(@HOLIDAY_DATE_TO,'') is null)
			or (cast(holiday_date as date) between @HOLIDAY_DATE_FROM and @HOLIDAY_DATE_TO)
		and (nullif(@BASELINE_DATE_FROM,'') is null and nullif(@BASELINE_DATE_TO,'') is null)
			or (cast(baseline_date as date) between @BASELINE_DATE_FROM and @BASELINE_DATE_TO)
) reff 
where reff.number between @NumberFrom and @NumberTo;