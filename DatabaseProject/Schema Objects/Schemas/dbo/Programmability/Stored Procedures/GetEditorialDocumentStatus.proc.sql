
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetEditorialDocumentStatus]
	@ID bigint
AS
	SELECT
		f_status
		FROM
		apollo_content
		WHERE
		ID = @ID
