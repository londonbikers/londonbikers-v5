IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetLatestReportedComments')
	BEGIN
		DROP Procedure GetLatestReportedComments
	END
GO

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
GO

GRANT EXEC ON GetLatestReportedComments TO PUBLIC
GO