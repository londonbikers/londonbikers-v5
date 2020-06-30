IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'IncrementImageViewCount')
	BEGIN
		PRINT 'Dropping Procedure IncrementImageViewCount'
		DROP Procedure IncrementImageViewCount
	END
GO

PRINT 'Creating Procedure IncrementImageViewCount'
GO

CREATE Procedure dbo.IncrementImageViewCount
	@ID bigint
AS
	SET NOCOUNT ON
	UPDATE
		GalleryImages
		SET
		Views = Views + 1
		WHERE
		ID = @ID
GO

GRANT EXEC ON IncrementImageViewCount TO PUBLIC
GO