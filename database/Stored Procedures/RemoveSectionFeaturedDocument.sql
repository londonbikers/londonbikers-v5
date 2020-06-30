IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'RemoveSectionFeaturedDocument')
	BEGIN
		PRINT 'Dropping Procedure RemoveSectionFeaturedDocument'
		DROP Procedure RemoveSectionFeaturedDocument
	END
GO

PRINT 'Creating Procedure RemoveSectionFeaturedDocument'
GO

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
GO

GRANT EXEC ON RemoveSectionFeaturedDocument TO PUBLIC
GO