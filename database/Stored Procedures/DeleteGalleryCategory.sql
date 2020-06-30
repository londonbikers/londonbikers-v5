IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteGalleryCategory')
	BEGIN
		PRINT 'Dropping Procedure DeleteGalleryCategory'
		DROP Procedure DeleteGalleryCategory
	END
GO

PRINT 'Creating Procedure DeleteGalleryCategory'
GO

CREATE PROCEDURE [dbo].[DeleteGalleryCategory]
	(
		@ID bigint
	)
AS
	DELETE FROM
		apollo_gallery_categories
		WHERE
		ID = @ID
		
	-- this should surfice, Apollo will ensure all relations underneath are severed before hand.
		
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		CategoryID = @ID

	SET NOCOUNT ON
	RETURN
GO

GRANT EXEC ON DeleteGalleryCategory TO PUBLIC
GO