select  * from scoreHistory
select  * from ScoreType


declare @tempDay table(currentDay int)
declare @i int;
declare @total int;
set @i=1 ;
set @total=30;
while(@i<@total)
begin
insert into @tempDay 
values (@i)
set @i=@i+1;
end

insert into scoreHistory
select 1,10,1,1,DATEADD(day,currentDay, '2020-01-01'),DATEADD(YEAR,1,GETDATE())
from @tempDay
 



insert into ScoreType
values (1,N'ǩ��',1,GETDATE(),N'ÿ���һ��')


-- ��ѯ������Щ����
SELECT *
FROM sys.partitions AS p JOIN sys.tables AS t  ON  p.object_id = t.object_id
  WHERE p.partition_id IS NOT NULL
      AND t.name = 'PointHistory';


--��ѯ��������
select * from sys.partition_functions
