
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
