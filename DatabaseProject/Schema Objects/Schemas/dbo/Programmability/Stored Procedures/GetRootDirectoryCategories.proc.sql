
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetRootDirectoryCategories]
AS
	SET NOCOUNT ON
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID IS NULL
		ORDER BY
		[Name]
