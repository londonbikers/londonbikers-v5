CREATE PROCEDURE [dbo].[GetRootGalleryCategories]
AS
	SELECT 
		f_uid AS [UID],
		f_name AS [Name],
		f_description AS [Description],
		f_active AS [Active],
		f_owner AS [Owner],
		f_type AS [Type]
	FROM
		apollo_gallery_categories
	WHERE
		f_type = 0
	SET NOCOUNT ON
