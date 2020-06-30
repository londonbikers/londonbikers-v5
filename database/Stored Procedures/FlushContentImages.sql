IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'FlushContentImages')
	BEGIN
		PRINT 'Dropping FlushContentImages'
		DROP Procedure FlushContentImages
	END
GO

PRINT 'Creating Procedure FlushContentImages'
GO

CREATE PROCEDURE [dbo].[FlushContentImages]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_content_images
		WHERE
		ContentID = @ID
GO