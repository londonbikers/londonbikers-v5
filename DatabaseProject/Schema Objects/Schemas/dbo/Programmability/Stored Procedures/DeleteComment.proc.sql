
CREATE Procedure dbo.DeleteComment
(
	@ID bigint
)
AS
	DELETE FROM
		Comments
		WHERE
		ID = @ID
