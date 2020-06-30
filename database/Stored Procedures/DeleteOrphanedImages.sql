IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteOrphanedImages')
	BEGIN
		PRINT 'Dropping Procedure DeleteOrphanedImages'
		DROP Procedure DeleteOrphanedImages
	END
GO

PRINT 'Creating Procedure DeleteOrphanedImages'
GO

CREATE Procedure dbo.DeleteOrphanedImages
AS
	SET NOCOUNT ON
	DELETE FROM
		GalleryImages
		WHERE
		GalleryID IS NULL
GO

GRANT EXEC ON DeleteOrphanedImages TO PUBLIC
GO