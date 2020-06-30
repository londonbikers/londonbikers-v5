
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetSiteGalleryCategories]
	@SiteID int
AS
	SELECT
		ID
		FROM
		apollo_gallery_categories
		WHERE
		f_type = 0 AND
		f_active = 1 AND
		ParentSiteID = @SiteID
		ORDER BY
		f_name
