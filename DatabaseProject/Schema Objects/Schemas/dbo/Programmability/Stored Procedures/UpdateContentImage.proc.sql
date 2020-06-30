
-------------------------------------------------------------------

CREATE Procedure [dbo].[UpdateContentImage]
	@ID bigint,
	@Name varchar(255),
	@Filename varchar(255),
	@Width int,
	@Height int,
	@Type tinyint
AS
	SET NOCOUNT ON
	IF (@ID < 1)
	BEGIN
		INSERT INTO
			apollo_images
			(
				f_name,
				f_filename,
				f_width,
				f_height,
				[Type]
			)
			VALUES
			(
				@Name,
				@Filename,
				@Width,
				@Height,
				@Type
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			apollo_images
			SET
			f_name = @Name,
			f_filename = @Filename,
			f_width = @Width,
			f_height = @Height,
			[Type] = @Type
			WHERE
			ID = @ID
		SELECT @ID
	END
