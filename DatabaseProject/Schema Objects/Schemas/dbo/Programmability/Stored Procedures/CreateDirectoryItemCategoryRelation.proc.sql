
-------------------------------------------------------------------

CREATE Procedure [dbo].[CreateDirectoryItemCategoryRelation]
	@ID BIGINT,
	@CategoryID BIGINT
AS
	SET NOCOUNT ON
	INSERT INTO
		DirectoryCategoryItems
		(
			CategoryID,
			ItemID
		)
		VALUES
		(
			@CategoryID,
			@ID
		)
