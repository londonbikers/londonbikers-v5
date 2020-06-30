IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ClearDocumentMappings')
	BEGIN
		PRINT 'Dropping Procedure ClearDocumentMappings'
		DROP Procedure ClearDocumentMappings
	END
GO

PRINT 'Creating Procedure ClearDocumentMappings'
GO

CREATE Procedure dbo.ClearDocumentMappings
	@DocumentID bigint
AS
	DELETE FROM
		DocumentMappings
		WHERE
		DocumentID = @DocumentID
GO

GRANT EXEC ON dbo.ClearDocumentMappings TO PUBLIC
GO