IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDirectoryCategory')
	BEGIN
		PRINT 'Dropping Procedure GetDirectoryCategory'
		DROP Procedure GetDirectoryCategory
	END
GO

PRINT 'Creating Procedure GetDirectoryCategory'
GO

CREATE Procedure dbo.GetDirectoryCategory
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		[Name],
		[Description],
		RequiresMembership,
		Keywords,
		ParentCategoryID
		FROM
		DirectoryCategories
		WHERE
		ID = @ID
GO

GRANT EXEC ON GetDirectoryCategory TO PUBLIC
GO