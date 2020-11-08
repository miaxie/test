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
	CONSTRAINT ExpiresPoint_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('ExpiresPoint') IS NOT NULL
    PRINT '<<< CREATED TABLE ExpiresPoint >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE ExpiresPoint >>>'
go

/* 
 * TABLE: PointHistory 积分历史表
 */

CREATE TABLE PointHistory(
    Id             int         IDENTITY(1,1),
    TypeId         int         NOT NULL,
    Amount         int         NULL,
    CustomerId     int         NOT NULL,
    getTime        datetime    NOT NULL,
    UsedDate       datetime    NULL,
    ExpiredDate    datetime    NOT NULL,
    StatusId       int         NOT NULL,
    CONSTRAINT PointHistory_PK PRIMARY KEY CLUSTERED (Id)
)
go



IF OBJECT_ID('PointHistory') IS NOT NULL
    PRINT '<<< CREATED TABLE PointHistory >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE PointHistory >>>'
go

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

/* 
 * TABLE: ConsumeDetail 
 */

ALTER TABLE ConsumeDetail ADD CONSTRAINT RefPointHistory81 
    FOREIGN KEY (PointHistoryId)
    REFERENCES PointHistory(Id)
go

ALTER TABLE ConsumeDetail ADD CONSTRAINT RefConsumeHistory101 
    FOREIGN KEY (ConsumeHistoryId)
    REFERENCES ConsumeHistory(Id)
go


/* 
 * TABLE: ExpiresPoint 
 */

ALTER TABLE ExpiresPoint ADD CONSTRAINT RefPointHistory141 
    FOREIGN KEY (PointHistoryId)
    REFERENCES PointHistory(Id)
go


