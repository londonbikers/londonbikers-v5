IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetCategorySubCategories')
	BEGIN
		PRINT 'Dropping Procedure GetCategorySubCategories'
		DROP Procedure GetCategorySubCategories
	END
GO

PRINT 'Creating Procedure GetCategorySubCategories'
GO

CREATE Procedure dbo.GetCategorySubCategories
	@CategoryID int
AS

/******************************************************************************
**		Desc: Retrieves the ID's of the categories relating to another.
**
**		Auth: Jay Adair
**		Date: 14.06.06
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
	ContentCategories
	WHERE
	ChannelID = @ChannelID
GO

GRANT EXEC ON dbo.GetCategorySubCategories TO PUBLIC
GO