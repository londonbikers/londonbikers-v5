IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'DirectoryItems')
	BEGIN
		PRINT 'Dropping Table DirectoryItems'
		DROP Table DirectoryItems
	END
GO

/******************************************************************************
**		File: DirectoryItems.sql
**		Name: DirectoryItems
**		Desc: 
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

PRINT 'Creating Table DirectoryItems'
GO

CREATE TABLE [DirectoryItems] (
	[UID] [uniqueidentifier] NOT NULL ,
	[Title] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[TelephoneNumber] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Keywords] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Links] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Images] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Rating] [bigint] NOT NULL CONSTRAINT [DF_DirectoryItems_Rating] DEFAULT (0),
	[NumberOfRatings] [int] NOT NULL CONSTRAINT [DF_DirectoryItems_NumberOfRatings] DEFAULT (0),
	[Submiter] [uniqueidentifier] NOT NULL ,
	[Status] [tinyint] NOT NULL CONSTRAINT [DF_DirectoryItems_Status] DEFAULT (2),
	[Created] [datetime] NOT NULL CONSTRAINT [DF_DirectoryItems_Created] DEFAULT (getdate()),
	[Updated] [datetime] NOT NULL CONSTRAINT [DF_DirectoryItems_Updated] DEFAULT (getdate()),
	CONSTRAINT [PK_DirectoryItems] PRIMARY KEY  CLUSTERED 
	(
		[UID]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GRANT SELECT ON DirectoryItems TO PUBLIC
GO