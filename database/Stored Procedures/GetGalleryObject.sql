IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGalleryObject')
	BEGIN
		PRINT 'Dropping Procedure GetGalleryObject'
		DROP Procedure GetGalleryObject
	END
GO

PRINT 'Creating Procedure GetGalleryObject'
GO

CREATE Procedure GetGalleryObject
	@ID BigInt
AS
	-- GALLERY DATA
	SELECT
		ID,
		f_title as [Title],
		f_creation_date as [CreationDate],
		f_description as [Description],
		f_type as [Type],
		f_status as [Status],
		f_is_public as [IsPublic]
		FROM
		apollo_galleries
		WHERE
		ID = @ID
		
	-- GALLERY CATEGORIES
	SELECT
		CategoryID
		FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID
		
	-- GALLERY IMAGES
	SELECT
		ID,
		BaseUrl,
		CreationDate,
		CaptureDate,
		[Name],
		Comment,
		Credit,
		Views,
		GalleryID,
		Filename800,
		Filename1024,
		Filename1600,
		ThumbnailFilename
		FROM
		GalleryImages
		WHERE
		GalleryID = @ID
		ORDER BY
		CreationDate
GO

GRANT EXEC ON GetGalleryObject TO PUBLIC
GO