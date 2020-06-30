/*CREATE TABLE [dbo].[InstantForum_Events] (
    [EventID]         INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          INT            /*DEFAULT ((0))*/ NOT NULL,
    [Title]           NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [Message]         NTEXT          NULL,
    [EventStartMonth] TINYINT        NOT NULL,
    [EventStartYear]  SMALLINT       NOT NULL,
    [EventStartDay]   TINYINT        NOT NULL,
    [EventStart]      DATETIME       /*DEFAULT (getdate())*/ NOT NULL,
    [EventEndMonth]   TINYINT        NOT NULL,
    [EventEndYear]    SMALLINT       NOT NULL,
    [EventEndDay]     TINYINT        NOT NULL,
    [EventEnd]        DATETIME       /*DEFAULT (getdate())*/ NOT NULL,
    [PublicEvent]     BIT            /*DEFAULT ((0))*/ NOT NULL,
    [BackGroundColor] NVARCHAR (100) /*DEFAULT ('')*/ NOT NULL,
    [FontColor]       NVARCHAR (100) /*DEFAULT ('')*/ NOT NULL,
    [RecurringType]   TINYINT        /*DEFAULT ((0))*/ NOT NULL,
    [IPAddress]       NVARCHAR (50)  /*DEFAULT ('')*/ NOT NULL,
    [DateStamp]       DATETIME       /*DEFAULT (getdate())*/ NOT NULL,
    [EditUserID]      INT            /*DEFAULT ((0))*/ NOT NULL,
    [EditUsername]    NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [EditDateStamp]   DATETIME       /*DEFAULT (getdate())*/ NOT NULL
);*/

