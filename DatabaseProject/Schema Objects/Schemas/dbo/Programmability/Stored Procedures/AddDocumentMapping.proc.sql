
-------------------------------------------------------------------

CREATE Procedure [dbo].[AddDocumentMapping]
	@DocumentID bigint,
	@SectionID int
AS
	INSERT INTO
		DocumentMappings
		(
			DocumentID,
			ParentSectionID
		)
		VALUES
		(
			@DocumentID,
			@SectionID
		)
