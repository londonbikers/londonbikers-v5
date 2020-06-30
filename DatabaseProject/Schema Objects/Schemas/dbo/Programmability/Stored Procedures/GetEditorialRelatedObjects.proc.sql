
---------------------------------------------------------------------

CREATE Procedure [dbo].[GetEditorialRelatedObjects]
	@ObjectID bigint
AS
	SET NOCOUNT ON
	SELECT
		ObjectBID as [ID]
		FROM
		apollo_related_objects
		WHERE
		ObjectAID = @ObjectID
