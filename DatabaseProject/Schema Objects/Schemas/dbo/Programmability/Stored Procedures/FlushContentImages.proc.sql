
-------------------------------------------------------------------

CREATE PROCEDURE [dbo].[FlushContentImages]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_content_images
		WHERE
		ContentID = @ID
