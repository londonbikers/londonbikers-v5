IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGalleryImage')
	BEGIN
		PRINT 'Dropping Procedure GetGalleryImage'
		DROP Procedure GetGalleryImage
	END
GO

PRINT 'Creating Procedure GetGalleryImage'
GO

CREATE Procedure dbo.GetGalleryImage
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
		Views,
		GalleryID
		FROM
		GalleryImages
		WHERE
		ID = @ID
GO

GRANT EXEC ON GetGalleryImage TO PUBLIC
GO