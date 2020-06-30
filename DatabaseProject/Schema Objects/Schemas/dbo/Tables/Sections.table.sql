CREATE TABLE [dbo].[Sections] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50)  NOT NULL,
    [ShortDescription]  NVARCHAR (255) NOT NULL,
    [UrlIdentifier]     VARCHAR (100)  NULL,
    [ContentTypeID]     TINYINT        NOT NULL,
    [ParentChannelID]   INT            NULL,
    [ParentSiteID]      INT            NULL,
    [FavouriteTags]     TEXT           NULL,
    [Status]            TINYINT        NOT NULL,
    [DefaultDocumentID] BIGINT         NULL
);



