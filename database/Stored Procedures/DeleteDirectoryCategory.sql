IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteDirectoryCategory')
	BEGIN
		PRINT 'Dropping Procedure DeleteDirectoryCategory'
		DROP Procedure DeleteDirectoryCategory
	END
GO

PRINT 'Creating Procedure DeleteDirectoryCategory'
GO

CREATE Procedure dbo.DeleteDirectoryCategory
	@ID bigint
AS
	SET NOCOUNT ON

	-- remove any subcategory associations.
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = null
		WHERE
		ParentCategoryID = @ID

	DELETE FROM
		DirectoryCategoryItems
		WHERE
		CategoryID = @ID
		
	DELETE FROM 
		DirectoryCategories
		WHERE
		ID = @ID
GO

GRANT EXEC ON DeleteDirectoryCategory TO PUBLIC
GO