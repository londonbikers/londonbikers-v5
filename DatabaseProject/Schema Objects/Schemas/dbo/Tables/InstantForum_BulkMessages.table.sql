/*CREATE TABLE [dbo].[InstantForum_BulkMessages] (
    [BulkMessageID]           INT            IDENTITY (1, 1) NOT NULL,
    [Subject]                 NVARCHAR (400) /*DEFAULT ('')*/ NOT NULL,
    [Message]                 NTEXT          /*DEFAULT ('')*/ NOT NULL,
    [GroupRecipients]         NVARCHAR (400) /*DEFAULT ('')*/ NOT NULL,
    [SendAsType]              TINYINT        /*DEFAULT ((1))*/ NOT NULL,
    [HonorOptIn]              BIT            /*DEFAULT ((1))*/ NOT NULL,
    [UserNoOfVisits]          INT            /*DEFAULT ((0))*/ NOT NULL,
    [UserNoOfVisitsWith]      TINYINT        NOT NULL,
    [UserNoOfPosts]           INT            NOT NULL,
    [UserNoOfPostsWith]       TINYINT        NOT NULL,
    [UserLastVisitDate]       SMALLINT       /*DEFAULT ((0))*/ NOT NULL,
    [UserLastVisitDateOrder]  TINYINT        NOT NULL,
    [LastSentDate]            DATETIME       NULL,
    [DateStamp]               DATETIME       /*DEFAULT (getdate())*/ NOT NULL,
    [IncludeSendersSignature] BIT            /*DEFAULT ((1))*/ NOT NULL
);*/

