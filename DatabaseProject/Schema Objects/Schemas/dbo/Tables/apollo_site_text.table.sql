CREATE TABLE [dbo].[apollo_site_text] (
    [ID]         INT     IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Text]       NTEXT   NOT NULL,
    [LanguageID] TINYINT NOT NULL
);



