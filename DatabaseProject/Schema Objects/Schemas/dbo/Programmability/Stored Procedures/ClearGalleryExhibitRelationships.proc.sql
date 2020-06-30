
-------------------------------------------------------------------

CREATE Procedure [dbo].[ClearGalleryExhibitRelationships]
	@ID bigint
AS
	SET NOCOUNT ON
	UPDATE
		GalleryImages
		SET
		GalleryID = NULL
		WHERE
		GalleryID = @ID
