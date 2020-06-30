
-------------------------------------------------------------------

CREATE Procedure [dbo].[ClearGalleryCategoryGalleries]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		CategoryID = @ID
