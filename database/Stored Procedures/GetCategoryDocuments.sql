IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetCategoryDocuments')
	BEGIN
		PRINT 'Dropping Procedure GetCategoryDocuments'
		DROP Procedure GetCategoryDocuments
	END
GO

PRINT 'Creating Procedure GetCategoryDocuments'
GO

CREATE Procedure dbo.GetCategoryDocuments
	@CategoryID int
AS

/******************************************************************************
**		Desc: Retrieves the ID's for the documents associated with a category.
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
	INNER JOIN apollo_content ON apollo_content.f_uid = DocumentMappings.DocumentID
	WHERE
	ParentCategoryID = @CategoryID AND
	apollo_content.f_status = 'Published'
	ORDER BY
	apollo_content.f_creation_date DESC
GO

GRANT EXEC ON dbo.GetCategoryDocuments TO PUBLIC
GO