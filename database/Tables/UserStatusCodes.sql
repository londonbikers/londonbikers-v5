IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'UserStatusCodes')
	BEGIN
		PRINT 'Dropping Table UserStatusCodes'
		DROP Table UserStatusCodes
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

PRINT 'Creating Table UserStatusCodes'
GO

CREATE TABLE [UserStatusCodes] (
	[ID] [tinyint] NOT NULL ,
	[Description] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	CONSTRAINT [PK_UserStatusCodes] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

GRANT SELECT ON UserStatusCodes TO PUBLIC
GO