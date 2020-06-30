IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteDocument')
	BEGIN
		DROP Procedure dbo.DeleteDocument
	END
GO

CREATE Procedure dbo.DeleteDocument
(
	@ID INT
)
AS
    DELETE FROM DocumentMappings WHERE DocumentID = @ID
    DELETE FROM apollo_related_objects WHERE ObjectAID = @ID OR ObjectBID = @ID
    DELETE FROM apollo_content_images WHERE ContentID = @ID
    DELETE FROM apollo_content WHERE ID = @ID
GO

GRANT EXEC ON dbo.DeleteDocument TO PUBLIC
GO