IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'DirectoryCategories')
	BEGIN
		PRINT 'Dropping Table DirectoryCategories'
		DROP Table DirectoryCategories
	END
GO

/******************************************************************************
**		File: DirectoryCategories.sql
**		Name: DirectoryCategories
**		Desc: Represents the Categories of the Directory system.
**
**		Auth: Jay Adair
**		Date: 31.08.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

PRINT 'Creating Table DirectoryCategories'
GO

CREATE TABLE [DirectoryCategories] (
	[UID] [uniqueidentifier] NOT NULL ,
	[Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[RequiresMembership] [bit] NOT NULL CONSTRAINT [DF_DirectoryCategories_RequiresMembership] DEFAULT (0),
	[Keywords] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	CONSTRAINT [PK_DirectoryCategories] PRIMARY KEY  CLUSTERED 
	(
		[UID]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GRANT SELECT ON DirectoryCategories TO PUBLIC
GO