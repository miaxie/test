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
values (1,N'签到',1,GETDATE(),N'每天仅一次')


-- 查询表有哪些分区
SELECT *
FROM sys.partitions AS p JOIN sys.tables AS t  ON  p.object_id = t.object_id
  WHERE p.partition_id IS NOT NULL
      AND t.name = 'PointHistory';


--查询分区函数
select * from sys.partition_functions
