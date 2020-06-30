
CREATE Procedure [dbo].[MarkContentViewed]
(
	@ContentID BIGINT,
	@ContentTypeID TINYINT
)
AS
	SET NOCOUNT ON
	IF (@ContentTypeID = 1)
	BEGIN
		UPDATE apollo_content SET [Views] = [Views] + 1 WHERE ID = @ContentID
	END
	ELSE IF (@ContentTypeID = 2)
	BEGIN
		UPDATE apollo_images SET [Views] = [Views] + 1 WHERE ID = @ContentID
	END
	ELSE IF (@ContentTypeID = 4)
	BEGIN
		UPDATE GalleryImages SET [Views] = [Views] + 1 WHERE ID = @ContentID
	END
	ELSE IF (@ContentTypeID = 9)
	BEGIN
		UPDATE DirectoryItems SET [Views] = [Views] + 1 WHERE ID = @ContentID
	END
