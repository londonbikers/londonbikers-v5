
-------------------------------------------------------------------

CREATE Procedure [dbo].[DeleteDirectoryCategory]
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
