IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDocumentSections')
	BEGIN
		PRINT 'Dropping Procedure GetDocumentSections'
		DROP Procedure GetDocumentSections
	END
GO

PRINT 'Creating Procedure GetDocumentSections'
GO

CREATE Procedure dbo.GetDocumentSections
	@DocumentID bigint
AS
	SELECT
		ParentSectionID
		FROM
		DocumentMappings
		WHERE
		DocumentID = @DocumentID
GO

GRANT EXEC ON dbo.GetDocumentSections TO PUBLIC
GO