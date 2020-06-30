IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetContentImageAssociationsCount')
	BEGIN
		PRINT 'Dropping Procedure GetContentImageAssociationsCount'
		DROP Procedure GetContentImageAssociationsCount
	END
GO

PRINT 'Creating Procedure GetContentImageAssociationsCount'
GO

CREATE Procedure dbo.GetContentImageAssociationsCount
	@ID bigint
AS
	SELECT 
		COUNT(0)
		FROM
		apollo_content_images
		WHERE
		ImageID = @ID
GO

GRANT EXEC ON GetContentImageAssociationsCount TO PUBLIC
GO