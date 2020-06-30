CREATE TABLE [dbo].[DirectoryItems] (
    [ID]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [UID]             UNIQUEIDENTIFIER NULL,
    [Title]           VARCHAR (256)    NOT NULL,
    [Description]     TEXT             NOT NULL,
    [TelephoneNumber] VARCHAR (20)     NULL,
    [Keywords]        TEXT             NULL,
    [Links]           TEXT             NULL,
    [Images]          TEXT             NULL,
    [Rating]          BIGINT           NOT NULL,
    [NumberOfRatings] INT              NOT NULL,
    [Submiter]        UNIQUEIDENTIFIER NOT NULL,
    [Status]          TINYINT          NOT NULL,
    [Created]         DATETIME         NOT NULL,
    [Updated]         DATETIME         NOT NULL,
    [Longitude]       FLOAT            NULL,
    [Latitude]        FLOAT            NULL,
    [Postcode]        VARCHAR (8)      NULL,
    [Views]           BIGINT           NOT NULL
);



