if not exists (select 1 from sys.databases where name='ECPoints')
begin
Create database ECPoints
end
go
use ECPoints
/*
 * ER/Studio 8.0 SQL Code Generation
 * Company :      mia
 * Project :      Model1.DM1
 * Author :       Mia
 *
 * Date Created : Sunday, October 11, 2020 19:36:50
 * Target DBMS : Microsoft SQL Server 2008
 */
 
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
('2020-11-01','2020-12-01','2021-01-01','2021-02-01','2021-03-01','2021-04-01','2021-05-01','2021-06-01',
'2021-07-01','2021-08-01','2021-09-01','2021-10-01')


--创建分区方案
CREATE PARTITION SCHEME [PS_EcPoints] AS PARTITION [PF_EcPoints]
TO ([PRIMARY], [fileGroup01], [fileGroup02], [fileGroup03], [fileGroup04], [fileGroup05], 
[fileGroup06], [fileGroup07], [fileGroup08], [fileGroup09], [fileGroup10], [fileGroup11], [fileGroup12])

/* 
 * TABLE: ConfirmHistory 积分确认历史表
 */

CREATE TABLE ConfirmHistory(
    Id                   int         IDENTITY(1,1),
    CustomerId           int         NOT NULL,
    Amount               int         DEFAULT 0 NOT NULL,
    sourceId             int         DEFAULT 0 NOT NULL,
    GetTime              datetime    NOT NULL,
    TypeId               int         NOT NULL,
    StatusId             int         NOT NULL,
    ConfirmCustomerId    int         NOT NULL,
    ConfirmDate          datetime    NULL,
    CONSTRAINT PK6 PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('ConfirmHistory') IS NOT NULL
    PRINT '<<< CREATED TABLE ConfirmHistory >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE ConfirmHistory >>>'
go

/* 
 * TABLE: ConsumeDetail 积分消费历史明细表
 */

CREATE TABLE ConsumeDetail(
    Id                   int         IDENTITY(1,1),
    PointHistoryId      int    NOT NULL,
    ConsumeHistoryId    int    NOT NULL,
    Amount              int    DEFAULT 0 NOT NULL,
	CONSTRAINT ConsumeDetail_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('ConsumeDetail') IS NOT NULL
    PRINT '<<< CREATED TABLE ConsumeDetail >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE ConsumeDetail >>>'
go

/* 
 * TABLE: ConsumeHistory  积分消费历史表
 */

CREATE TABLE ConsumeHistory(
    Id               int         IDENTITY(1,1),
    CustomerId       int         NOT NULL,
    TotalAmount      int         DEFAULT 0 NOT NULL,
    ConsumDate       datetime    NOT NULL,
    ConsumeTypeId    int         DEFAULT 0 NOT NULL,
	Remark           Nvarchar(500) null,
    CONSTRAINT ConsumeHistory_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('ConsumeHistory') IS NOT NULL
    PRINT '<<< CREATED TABLE ConsumeHistory >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE ConsumeHistory >>>'
go



/* 
 * TABLE: CustomerPoints 会员积分余额表
 */

CREATE TABLE CustomerPoints(
    Id            int    IDENTITY(1,1),
    CustomerId    int    NOT NULL,
    Amount        int    DEFAULT 0 NOT NULL,
	UpdateDate    datetime not null,
    CONSTRAINT CustomerPoints_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('CustomerPoints') IS NOT NULL
    PRINT '<<< CREATED TABLE CustomerPoints >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE CustomerPoints >>>'
go



/* 
 * TABLE: ExpiresPoint 过期积分表
 */
CREATE TABLE ExpiresPoint(
	Id            int         IDENTITY(1,1),
    PointHistoryId     int      NOT NULL,
    CustomerId    int         NOT NULL,
    Amount        int         DEFAULT 0 NOT NULL,
    [ExpireDate]    datetime    NOT NULL,
	CreateDate datetime not null,
	RemainAmount  int         DEFAULT 0 NOT NULL,
	CONSTRAINT ExpiresPoint_PK PRIMARY KEY CLUSTERED (Id)
)
go


IF OBJECT_ID('ExpiresPoint') IS NOT NULL
    PRINT '<<< CREATED TABLE ExpiresPoint >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE ExpiresPoint >>>'
go

BEGIN TRANSACTION

ALTER TABLE [dbo].[ExpiresPoint] DROP CONSTRAINT [ExpiresPoint_PK]


ALTER TABLE [dbo].[ExpiresPoint] ADD  CONSTRAINT [ExpiresPoint_PK] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


CREATE CLUSTERED INDEX [ClusteredIndex_on_PS_EcPoints_637410639492374544] ON [dbo].[ExpiresPoint]
(
	[CreateDate]
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PS_EcPoints]([CreateDate])


DROP INDEX [ClusteredIndex_on_PS_EcPoints_637410639492374544] ON [dbo].[ExpiresPoint]

COMMIT TRANSACTION

/* 
 * TABLE: PointHistory 积分历史表
 */

CREATE TABLE PointHistory(
    Id             int         IDENTITY(1,1),
    TypeId         int         NOT NULL,
    Amount         int         not NULL default 0,
    CustomerId     int         NOT NULL,
    getTime        datetime    NOT NULL,
    UsedDate       datetime    NULL,
    ExpiredDate    datetime    NOT NULL,
    StatusId       int         NOT NULL,
	RemainAmount   int         not NULL default 0,
    CONSTRAINT PointHistory_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('PointHistory') IS NOT NULL
    PRINT '<<< CREATED TABLE PointHistory >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE PointHistory >>>'
go


BEGIN TRANSACTION

ALTER TABLE [dbo].[PointHistory] DROP CONSTRAINT [PointHistory_PK]
ALTER TABLE [dbo].[PointHistory] ADD  CONSTRAINT [PointHistory_PK] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE CLUSTERED INDEX [ClusteredIndex_on_PS_EcPoints_637410615430843817] ON [dbo].[PointHistory]
(
	[getTime]
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PS_EcPoints]([getTime])

DROP INDEX [ClusteredIndex_on_PS_EcPoints_637410615430843817] ON [dbo].[PointHistory]
COMMIT TRANSACTION
/* 
 * TABLE: Setting 
 */

CREATE TABLE Setting(
    Id                int            IDENTITY(1,1),
    Name              varchar(50)    NOT NULL,
    AttributeValue    varchar(50)    NOT NULL,
    CONSTRAINT Setting_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('Setting') IS NOT NULL
    PRINT '<<< CREATED TABLE Setting >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE Setting >>>'
go

/* 
 * INDEX: Ref28 
 */

CREATE INDEX Ref28 ON ConsumeDetail(Id)
go
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('ConsumeDetail') AND name='Ref28')
    PRINT '<<< CREATED INDEX ConsumeDetail.Ref28 >>>'
ELSE
    PRINT '<<< FAILED CREATING INDEX ConsumeDetail.Ref28 >>>'
go

/* 
 * INDEX: Ref214 
 */

CREATE INDEX Ref214 ON ExpiresPoint(Id)
go
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('ExpiresPoint') AND name='Ref214')
    PRINT '<<< CREATED INDEX ExpiresPoint.Ref214 >>>'
ELSE
    PRINT '<<< FAILED CREATING INDEX ExpiresPoint.Ref214 >>>'
go




BEGIN TRANSACTION

ALTER TABLE [dbo].[ConsumeHistory] DROP CONSTRAINT [ConsumeHistory_PK]


ALTER TABLE [dbo].[ConsumeHistory] ADD  CONSTRAINT [ConsumeHistory_PK] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


CREATE CLUSTERED INDEX [ClusteredIndex_on_PS_EcPoints_637410638917457113] ON [dbo].[ConsumeHistory]
(
	[ConsumDate]
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PS_EcPoints]([ConsumDate])


DROP INDEX [ClusteredIndex_on_PS_EcPoints_637410638917457113] ON [dbo].[ConsumeHistory]


COMMIT TRANSACTION


insert into Setting
values ('Point.ClearCount','1000'),
('Point.ExpiredDate','30')

--清空数据
--DELETE [PointHistory]; 
--DBCC CHECKIDENT('PointHistory', RESEED, 0);
