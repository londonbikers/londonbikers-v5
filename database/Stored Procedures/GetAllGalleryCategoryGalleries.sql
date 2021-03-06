IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetAllGalleryCategoryGalleries')
	BEGIN
		PRINT 'Dropping Procedure GetAllGalleryCategoryGalleries'
		DROP Procedure GetAllGalleryCategoryGalleries
	END
GO

PRINT 'Creating Procedure GetAllGalleryCategoryGalleries'
GO

CREATE Procedure dbo.GetAllGalleryCategoryGalleries
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		cgr.GalleryID as [ID]
		FROM
		apollo_gallery_category_gallery_relations cgr
		INNER JOIN apollo_galleries g on g.ID = cgr.GalleryID
		WHERE
		cgr.CategoryID = @ID
		ORDER BY
		g.f_creation_date desc
GO

GRANT EXEC ON GetAllGalleryCategoryGalleries TO PUBLIC
GO