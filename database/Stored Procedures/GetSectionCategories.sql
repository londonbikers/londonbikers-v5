IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSectionCategories')
	BEGIN
		PRINT 'Dropping Procedure GetSectionCategories'
		DROP Procedure GetSectionCategories
	END
GO

PRINT 'Creating Procedure GetSectionCategories'
GO

CREATE Procedure dbo.GetSectionCategories
	@SectionID int
AS

/******************************************************************************
**		Desc: Retrieves the category data for all those relating to a specific
**			  sections.
**
**		Auth: Jay Adair
**		Date: 13.06.06
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
	ParentSectionID = @SectionID
	ORDER BY
	DisplayOrder
GO

GRANT EXEC ON GetSectionCategories TO PUBLIC
GO