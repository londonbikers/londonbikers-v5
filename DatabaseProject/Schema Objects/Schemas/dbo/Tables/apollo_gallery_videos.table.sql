CREATE TABLE [dbo].[apollo_gallery_videos] (
    [f_uid]                UNIQUEIDENTIFIER NOT NULL,
    [f_name]               VARCHAR (256)    NULL,
    [f_comment]            TEXT             NULL,
    [f_capture_date]       DATETIME         NULL,
    [f_creation_date]      DATETIME         NOT NULL,
    [f_filename]           VARCHAR (256)    NOT NULL,
    [f_thumbnail_filename] VARCHAR (256)    NULL,
    [f_codec]              TINYINT          NOT NULL,
    [f_views]              INT              NULL
);



