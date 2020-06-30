IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AddDocumentMapping')
	BEGIN
		PRINT 'Dropping Procedure AddDocumentMapping'
		DROP Procedure AddDocumentMapping
	END
GO

PRINT 'Creating Procedure AddDocumentMapping'
GO

CREATE Procedure dbo.AddDocumentMapping
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
GO

GRANT EXEC ON dbo.AddDocumentMapping TO PUBLIC
GO