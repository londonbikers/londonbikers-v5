IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetEditorialImage')
	BEGIN
		PRINT 'Dropping Procedure GetEditorialImage'
		DROP Procedure GetEditorialImage
	END
GO

PRINT 'Creating Procedure GetEditorialImage'
GO

CREATE Procedure dbo.GetEditorialImage
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		ID,
		f_name AS [Name],
		f_filename AS [Filename],
		f_width AS [width],
		f_height AS [Height],
		f_created AS [Created],
		[Type]
		FROM
		apollo_images
		WHERE
		ID = @ID
GO

GRANT EXEC ON dbo.GetEditorialImage TO PUBLIC
GO