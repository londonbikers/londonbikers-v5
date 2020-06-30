IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteComment')
	BEGIN
		DROP Procedure DeleteComment
	END
GO

CREATE Procedure dbo.DeleteComment
(
	@ID bigint
)
AS
	DELETE FROM
		Comments
		WHERE
		ID = @ID
GO

GRANT EXEC ON DeleteComment TO PUBLIC
GO