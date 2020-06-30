IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetEditorialRelatedObjects')
	BEGIN
		PRINT 'Dropping Procedure GetEditorialRelatedObjects'
		DROP Procedure GetEditorialRelatedObjects
	END
GO

PRINT 'Creating Procedure GetEditorialRelatedObjects'
GO

CREATE Procedure dbo.GetEditorialRelatedObjects
	@ObjectID bigint
AS
	SET NOCOUNT ON
	SELECT
		ObjectBID as [ID]
		FROM
		apollo_related_objects
		WHERE
		ObjectAID = @ObjectID
GO

GRANT EXEC ON GetEditorialRelatedObjects TO PUBLIC
GO