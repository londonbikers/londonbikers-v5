
-------------------------------------------------------------------

CREATE Procedure [dbo].[AddSectionFeaturedDocument]
	@SectionID int,
	@DocumentID bigint
AS
	UPDATE
		DocumentMappings
		SET
		IsFeaturedDocument = 1
		WHERE
		ParentSectionID = @SectionID AND
		DocumentID = @DocumentID
