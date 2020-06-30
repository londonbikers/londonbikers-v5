IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetLatestComments')
	BEGIN
		DROP Procedure GetLatestComments
	END
GO

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
GO

GRANT EXEC ON GetLatestComments TO PUBLIC
GO