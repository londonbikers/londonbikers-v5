IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteGallery')
	BEGIN
		PRINT 'Dropping Procedure DeleteGallery'
		DROP Procedure DeleteGallery
	END
GO

PRINT 'Creating Procedure DeleteGallery'
GO

CREATE Procedure dbo.DeleteGallery
	@ID bigint
AS
	SET NOCOUNT ON

	DELETE FROM
		GalleryImages
		WHERE
		GalleryID = @ID
			
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID

	DELETE FROM
		apollo_galleries
		WHERE
		ID = @ID
GO

GRANT EXEC ON DeleteGallery TO PUBLIC
GO