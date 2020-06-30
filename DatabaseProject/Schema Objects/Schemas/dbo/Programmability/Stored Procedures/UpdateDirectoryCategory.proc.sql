
-------------------------------------------------------------------

CREATE Procedure [dbo].[UpdateDirectoryCategory]
	@ID bigint,
	@Name varchar(256),
	@Description text,
	@RequiresMembership bit,
	@Keywords text,
	@ParentCategoryID bigint
AS
	SET NOCOUNT ON
	IF (@ID = 0)
	BEGIN
		INSERT INTO
			DirectoryCategories
			(
				[Name],
				[Description],
				RequiresMembership,
				Keywords,
				ParentCategoryID
			)
			VALUES
			(
				@Name,
				@Description,
				@RequiresMembership,
				@Keywords,
				@ParentCategoryID
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			DirectoryCategories
			SET
			[Name] = @Name,
			[Description] = @Description,
			RequiresMembership = @RequiresMembership,
			Keywords = @Keywords,
			ParentCategoryID = @ParentCategoryID
			WHERE
			ID = @ID
		SELECT @ID
	END
