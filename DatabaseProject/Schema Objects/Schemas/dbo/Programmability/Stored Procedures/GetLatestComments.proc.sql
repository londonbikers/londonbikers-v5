
CREATE Procedure dbo.GetLatestComments
(
	@MaxComments int
)
AS
	SELECT
		TOP (@MaxComments)
		ID
		FROM
		Comments
		ORDER BY
		Created DESC
