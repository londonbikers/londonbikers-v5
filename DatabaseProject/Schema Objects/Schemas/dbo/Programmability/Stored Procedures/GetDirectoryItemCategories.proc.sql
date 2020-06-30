
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetDirectoryItemCategories]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		CategoryID
		FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
