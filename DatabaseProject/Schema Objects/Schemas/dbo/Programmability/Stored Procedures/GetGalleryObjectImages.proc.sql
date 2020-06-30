CREATE PROCEDURE [dbo].[GetGalleryObjectImages]
	(
		@uid uniqueidentifier
	)
	AS
	SELECT
		i.f_uid as [UID],
		i.f_base_url as [BaseUrl],
		i.f_creation_date as [CreationDate],
		i.f_capture_date as [CaptureDate],
		i.f_name as [Name],
		i.f_comment as [Comment],
		i.f_credit as [Credit],
		i.f_views as [Views],
		i.f_custom_thumbnail_filename as [Images.CustomThumbnail],
		i.f_800_filename as [Images.EightHundred],
		i.f_1024_filename as [Images.OneThousandAntTwentyFour],
		i.f_1280_filename as [Images.TwelveEighty],
		i.f_1600_filename as [Images.SixteenHundred],
		i.f_thumbnail_filename as [Images.Thumbnail]
	FROM
		apollo_gallery_images i
		INNER JOIN
		apollo_gallery_image_relations r ON
		i.f_uid = r.f_image_uid
	WHERE
		r.f_gallery_uid = @uid
	ORDER BY
		f_creation_date
	SET NOCOUNT ON