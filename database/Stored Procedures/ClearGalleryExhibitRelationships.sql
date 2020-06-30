IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ClearGalleryExhibitRelationships')
	BEGIN
		PRINT 'Dropping Procedure ClearGalleryExhibitRelationships'
		DROP Procedure ClearGalleryExhibitRelationships
	END
GO

PRINT 'Creating Procedure ClearGalleryExhibitRelationships'
GO

CREATE Procedure dbo.ClearGalleryExhibitRelationships
	@ID bigint
AS
	SET NOCOUNT ON
	UPDATE
		GalleryImages
		SET
		GalleryID = NULL
		WHERE
		GalleryID = @ID
GO

GRANT EXEC ON dbo.ClearGalleryExhibitRelationships TO PUBLIC
GO