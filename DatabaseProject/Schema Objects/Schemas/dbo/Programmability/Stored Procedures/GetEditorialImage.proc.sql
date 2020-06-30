
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
		[Type],
		Views
		FROM
		apollo_images
		WHERE
		ID = @ID
