IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Logs')
	BEGIN
		PRINT 'Dropping Table Logs'
		DROP Table Logs
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

PRINT 'Creating Table Logs'
GO

CREATE TABLE [Logs] (
	[LogID] [int] IDENTITY (1, 1) NOT NULL ,
	[Type] [tinyint] NOT NULL CONSTRAINT [DF_Logs_Type] DEFAULT (1),
	[Message] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[StackTrace] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Context] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[When] [datetime] NOT NULL CONSTRAINT [DF_Logs_When] DEFAULT (getdate()),
	CONSTRAINT [PK_Logs] PRIMARY KEY  CLUSTERED 
	(
		[LogID]
	)  ON [PRIMARY] ,
	CONSTRAINT [FK_Logs_LogTypes] FOREIGN KEY 
	(
		[Type]
	) REFERENCES [LogTypes] (
		[LogTypeID]
	)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GRANT SELECT ON Logs TO PUBLIC
GO