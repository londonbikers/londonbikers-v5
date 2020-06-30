/*
   Add the Image Type table.
*/
GO
/****** Object:  Table [dbo].[ContentImageTypes]    Script Date: 03/21/2006 20:12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContentImageTypes](
	[ID] [tinyint] NOT NULL,
	[Name] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ContentImageTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

/*
   Add the Image Type values.
*/

INSERT INTO ContentImageTypes (ID, [Name]) VALUES (0, 'Normal')
INSERT INTO ContentImageTypes (ID, [Name]) VALUES (1, 'Cover')
INSERT INTO ContentImageTypes (ID, [Name]) VALUES (2, 'Intro')
INSERT INTO ContentImageTypes (ID, [Name]) VALUES (3, 'SlideShow')

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
ALTER TABLE dbo.apollo_images ADD
	Type tinyint NOT NULL CONSTRAINT DF_apollo_images_Type DEFAULT 0
GO
COMMIT

/*
	Convert f_type values to the new Type column
*/

UPDATE dbo.apollo_images SET Type = 0 WHERE f_type = 'Fullsize'
UPDATE dbo.apollo_images SET Type = 1 WHERE f_type = 'Cover'
UPDATE dbo.apollo_images SET Type = 2 WHERE f_type = 'Intro'

/*
   Drop the f_type column.
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
ALTER TABLE dbo.apollo_images
	DROP COLUMN f_type
GO
COMMIT