
-------------------------------------------------------------------

CREATE Procedure [dbo].[DeleteDirectoryItem]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
		
	DELETE FROM
		DirectoryItems
		WHERE
		ID = @ID
