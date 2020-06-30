CREATE TABLE [dbo].[apollo_galleries] (
    [ID]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [f_uid]           UNIQUEIDENTIFIER NULL,
    [f_title]         VARCHAR (512)    NOT NULL,
    [f_description]   TEXT             NULL,
    [f_creation_date] DATETIME         NOT NULL,
    [f_type]          TINYINT          NOT NULL,
    [f_is_public]     BIT              NOT NULL,
    [f_status]        TINYINT          NOT NULL,
    [f_views]         INT              NOT NULL
);



