/*CREATE TABLE [dbo].[InstantASP_Tags] (
    [TagID]             INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationID]     TINYINT        DEFAULT ((0)) NOT NULL,
    [TabID]             INT            DEFAULT ((0)) NOT NULL,
    [RelatedEntityID]   INT            DEFAULT ((0)) NOT NULL,
    [TagKeyword]        NVARCHAR (255) NOT NULL,
    [TagKeywordEncoded] NVARCHAR (255) DEFAULT ('') NOT NULL,
    [UserID]            INT            DEFAULT ((0)) NOT NULL,
    [DateStamp]         DATETIME       DEFAULT (getdate()) NOT NULL
);*/

