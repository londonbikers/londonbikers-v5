IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSites')
	BEGIN
		PRINT 'Dropping Procedure GetSites'
		DROP Procedure GetSites
	END
GO

PRINT 'Creating Procedure GetSites'
GO

CREATE Procedure dbo.GetSites
AS

/******************************************************************************
**		Desc: Retrieves the ID's for all the site's in the system.
**
**		Auth: Jay Adair
**		Date: 16.10.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	ID
	FROM
	Sites
	ORDER BY [Name]
GO

GRANT EXEC ON dbo.GetSites TO PUBLIC
GO