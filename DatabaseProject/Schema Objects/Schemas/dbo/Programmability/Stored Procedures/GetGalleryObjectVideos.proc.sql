CREATE PROCEDURE [dbo].[GetGalleryObjectVideos]
	(
		@uid uniqueidentifier
	)
	AS
	SELECT
		v.f_uid as [UID],
		v.f_creation_date as [CreationDate],
		v.f_capture_date as [CaptureDate],
		v.f_codec as [Codec],
		v.f_name as [Name],
		v.f_comment as [Comment],
		v.f_filename as [Filename],
		v.f_thumbnail_filename as [Thumbnail]
	FROM
		apollo_gallery_videos v
		INNER JOIN
		apollo_gallery_video_relations r ON
		v.f_uid = r.f_video_uid
	WHERE
		r.f_gallery_uid = @uid
	SET NOCOUNT ON
