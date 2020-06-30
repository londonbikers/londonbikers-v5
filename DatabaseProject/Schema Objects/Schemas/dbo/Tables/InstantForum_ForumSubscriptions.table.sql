/*CREATE TABLE [dbo].[InstantForum_ForumSubscriptions] (
    [ForumSubscriptionID] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ForumID]             INT      NOT NULL,
    [UserID]              INT      NOT NULL,
    [SubscriptionType]    TINYINT  /*DEFAULT ((0))*/ NOT NULL,
    [DateStamp]           DATETIME /*DEFAULT (getdate())*/ NOT NULL
);*/

