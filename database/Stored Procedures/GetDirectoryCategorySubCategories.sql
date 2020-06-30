IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDirectoryCategorySubCategories')
	BEGIN
		PRINT 'Dropping Procedure GetDirectoryCategorySubCategories'
		DROP Procedure GetDirectoryCategorySubCategories
	END
GO

PRINT 'Creating Procedure GetDirectoryCategorySubCategories'
GO

CREATE Procedure dbo.GetDirectoryCategorySubCategories
	@CategoryID bigint
AS
	SET NOCOUNT ON
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID = @CategoryID
		ORDER BY
		[Name]
GO

GRANT EXEC ON dbo.GetDirectoryCategorySubCategories TO PUBLIC
GO