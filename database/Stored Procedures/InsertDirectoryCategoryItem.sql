IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertDirectoryCategoryItem')
	BEGIN
		PRINT 'Dropping Procedure InsertDirectoryCategoryItem'
		DROP Procedure InsertDirectoryCategoryItem
	END
GO

PRINT 'Creating Procedure InsertDirectoryCategoryItem'
GO

CREATE Procedure dbo.InsertDirectoryCategoryItem
	@CategoryID bigint,
	@ItemID bigint
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
			@ItemID
		)
GO

GRANT EXEC ON InsertDirectoryCategoryItem TO PUBLIC
GO