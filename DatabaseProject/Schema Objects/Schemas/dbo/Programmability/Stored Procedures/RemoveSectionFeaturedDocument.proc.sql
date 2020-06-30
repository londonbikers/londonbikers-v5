
CREATE Procedure dbo.RemoveSectionFeaturedDocument
	@SectionID int,
	@DocumentID bigint
AS
	UPDATE
		DocumentMappings
		SET
		IsFeaturedDocument = 0
		WHERE
		ParentSectionID = @SectionID AND
		DocumentID = @DocumentID
