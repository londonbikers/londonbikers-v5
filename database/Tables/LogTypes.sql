IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'LogTypes')
	BEGIN
		PRINT 'Dropping Table LogTypes'
		DROP Table LogTypes
	END
GO

/******************************************************************************
**		File: 
**		Name: Table_Name
**		Desc: 
**
**		This template can be customized:
**              
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

PRINT 'Creating Table LogTypes'
GO

CREATE TABLE [LogTypes] (
	[LogTypeID] [tinyint] NOT NULL ,
	[Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	CONSTRAINT [PK_LogTypes] PRIMARY KEY  CLUSTERED 
	(
		[LogTypeID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

GRANT SELECT ON LogTypes TO PUBLIC
GO