IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'FlushDirectoryCategoryItems')
	BEGIN
		PRINT 'Dropping Procedure FlushDirectoryCategoryItems'
		DROP Procedure FlushDirectoryCategoryItems
	END
GO

PRINT 'Creating Procedure FlushDirectoryCategoryItems'
GO

CREATE Procedure dbo.FlushDirectoryCategoryItems
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		CategoryID = @ID
GO

GRANT EXEC ON FlushDirectoryCategoryItems TO PUBLIC
GO