IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDirectoryItemCategories')
	BEGIN
		PRINT 'Dropping Procedure GetDirectoryItemCategories'
		DROP Procedure GetDirectoryItemCategories
	END
GO

PRINT 'Creating Procedure GetDirectoryItemCategories'
GO

CREATE Procedure dbo.GetDirectoryItemCategories
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		CategoryID
		FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
GO

GRANT EXEC ON GetDirectoryItemCategories TO PUBLIC
GO