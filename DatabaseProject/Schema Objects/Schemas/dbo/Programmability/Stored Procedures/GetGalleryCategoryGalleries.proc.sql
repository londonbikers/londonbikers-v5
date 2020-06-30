
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetGalleryCategoryGalleries]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		r.GalleryID as [ID]
		FROM
		apollo_gallery_category_gallery_relations r 
		INNER JOIN apollo_galleries g ON g.ID = r.GalleryID
		WHERE
		r.CategoryID = @ID AND
		g.f_status = 1
		ORDER BY
		g.f_creation_date desc
