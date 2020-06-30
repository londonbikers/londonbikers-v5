IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ClearGalleryCategoryGalleries')
	BEGIN
		PRINT 'Dropping Procedure ClearGalleryCategoryGalleries'
		DROP Procedure ClearGalleryCategoryGalleries
	END
GO

PRINT 'Creating Procedure ClearGalleryCategoryGalleries'
GO

CREATE Procedure dbo.ClearGalleryCategoryGalleries
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		CategoryID = @ID
GO

GRANT EXEC ON dbo.ClearGalleryCategoryGalleries TO PUBLIC
GO