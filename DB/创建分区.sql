
--创建文件组[EcPoints]
alter database EcPoints add filegroup fileGroup01
alter database EcPoints add filegroup fileGroup02
alter database EcPoints add filegroup fileGroup03
alter database EcPoints add filegroup fileGroup04
alter database EcPoints add filegroup fileGroup05
alter database EcPoints add filegroup fileGroup06
alter database EcPoints add filegroup fileGroup07
alter database EcPoints add filegroup fileGroup08
alter database EcPoints add filegroup fileGroup09
alter database EcPoints add filegroup fileGroup10
alter database EcPoints add filegroup fileGroup11
alter database EcPoints add filegroup fileGroup12



---创建数据文件到文件组
alter database EcPoints add file (name='EcPoints01',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints01.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup01
alter database EcPoints add file (name='EcPoints02',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints02.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup02
alter database EcPoints add file (name='EcPoints03',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints03.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup03
alter database EcPoints add file (name='EcPoints04',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints04.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup04
alter database EcPoints add file (name='EcPoints05',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints05.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup05
alter database EcPoints add file (name='EcPoints06',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints06.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup06
alter database EcPoints add file (name='EcPoints07',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints07.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup07
alter database EcPoints add file (name='EcPoints08',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints08.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup08
alter database EcPoints add file (name='EcPoints09',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints09.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup09
alter database EcPoints add file (name='EcPoints10',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints10.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup10
alter database EcPoints add file (name='EcPoints11',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints11.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup11
alter database EcPoints add file (name='EcPoints12',filename=N'D:\Mia\EcPoints\test\DB\fileGroup\EcPoints12.ndf',size=5Mb,filegrowth=5mb) to filegroup fileGroup12

--创建分区函数
create partition function [PF_EcPoints] (datetime) as range right for values
('2020-01-01','2020-02-01','2020-03-01','2020-04-01','2020-05-01','2020-06-01',
'2020-07-01','2020-08-01','2020-09-01','2020-10-01','2020-11-01','2020-12-01')


--创建分区方案
CREATE PARTITION SCHEME [PS_EcPoints] AS PARTITION [PF_EcPoints]
TO ([PRIMARY], [fileGroup01], [fileGroup02], [fileGroup03], [fileGroup04], [fileGroup05], 
[fileGroup06], [fileGroup07], [fileGroup08], [fileGroup09], [fileGroup10], [fileGroup11], [fileGroup12])

--按分区创建表和索引
--create table gf_card (
--  id            bigint not null,
--  crt_time        datetime not null,
--  name            varchar(20) not null
--) on [PS_card] (crt_time)



