CREATE TABLE [dbo].[apollo_keywords] (
    [f_uid]        UNIQUEIDENTIFIER NOT NULL,
    [f_name]       VARCHAR (256)    NOT NULL,
    [f_url]        VARCHAR (512)    NULL,
    [f_definition] TEXT             NULL,
    [f_author]     UNIQUEIDENTIFIER NOT NULL
);



