IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGalleryCategories')
	BEGIN
		PRINT 'Dropping Procedure GetGalleryCategories'
		DROP Procedure GetGalleryCategories
	END
GO

PRINT 'Creating Procedure GetGalleryCategories'
GO

CREATE PROCEDURE [dbo].[GetGalleryCategories]
(
		@ID bigint
)
AS
	SET NOCOUNT ON
	SELECT
		CategoryID
		FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID
GO

GRANT EXEC ON GetGalleryCategories TO PUBLIC
GO