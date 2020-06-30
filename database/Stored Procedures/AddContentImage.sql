IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AddContentImage')
	BEGIN
		PRINT 'Dropping Procedure AddContentImage'
		DROP Procedure AddContentImage
	END
GO

PRINT 'Creating Procedure AddContentImage'
GO

CREATE PROCEDURE [dbo].[AddContentImage]
	@content bigint,
	@image bigint,
	@coverImage bit,
	@IntroImage bit
AS
	SET NOCOUNT ON
	INSERT INTO
		apollo_content_images
		(
			ContentID,
			ImageID,
			f_cover_image,
			IntroImage
		)
		VALUES
		(
			@content,
			@image,
			@coverImage,
			@IntroImage
		)
GO

GRANT EXEC ON dbo.AddContentImage TO PUBLIC
GO