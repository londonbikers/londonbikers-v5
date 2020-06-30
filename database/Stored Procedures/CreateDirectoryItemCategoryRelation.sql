IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CreateDirectoryItemCategoryRelation')
	BEGIN
		PRINT 'Dropping Procedure CreateDirectoryItemCategoryRelation'
		DROP Procedure CreateDirectoryItemCategoryRelation
	END
GO

PRINT 'Creating Procedure CreateDirectoryItemCategoryRelation'
GO

CREATE Procedure dbo.CreateDirectoryItemCategoryRelation
	@ID BIGINT,
	@CategoryID BIGINT
AS
	SET NOCOUNT ON
	INSERT INTO
		DirectoryCategoryItems
		(
			CategoryID,
			ItemID
		)
		VALUES
		(
			@CategoryID,
			@ID
		)
GO

GRANT EXEC ON dbo.CreateDirectoryItemCategoryRelation TO PUBLIC
GO