CREATE TABLE [dbo].[Channels] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [ShortDescription] NVARCHAR (255) NOT NULL,
    [UrlIdentifier]    VARCHAR (100)  NOT NULL,
    [SiteID]           INT            NOT NULL
);



