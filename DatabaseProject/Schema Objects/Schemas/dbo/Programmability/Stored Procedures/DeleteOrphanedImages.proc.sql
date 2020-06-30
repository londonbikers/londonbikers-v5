
-------------------------------------------------------------------

CREATE Procedure [dbo].[DeleteOrphanedImages]
AS
	SET NOCOUNT ON
	DELETE FROM
		GalleryImages
		WHERE
		GalleryID IS NULL
