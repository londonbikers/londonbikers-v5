
-------------------------------------------------------------------

CREATE Procedure [dbo].[FlushCategorySubCategories]
	@CategoryID bigint
AS
	SET NOCOUNT ON
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = null
		WHERE
		ParentCategoryID = @CategoryID
