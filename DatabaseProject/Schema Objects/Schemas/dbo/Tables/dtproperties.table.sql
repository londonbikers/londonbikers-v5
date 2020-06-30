﻿CREATE TABLE [dbo].[dtproperties] (
    [id]       INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [objectid] INT            NULL,
    [property] VARCHAR (64)   NOT NULL,
    [value]    VARCHAR (255)  NULL,
    [uvalue]   NVARCHAR (255) NULL,
    [lvalue]   IMAGE          NULL,
    [version]  INT            DEFAULT ((0)) NOT NULL
);



