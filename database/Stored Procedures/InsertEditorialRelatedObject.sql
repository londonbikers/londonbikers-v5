IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertEditorialRelatedObject')
	BEGIN
		PRINT 'Dropping Procedure InsertEditorialRelatedObject'
		DROP Procedure InsertEditorialRelatedObject
	END
GO

PRINT 'Creating Procedure InsertEditorialRelatedObject'
GO

CREATE Procedure dbo.InsertEditorialRelatedObject
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
GO

GRANT EXEC ON dbo.InsertEditorialRelatedObject TO PUBLIC
GO