IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteDirectoryItem')
	BEGIN
		PRINT 'Dropping DeleteDirectoryItem'
		DROP Procedure DeleteDirectoryItem
	END
GO

PRINT 'Creating Procedure DeleteDirectoryItem'
GO

CREATE Procedure dbo.DeleteDirectoryItem
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
GO

GRANT EXEC ON DeleteDirectoryItem TO PUBLIC
GO