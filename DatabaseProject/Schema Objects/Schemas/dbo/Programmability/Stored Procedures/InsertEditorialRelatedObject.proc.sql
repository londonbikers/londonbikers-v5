
-------------------------------------------------------------------

CREATE Procedure [dbo].[InsertEditorialRelatedObject]
	@ObjectID bigint,
	@RelatedObjectID bigint
AS
	SET NOCOUNT ON
	INSERT INTO
		apollo_related_objects
		(
			ObjectAID,
			ObjectBID
		)
		VALUES
		(
			@ObjectID,
			@RelatedObjectID
		)
