IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGalleryImageParentID')
	BEGIN
		DROP Procedure GetGalleryImageParentID
	END
GO

CREATE Procedure dbo.GetGalleryImageParentID
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
GO

GRANT EXEC ON GetGalleryImageParentID TO PUBLIC
GO