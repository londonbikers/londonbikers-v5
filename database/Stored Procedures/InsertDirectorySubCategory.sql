IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertDirectorySubCategory')
	BEGIN
		PRINT 'Dropping Procedure InsertDirectorySubCategory'
		DROP Procedure InsertDirectorySubCategory
	END
GO

PRINT 'Creating Procedure InsertDirectorySubCategory'
GO

CREATE Procedure dbo.InsertDirectorySubCategory
	@CategoryID bigint,
	@SubCategoryID bigint
AS
	SET NOCOUNT ON
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = @CategoryID
		WHERE
		ID = @SubCategoryID
GO

GRANT EXEC ON dbo.InsertDirectorySubCategory TO PUBLIC
GO