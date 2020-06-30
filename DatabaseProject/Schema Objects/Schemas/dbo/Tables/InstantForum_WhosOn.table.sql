/*CREATE TABLE [dbo].[InstantForum_WhosOn] (
    [WhosOnID]        INT            IDENTITY (1, 1) NOT NULL,
    [CurrentActivity] INT            /*DEFAULT ((0))*/ NOT NULL,
    [SessionIdentity] NVARCHAR (100) /*DEFAULT ('')*/ NOT NULL,
    [Username]        NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [UserID]          INT            /*DEFAULT ((0))*/ NOT NULL,
    [PrimaryRoleID]   INT            /*DEFAULT ((0))*/ NOT NULL,
    [ForumID]         INT            /*DEFAULT ((0))*/ NOT NULL,
    [ForumName]       NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [TopicID]         INT            /*DEFAULT ((0))*/ NOT NULL,
    [TopicTitle]      NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [EventID]         INT            /*DEFAULT ((0))*/ NOT NULL,
    [EventTitle]      NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [IPAddress]       NVARCHAR (50)  /*DEFAULT (N'')*/ NOT NULL,
    [IsAnonymous]     BIT            /*DEFAULT ((0))*/ NOT NULL,
    [DateStamp]       DATETIME       /*DEFAULT (getdate())*/ NOT NULL,
    [Expires]         DATETIME       /*DEFAULT (getdate())*/ NOT NULL
);*/

