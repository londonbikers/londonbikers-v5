
-------------------------------------------------------------------

CREATE Procedure [dbo].[DeleteGallery]
	@ID bigint
AS
	SET NOCOUNT ON

	DELETE FROM
		GalleryImages
		WHERE
		GalleryID = @ID
			
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID

	DELETE FROM
		apollo_galleries
		WHERE
		ID = @ID
