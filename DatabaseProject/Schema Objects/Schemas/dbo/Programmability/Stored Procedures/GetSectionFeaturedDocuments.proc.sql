
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetSectionFeaturedDocuments]
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
