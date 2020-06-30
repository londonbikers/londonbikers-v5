IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'FlushCategorySubCategories')
	BEGIN
		PRINT 'Dropping FlushCategorySubCategories'
		DROP Procedure FlushCategorySubCategories
	END
GO

PRINT 'Creating Procedure FlushCategorySubCategories'
GO

CREATE Procedure dbo.FlushCategorySubCategories
	@CategoryID bigint
AS
	SET NOCOUNT ON
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = null
		WHERE
		ParentCategoryID = @CategoryID
GO

GRANT EXEC ON FlushCategorySubCategories TO PUBLIC
GO