IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetEditorialDocument')
	BEGIN
		PRINT 'Dropping Procedure GetEditorialDocument'
		DROP Procedure GetEditorialDocument
	END
GO

PRINT 'Creating Procedure GetEditorialDocument'
GO

CREATE Procedure dbo.GetEditorialDocument
	@ID BigInt
AS

SET NOCOUNT ON
SELECT
	ID,
	f_title AS [Title],
	f_author AS [Author],
	f_creation_date AS [Created],
	f_publish_date AS [Published],
	f_lead_statement AS [LeadStatement],
	f_abstract AS [Abstract],
	f_body AS [Body],
	f_status AS [Status],
	Type,
	Tags
	FROM
	apollo_content
	WHERE
	ID = @ID
GO

GRANT EXEC ON dbo.GetEditorialDocument TO PUBLIC
GO