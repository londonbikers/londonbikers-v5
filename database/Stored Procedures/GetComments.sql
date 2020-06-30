IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetComments')
	BEGIN
		DROP Procedure GetComments
	END
GO

CREATE Procedure dbo.GetComments
(
	@OwnerID bigint,
	@OwnerType tinyint
)
AS
	SELECT
		ID
		FROM
		Comments
		WHERE
		OwnerID = @OwnerID AND 
		OwnerType = @OwnerType
		ORDER BY
		Created ASC
GO

GRANT EXEC ON GetComments TO PUBLIC
GO