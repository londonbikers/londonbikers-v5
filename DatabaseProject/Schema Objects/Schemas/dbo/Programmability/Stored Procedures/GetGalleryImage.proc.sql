
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetGalleryImage]
	@ID as bigint
AS
	SELECT
		ID,
		BaseUrl,
		CreationDate,
		CaptureDate,
		[Name],
		Comment,
		Credit,
		Filename800,
		Filename1024,
		Filename1600,
		ThumbnailFilename,
		[Views],
		GalleryID
		FROM
		GalleryImages
		WHERE
		ID = @ID
