CREATE TABLE [dbo].[DirectoryCategories] (
    [ID]                 BIGINT           IDENTITY (1, 1) NOT NULL,
    [UID]                UNIQUEIDENTIFIER NULL,
    [Name]               VARCHAR (256)    NOT NULL,
    [Description]        TEXT             NULL,
    [RequiresMembership] BIT              NOT NULL,
    [Keywords]           TEXT             NULL,
    [ParentCategoryID]   BIGINT           NULL
);



