
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetDirectoryCategorySubCategories]
	@CategoryID bigint
AS
	SET NOCOUNT ON
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID = @CategoryID
		ORDER BY
		[Name]
