
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetGalleryImageParentID]
(
	@ImageID bigint
)
AS
	SELECT
		GalleryID
		FROM
		GalleryImages
		WHERE
		ID = @ImageID
