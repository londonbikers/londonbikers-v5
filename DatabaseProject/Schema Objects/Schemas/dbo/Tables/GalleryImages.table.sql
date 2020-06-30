CREATE TABLE [dbo].[GalleryImages] (
    [ID]                      BIGINT           IDENTITY (1, 1) NOT NULL,
    [Uid]                     UNIQUEIDENTIFIER NULL,
    [Name]                    VARCHAR (256)    NULL,
    [Credit]                  VARCHAR (512)    NULL,
    [Comment]                 TEXT             NULL,
    [CaptureDate]             DATETIME         NOT NULL,
    [CreationDate]            DATETIME         NOT NULL,
    [BaseUrl]                 VARCHAR (256)    NULL,
    [Filename800]             VARCHAR (256)    NULL,
    [Filename1024]            VARCHAR (256)    NULL,
    [Filename1280]            VARCHAR (256)    NULL,
    [Filename1600]            VARCHAR (256)    NULL,
    [ThumbnailFilename]       VARCHAR (256)    NULL,
    [CustomThumbnailFilename] VARCHAR (256)    NULL,
    [Views]                   BIGINT           NOT NULL,
    [GalleryID]               BIGINT           NULL
);



