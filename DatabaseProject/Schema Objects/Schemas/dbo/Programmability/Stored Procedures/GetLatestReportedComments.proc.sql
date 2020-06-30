
CREATE Procedure dbo.GetLatestReportedComments
(
	@MaxComments int
)
AS
	SELECT
		TOP (@MaxComments)
		ID
		FROM
		Comments
		WHERE
		ReportStatus = 1
		ORDER BY
		Created DESC
