
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetContentImageAssociationsCount]
	@ID bigint
AS
	SELECT 
		COUNT(0)
		FROM
		apollo_content_images
		WHERE
		ImageID = @ID
