IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateDirectoryCategory')
	BEGIN
		PRINT 'Dropping Procedure UpdateDirectoryCategory'
		DROP Procedure UpdateDirectoryCategory
	END
GO

PRINT 'Creating Procedure UpdateDirectoryCategory'
GO

CREATE Procedure dbo.UpdateDirectoryCategory
	@ID bigint,
	@Name varchar(256),
	@Description text,
	@RequiresMembership bit,
	@Keywords text,
	@ParentCategoryID bigint
AS
	SET NOCOUNT ON
	DECLARE @ReturnID bigint
		
	IF (@ID = 0)
	BEGIN
		INSERT INTO
			DirectoryCategories
			(
				[Name],
				Description,
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
			SET @ReturnID = @@Identity
	END
	ELSE
	BEGIN
		UPDATE
			DirectoryCategories
			SET
			[Name] = @Name,
			Description = @Description,
			RequiresMembership = @RequiresMembership,
			Keywords = @Keywords,
			ParentCategoryID = @ParentCategoryID
			WHERE
			ID = @ID
			SET @ReturnID = @ID
	END	
	SELECT @ReturnID
GO

GRANT EXEC ON dbo.UpdateDirectoryCategory TO PUBLIC
GO