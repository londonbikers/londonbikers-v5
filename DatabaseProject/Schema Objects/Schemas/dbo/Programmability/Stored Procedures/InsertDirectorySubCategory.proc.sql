
-------------------------------------------------------------------

CREATE Procedure [dbo].[InsertDirectorySubCategory]
	@CategoryID bigint,
	@SubCategoryID bigint
AS
	SET NOCOUNT ON
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = @CategoryID
		WHERE
		ID = @SubCategoryID
