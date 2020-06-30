/*CREATE TABLE [dbo].[InstantForum_Topics] (
    [PostID]             INT            IDENTITY (1, 1) NOT NULL,
    [ForumID]            INT            NOT NULL,
    [TopicID]            INT            NOT NULL,
    [ParentID]           INT            NOT NULL,
    [UserID]             INT            NOT NULL,
    [MessageIconName]    NVARCHAR (200) NOT NULL,
    [LastPosterUserID]   INT            NOT NULL,
    [LastPosterUsername] NVARCHAR (255) NOT NULL,
    [LastPosterPostID]   INT            NOT NULL,
    [Replies]            INT            NOT NULL,
    [Views]              INT            NOT NULL,
    [Title]              NVARCHAR (300) NOT NULL,
    [Description]        NVARCHAR (300) NOT NULL,
    [DateStamp]          DATETIME       NOT NULL,
    [LastPosterDate]     DATETIME       NOT NULL,
    [Queued]             BIT            NOT NULL,
    [IsPinned]           BIT            NOT NULL,
    [IsLocked]           BIT            NOT NULL,
    [IsPoll]             BIT            NOT NULL,
    [IsMoved]            BIT            NOT NULL,
    [Rating]             TINYINT        NOT NULL,
    [TotalRatings]       INT            NOT NULL,
    [HasAttachments]     BIT            NOT NULL,
    [EditUserID]         INT            NOT NULL,
    [EditUsername]       NVARCHAR (255) NOT NULL,
    [EditDateStamp]      DATETIME       NOT NULL,
    [IPAddress]          NVARCHAR (50)  NOT NULL,
    [TitleEncoded]       NVARCHAR (300) DEFAULT ('') NOT NULL
);*/



