
-------------------------------------------------------------------

CREATE Procedure [dbo].[LatestPublicGalleries]
AS
	SELECT
		TOP 50
		ID
		FROM
		apollo_galleries
		WHERE
		f_is_public = 1 AND
		f_status = 1
		ORDER BY
		f_creation_date DESC
