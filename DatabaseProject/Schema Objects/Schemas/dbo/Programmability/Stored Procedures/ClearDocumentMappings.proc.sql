
-------------------------------------------------------------------

CREATE Procedure [dbo].[ClearDocumentMappings]
	@DocumentID bigint
AS
	DELETE FROM
		DocumentMappings
		WHERE
		DocumentID = @DocumentID
