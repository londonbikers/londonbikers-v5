IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSiteGalleryCategories')
	BEGIN
		PRINT 'Dropping Procedure GetSiteGalleryCategories'
		DROP Procedure GetSiteGalleryCategories
	END
GO

PRINT 'Creating Procedure GetSiteGalleryCategories'
GO

CREATE Procedure dbo.GetSiteGalleryCategories
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
GO

GRANT EXEC ON dbo.GetSiteGalleryCategories TO PUBLIC
GO