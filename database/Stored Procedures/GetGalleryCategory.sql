IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGalleryCategory')
	BEGIN
		PRINT 'Dropping Procedure GetGalleryCategory'
		DROP Procedure GetGalleryCategory
	END
GO

PRINT 'Creating Procedure GetGalleryCategory'
GO

CREATE Procedure dbo.GetGalleryCategory
	@ID bigint
AS
	SELECT 
		ID,
		f_name AS [Name],
		f_description AS [Description],
		f_active AS [Active],
		f_owner AS [Owner],
		f_type AS [Type],
		ParentSiteID
		FROM
		apollo_gallery_categories
		WHERE
		ID = @ID
GO

GRANT EXEC ON dbo.GetGalleryCategory TO PUBLIC
GO