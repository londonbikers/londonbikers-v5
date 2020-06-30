IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CreateGalleryCategoryGalleryRelation')
	BEGIN
		PRINT 'Dropping Procedure CreateGalleryCategoryGalleryRelation'
		DROP Procedure CreateGalleryCategoryGalleryRelation
	END
GO

PRINT 'Creating Procedure CreateGalleryCategoryGalleryRelation'
GO

CREATE Procedure dbo.CreateGalleryCategoryGalleryRelation
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
GO

GRANT EXEC ON dbo.CreateGalleryCategoryGalleryRelation TO PUBLIC
GO