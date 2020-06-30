IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetEditorialDocumentStatus')
	BEGIN
		PRINT 'Dropping Procedure GetEditorialDocumentStatus'
		DROP Procedure GetEditorialDocumentStatus
	END
GO

PRINT 'Creating Procedure GetEditorialDocumentStatus'
GO

CREATE Procedure dbo.GetEditorialDocumentStatus
	@ID bigint
AS
	SELECT
		f_status
		FROM
		apollo_content
		WHERE
		ID = @ID
GO

GRANT EXEC ON dbo.GetEditorialDocumentStatus TO PUBLIC
GO