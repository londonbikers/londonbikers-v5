
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetContentImages]
	@ContentID bigint
AS
	SET NOCOUNT ON
	SELECT
		ci.ImageID as "ID",
		ci.f_cover_image as "Cover",
		ci.IntroImage as "Intro"
		FROM
		apollo_content_images ci
		INNER JOIN apollo_images i ON i.ID = ci.ImageID
		WHERE
		ci.ContentID = @ContentID
