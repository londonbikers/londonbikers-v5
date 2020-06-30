IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AddSectionFeaturedDocument')
	BEGIN
		PRINT 'Dropping Procedure AddSectionFeaturedDocument'
		DROP Procedure AddSectionFeaturedDocument
	END
GO

PRINT 'Creating Procedure AddSectionFeaturedDocument'
GO

CREATE Procedure dbo.AddSectionFeaturedDocument
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
GO

GRANT EXEC ON AddSectionFeaturedDocument TO PUBLIC
GO