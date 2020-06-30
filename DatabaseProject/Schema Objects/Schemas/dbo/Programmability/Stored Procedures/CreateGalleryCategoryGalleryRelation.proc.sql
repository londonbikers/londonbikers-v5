
-------------------------------------------------------------------

CREATE Procedure [dbo].[CreateGalleryCategoryGalleryRelation]
	@ID bigint,
	@GalleryID bigint
AS
	SET NOCOUNT ON
	INSERT INTO
		apollo_gallery_category_gallery_relations
		(
			CategoryID,
			GalleryID
		)
		VALUES
		(
			@ID,
			@GalleryID
		)
