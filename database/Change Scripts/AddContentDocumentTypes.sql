 /*
   Add the DocumentType table.
*/
GO
/****** Object:  Table [dbo].[ContentTypes]    Script Date: 04/06/2006 16:35:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContentTypes](
	[ID] [tinyint] NOT NULL,
	[Name] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ContentTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

/*
   Add the DocumentType values.
*/

INSERT INTO ContentTypes (ID, [Name]) VALUES (0, 'News')
INSERT INTO ContentTypes (ID, [Name]) VALUES (1, 'Article')
INSERT INTO ContentTypes (ID, [Name]) VALUES (2, 'Generic')

/*
   Add the Type column.
*/

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_content ADD
	Type tinyint NOT NULL CONSTRAINT DF_apollo_document_Type DEFAULT 0
GO
COMMIT

/*
	Convert f_content_type values to the new Type column
*/

UPDATE dbo.apollo_content SET Type = 0 WHERE f_content_type = 'News'
UPDATE dbo.apollo_content SET Type = 1 WHERE f_content_type = 'Article'

/*
   Drop the f_content_type column.
*/

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_content
	DROP COLUMN f_content_type
GO
COMMIT