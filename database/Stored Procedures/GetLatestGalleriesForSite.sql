IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetLatestGalleriesForSite')
	BEGIN
		PRINT 'Dropping Procedure GetLatestGalleriesForSite'
		DROP  Procedure GetLatestGalleriesForSite
	END
GO

PRINT 'Creating Procedure GetLatestGalleriesForSite'
GO

CREATE Procedure dbo.GetLatestGalleriesForSite
	@ParentID int
AS
	SELECT
		g.ID
		FROM
		apollo_galleries g
		INNER JOIN apollo_gallery_category_gallery_relations cgr ON cgr.GalleryID = g.ID
		INNER JOIN apollo_gallery_categories gc ON cgr.CategoryID = gc.ID
		WHERE
		g.f_status = 1 AND
		gc.ParentSiteID = @ParentID
		GROUP BY
		g.ID, 
		g.f_creation_date
		ORDER BY
		g.f_creation_date DESC
GO

GRANT EXEC ON dbo.GetLatestGalleriesForSite TO PUBLIC
GO