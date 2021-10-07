DECLARE @@sqlstate varchar(max);
DECLARE @@MATDOC_NO  varchar(50);

SET @@sqlstate = '';
SET @@MATDOC_NO = @MATDOC_NO;


SET @@sqlstate = @@sqlstate + '
    select 
        *
    from 
        TB_R_GR_IR_FROM_SAP 
    where 1=1 ';

IF(@@MATDOC_NO <> '')
BEGIN
    SET @@sqlstate = @@sqlstate + '
    and MATDOC_NUMBER = '''+ @@MATDOC_NO + ''' ';
END

SET @@sqlstate = @@sqlstate + '
    order by MATDOC_DATE desc';


print(@@sqlstate);

execute(@@sqlstate);
