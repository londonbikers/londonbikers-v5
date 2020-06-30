IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetLatestDirectoryItems')
	BEGIN
		PRINT 'Dropping Procedure GetLatestDirectoryItems'
		DROP Procedure GetLatestDirectoryItems
	END
GO

PRINT 'Creating Procedure GetLatestDirectoryItems'
GO

CREATE Procedure GetLatestDirectoryItems
AS

/******************************************************************************
**		Name: GetLatestDirectoryItems
**		Desc: Returns the top 100 newest items.
**
**		Auth: Jay Adair
**		Date: 25.09.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON

SELECT
	TOP 100
	UID,
	Title,
	Description,
	Created,
	Updated,
	Submiter,
	Status,
	Rating,
	NumberOfRatings,
	Images,
	Links,
	Keywords
	FROM
	DirectoryItems
	ORDER BY
	Created DESC
GO

GRANT EXEC ON GetLatestDirectoryItems TO PUBLIC
GO