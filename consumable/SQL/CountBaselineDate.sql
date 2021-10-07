select count(1)
from tb_m_baseline_date
where 1=1
	and (nullif(@HOLIDAY_DATE_FROM,'') is null and nullif(@HOLIDAY_DATE_TO,'') is null)
		or (cast(holiday_date as date) between @HOLIDAY_DATE_FROM and @HOLIDAY_DATE_TO)
	and (nullif(@BASELINE_DATE_FROM,'') is null and nullif(@BASELINE_DATE_TO,'') is null)
		or (cast(baseline_date as date) between @BASELINE_DATE_FROM and @BASELINE_DATE_TO)