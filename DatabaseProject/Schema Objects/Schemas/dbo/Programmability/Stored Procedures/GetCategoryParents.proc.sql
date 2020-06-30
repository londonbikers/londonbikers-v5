
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetCategoryParents]
	@ID bigint
AS
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID = @ID
