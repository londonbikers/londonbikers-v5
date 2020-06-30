IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteContentImage')
	BEGIN
		PRINT 'Dropping Procedure DeleteContentImage'
		DROP Procedure DeleteContentImage
	END
GO

PRINT 'Creating Procedure DeleteContentImage'
GO

CREATE Procedure dbo.DeleteContentImage
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_content_images
		WHERE
		ImageID = @ID

	DELETE FROM
		apollo_images
		WHERE
		ID = @ID
GO

GRANT EXEC ON DeleteContentImage TO PUBLIC
GO