IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'FlushDirectoryItemCategories')
	BEGIN
		PRINT 'Dropping Procedure FlushDirectoryItemCategories'
		DROP Procedure FlushDirectoryItemCategories
	END
GO

PRINT 'Creating Procedure FlushDirectoryItemCategories'
GO

CREATE Procedure dbo.FlushDirectoryItemCategories
	@ID BIGINT
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
GO

GRANT EXEC ON FlushDirectoryItemCategories TO PUBLIC
GO