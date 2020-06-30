
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
