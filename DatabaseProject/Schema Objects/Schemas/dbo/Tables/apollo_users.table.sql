CREATE TABLE [dbo].[apollo_users] (
    [f_uid]             UNIQUEIDENTIFIER NOT NULL,
    [f_firstname]       VARCHAR (100)    NULL,
    [f_lastname]        VARCHAR (100)    NULL,
    [f_password]        VARCHAR (50)     NULL,
    [f_email]           VARCHAR (512)    NULL,
    [f_username]        VARCHAR (100)    NOT NULL,
    [f_created]         DATETIME         NULL,
    [Status]            TINYINT          NOT NULL,
    [ForumUserID]       INT              NOT NULL,
    [ReceiveNewsletter] BIT              NOT NULL
);



