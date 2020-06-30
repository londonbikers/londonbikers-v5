CREATE TABLE [dbo].[apollo_images] (
    [ID]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [f_uid]      UNIQUEIDENTIFIER NULL,
    [f_name]     VARCHAR (255)    NOT NULL,
    [f_filename] VARCHAR (255)    NOT NULL,
    [f_width]    INT              NOT NULL,
    [f_height]   INT              NOT NULL,
    [f_created]  DATETIME         NOT NULL,
    [Type]       TINYINT          NOT NULL,
    [Views]      BIGINT           NOT NULL
);



