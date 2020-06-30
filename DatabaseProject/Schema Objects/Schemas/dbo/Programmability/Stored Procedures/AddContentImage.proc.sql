
-----------------------------------------------------------------

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
