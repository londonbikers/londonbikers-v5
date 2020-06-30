
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
	Tags,
	Views
	FROM
	apollo_content
	WHERE
	ID = @ID
