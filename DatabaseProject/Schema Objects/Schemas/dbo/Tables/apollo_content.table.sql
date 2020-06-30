CREATE TABLE [dbo].[apollo_content] (
    [ID]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [f_uid]            UNIQUEIDENTIFIER NULL,
    [f_title]          VARCHAR (512)    NOT NULL,
    [f_author]         UNIQUEIDENTIFIER NOT NULL,
    [f_creation_date]  DATETIME         NOT NULL,
    [f_publish_date]   DATETIME         NOT NULL,
    [f_lead_statement] VARCHAR (512)    NULL,
    [f_abstract]       TEXT             NULL,
    [f_body]           TEXT             NOT NULL,
    [f_status]         VARCHAR (20)     NOT NULL,
    [Type]             TINYINT          NOT NULL,
    [Tags]             TEXT             NULL,
    [Views]            BIGINT           NOT NULL
);



