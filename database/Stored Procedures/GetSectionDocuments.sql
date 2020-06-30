IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSectionDocuments')
	BEGIN
		PRINT 'Dropping Procedure GetSectionDocuments'
		DROP Procedure GetSectionDocuments
	END
GO

PRINT 'Creating Procedure GetSectionDocuments'
GO

CREATE Procedure dbo.GetSectionDocuments
	@SectionID int
AS

/******************************************************************************
**		Desc: Retrieves the ID's for the documents associated with a section.
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
	DocumentID
	FROM
	DocumentMappings
	WHERE
	ParentSectionID = @SectionID
GO

GRANT EXEC ON dbo.GetSectionDocuments TO PUBLIC
GO