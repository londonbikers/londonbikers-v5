/*CREATE TABLE [dbo].[InstantForum_SearchResults] (
    [ResultID]        INT            IDENTITY (1, 1) NOT NULL,
    [PostID]          INT            NOT NULL,
    [SessionIdentity] NVARCHAR (100) NOT NULL,
    [Rank]            INT            DEFAULT ((0)) NOT NULL,
    [DateStamp]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [Expires]         DATETIME       DEFAULT (getdate()) NOT NULL
);*/

