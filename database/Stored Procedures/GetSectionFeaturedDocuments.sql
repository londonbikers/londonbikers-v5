IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSectionFeaturedDocuments')
	BEGIN
		PRINT 'Dropping Procedure GetSectionFeaturedDocuments'
		DROP Procedure GetSectionFeaturedDocuments
	END
GO

PRINT 'Creating Procedure GetSectionFeaturedDocuments'
GO

CREATE Procedure dbo.GetSectionFeaturedDocuments
	@SectionID int,
	@MaxResults int
AS
	SELECT
		TOP (@MaxResults)
		DocumentMappings.DocumentID
		FROM
		DocumentMappings 
		INNER JOIN apollo_content c ON c.ID = DocumentMappings.DocumentID
		WHERE
		DocumentMappings.IsFeaturedDocument = 1 AND 
		DocumentMappings.ParentSectionID = @SectionID
		ORDER BY 
		c.f_publish_date DESC
GO

GRANT EXEC ON GetSectionFeaturedDocuments TO PUBLIC
GO