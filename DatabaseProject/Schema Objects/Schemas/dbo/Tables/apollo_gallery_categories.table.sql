CREATE TABLE [dbo].[apollo_gallery_categories] (
    [ID]            BIGINT           IDENTITY (1, 1) NOT NULL,
    [f_uid]         UNIQUEIDENTIFIER NULL,
    [f_name]        VARCHAR (100)    NULL,
    [f_owner]       UNIQUEIDENTIFIER NULL,
    [f_description] TEXT             NULL,
    [f_type]        TINYINT          NOT NULL,
    [f_active]      BIT              NOT NULL,
    [ParentSiteID]  INT              NULL
);



