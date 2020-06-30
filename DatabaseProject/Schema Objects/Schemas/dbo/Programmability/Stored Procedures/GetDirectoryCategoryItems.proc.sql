
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetDirectoryCategoryItems]
	@CategoryID bigint
AS
	SELECT
		ID
		FROM
		DirectoryItems DI
		INNER JOIN DirectoryCategoryItems DCI ON DI.ID = DCI.ItemID
		WHERE
		DCI.CategoryID = @CategoryID
		ORDER BY
		Rating DESC,
		Title ASC
