
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetDocumentSections]
	@DocumentID bigint
AS
	SELECT
		ParentSectionID
		FROM
		DocumentMappings
		WHERE
		DocumentID = @DocumentID
