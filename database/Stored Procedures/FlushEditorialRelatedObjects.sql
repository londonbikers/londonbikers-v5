IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'FlushEditorialRelatedObjects')
	BEGIN
		PRINT 'Dropping Procedure FlushEditorialRelatedObjects'
		DROP Procedure FlushEditorialRelatedObjects
	END
GO

PRINT 'Creating Procedure FlushEditorialRelatedObjects'
GO

CREATE Procedure dbo.FlushEditorialRelatedObjects
	@ObjectID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_related_objects
		WHERE
		ObjectAID = @ObjectID
GO

GRANT EXEC ON dbo.FlushEditorialRelatedObjects TO PUBLIC
GO