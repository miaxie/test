

----删除文件组
--alter database test remove filegroup  fileGroup01
---删除数据文件到文件组
--alter database test remove file  test01
--删除分区函数
--drop partition function [PF_ScoreHistory]
--删除分区方案
--drop PARTITION SCHEME [PS_ScoreHistory]

--修改边界值
ALTER PARTITION SCHEME [PS_card] NEXT USED [fileGroup01]
alter partition function [PF_card]()  SPLIT RANGE ('2047-01-01')

ALTER PARTITION SCHEME [PS_scoreHistory] NEXT USED [fileGroup01]
alter partition function [PF_scoreHistory]()  SPLIT RANGE ('2021-01-01')


-- 查看指定分区中的数据记录
select * from scoreHistory where $partition.[PF_ScoreHistory](createdate)=3
select * from sys.partition_schemes -- 查询分区
select * from sys.partition_range_values -- 查询分区范围
select * from sys.partition_functions --查询分区函数
