IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetRootDirectoryCategories')
	BEGIN
		PRINT 'Dropping Procedure GetRootDirectoryCategories'
		DROP Procedure GetRootDirectoryCategories
	END
GO

PRINT 'Creating Procedure GetRootDirectoryCategories'
GO

CREATE Procedure dbo.GetRootDirectoryCategories
AS
	SET NOCOUNT ON
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID IS NULL
		ORDER BY
		[Name]
GO

GRANT EXEC ON dbo.GetRootDirectoryCategories TO PUBLIC
GO