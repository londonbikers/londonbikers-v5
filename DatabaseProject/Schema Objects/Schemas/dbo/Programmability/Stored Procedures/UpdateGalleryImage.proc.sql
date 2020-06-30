
-------------------------------------------------------------------

CREATE Procedure [dbo].[UpdateGalleryImage]
	@ID bigint,
	@Name varchar(256),
	@Comment text,
	@Credit varchar(512),
	@CreationDate datetime,
	@CaptureDate datetime,
	@BaseUrl varchar(256),
	@800 varchar(256),
	@1024 varchar(256),
	@1600 varchar(256),
	@Thumbnail varchar(256),
	@GalleryID bigint
AS
	IF (@ID > 0)
	BEGIN
		UPDATE
			GalleryImages
			SET
			[Name] = @Name,
			Comment = @Comment,
			Credit = @Credit,
			CaptureDate = @CaptureDate,
			BaseUrl = @BaseUrl,
			Filename800 = @800,
			Filename1024 = @1024,
			Filename1600 = @1600,
			ThumbnailFilename = @Thumbnail,
			GalleryID = @GalleryID
			WHERE
			ID = @ID
		SELECT @ID
	END
	ELSE
	BEGIN
		INSERT INTO
			GalleryImages
			(
				[Name],
				Comment,
				Credit,
				CreationDate,
				CaptureDate,
				BaseUrl,
				Filename800,
				Filename1024,
				Filename1600,
				ThumbnailFilename,
				GalleryID
			)
			VALUES
			(
				@Name,
				@Comment,
				@Credit,
				@CreationDate,
				@CaptureDate,
				@BaseUrl,
				@800,
				@1024,
				@1600,
				@Thumbnail,
				@GalleryID
			)
		SELECT SCOPE_IDENTITY()
	END
