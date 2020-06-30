IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetComment')
	BEGIN
		DROP Procedure GetComment
	END
GO

CREATE Procedure dbo.GetComment
(
	@CommentID bigint
)
AS
	SELECT
		*
		FROM
		Comments
		WHERE
		ID = @CommentID
GO

GRANT EXEC ON GetComment TO PUBLIC
GO