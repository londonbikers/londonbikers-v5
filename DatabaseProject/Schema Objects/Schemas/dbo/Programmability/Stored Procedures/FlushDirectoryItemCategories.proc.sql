
-------------------------------------------------------------------

CREATE Procedure [dbo].[FlushDirectoryItemCategories]
	@ID BIGINT
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
