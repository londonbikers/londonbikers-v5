
-------------------------------------------------------------------

CREATE Procedure [dbo].[DeleteContentImage]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_content_images
		WHERE
		ImageID = @ID

	DELETE FROM
		apollo_images
		WHERE
		ID = @ID
