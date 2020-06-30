IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateGalleryVideo')
	BEGIN
		PRINT 'Dropping UpdateGalleryVideo'
		DROP Procedure UpdateGalleryVideo
	END
GO

PRINT 'Creating Procedure UpdateGalleryVideo'
GO

CREATE Procedure UpdateGalleryVideo
	@UID uniqueidentifier,
	@Name varchar(256),
	@Comment text,
	@CreationDate datetime,
	@CaptureDate datetime,
	@Codec tinyint,
	@Filename varchar(256),
	@Thumbnail varchar(256)
AS

/******************************************************************************
**		File: UpdateGalleryVideo.sql
**		Name: UpdateGalleryVideo
**		Desc: 
**
**		Auth: Jay Adair
**		Date: 21.08.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
IF (SELECT COUNT(0) FROM apollo_gallery_videos WHERE f_uid = @UID) > 0
BEGIN
	UPDATE
		apollo_gallery_videos
		SET
		f_name = @Name,
		f_comment = @Comment,
		f_capture_date = @CaptureDate,
		f_codec = @Codec,
		f_filename = @Filename,
		f_thumbnail_filename = @Thumbnail
		WHERE
		f_uid = @UID
END
ELSE
BEGIN
	INSERT INTO apollo_gallery_videos
	(
		f_uid,
		f_name,
		f_comment,
		f_creation_date,
		f_capture_date,
		f_codec,
		f_filename,
		f_thumbnail_filename
	)
	VALUES
	(
		@UID,
		@Name,
		@Comment,
		@CreationDate,
		@CaptureDate,
		@Codec,
		@Filename,
		@Thumbnail
	)
END
GO

GRANT EXEC ON UpdateGalleryVideo TO PUBLIC
GO