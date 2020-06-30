IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'DirectoryCategoryItems')
	BEGIN
		PRINT 'Dropping Table DirectoryCategoryItems'
		DROP Table DirectoryCategoryItems
	END
GO

/******************************************************************************
**		File: DirectoryCategoryItems 
**		Name: DirectoryCategoryItems.sql
**		Desc: Stores the associations between categories and items.
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

PRINT 'Creating Table DirectoryCategoryItems'
GO

CREATE TABLE [DirectoryCategoryItems] (
	[CategoryUID] [uniqueidentifier] NOT NULL ,
	[ItemUID] [uniqueidentifier] NOT NULL ,
	CONSTRAINT [PK_DirectoryCategoryItems] PRIMARY KEY  CLUSTERED 
	(
		[CategoryUID],
		[ItemUID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO


GRANT SELECT ON DirectoryCategoryItems TO PUBLIC
GO