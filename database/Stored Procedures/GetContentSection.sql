IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetContentSection')
	BEGIN
		PRINT 'Dropping Procedure GetContentSection'
		DROP Procedure GetContentSection
	END
GO

PRINT 'Creating Procedure GetContentSection'
GO

CREATE Procedure dbo.GetContentSection
	@SectionID int
AS

/******************************************************************************
**		Desc: Retrieves the detail information on a particular Section.
**
**		Auth: Jay Adair
**		Date: 21.08.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	*
	FROM
	Sections
	WHERE
	ID = @SectionID
GO

GRANT EXEC ON dbo.GetContentSection TO PUBLIC
GO