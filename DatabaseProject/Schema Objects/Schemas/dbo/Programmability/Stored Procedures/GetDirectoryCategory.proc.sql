
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetDirectoryCategory]
	@ID bigint
AS
SET NOCOUNT ON
	SELECT
		[Name],
		[Description],
		RequiresMembership,
		Keywords,
		ParentCategoryID
		FROM
		DirectoryCategories
		WHERE
		ID = @ID
