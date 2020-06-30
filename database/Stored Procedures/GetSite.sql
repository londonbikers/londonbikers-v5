IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSite')
	BEGIN
		PRINT 'Dropping Procedure GetSite'
		DROP Procedure GetSite
	END
GO

PRINT 'Creating Procedure GetSite'
GO

CREATE Procedure dbo.GetSite
	@ID int
AS

/******************************************************************************
**		Desc: 
**
**		Auth: Jay Adair
**		Date: 11.06.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	[Name],
	URL
	FROM
	Sites
	WHERE
	ID = @ID
GO

GRANT EXEC ON dbo.GetSite TO PUBLIC
GO