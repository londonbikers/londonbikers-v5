IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetCategoryParents')
	BEGIN
		PRINT 'Dropping Procedure GetCategoryParents'
		DROP Procedure GetCategoryParents
	END
GO

PRINT 'Creating Procedure GetCategoryParents'
GO

CREATE Procedure dbo.GetCategoryParents
	@ID bigint
AS
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID = @ID
GO

GRANT EXEC ON dbo.GetCategoryParents TO PUBLIC
GO