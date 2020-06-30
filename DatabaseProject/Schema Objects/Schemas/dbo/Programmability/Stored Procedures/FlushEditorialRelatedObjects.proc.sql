
-------------------------------------------------------------------

CREATE Procedure [dbo].[FlushEditorialRelatedObjects]
	@ObjectID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_related_objects
		WHERE
		ObjectAID = @ObjectID
