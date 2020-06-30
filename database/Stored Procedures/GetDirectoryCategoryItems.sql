IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDirectoryCategoryItems')
	BEGIN
		PRINT 'Dropping Procedure GetDirectoryCategoryItems'
		DROP Procedure GetDirectoryCategoryItems
	END
GO

PRINT 'Creating Procedure GetDirectoryCategoryItems'
GO

CREATE Procedure dbo.GetDirectoryCategoryItems
	@CategoryID bigint
AS
	SELECT
		ID
		FROM
		DirectoryItems DI
		INNER JOIN DirectoryCategoryItems DCI ON DI.ID = DCI.ItemID
		WHERE
		DCI.CategoryID = @CategoryID
		ORDER BY
		Rating DESC,
		Title ASC
GO

GRANT EXEC ON dbo.GetDirectoryCategoryItems TO PUBLIC
GO