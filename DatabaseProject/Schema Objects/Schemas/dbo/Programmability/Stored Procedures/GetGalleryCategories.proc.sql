
-------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetGalleryCategories]
(
		@ID bigint
)
AS
	SET NOCOUNT ON
	SELECT
		CategoryID
		FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID
