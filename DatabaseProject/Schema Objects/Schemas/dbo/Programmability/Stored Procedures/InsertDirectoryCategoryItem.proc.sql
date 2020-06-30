
-------------------------------------------------------------------

CREATE Procedure [dbo].[InsertDirectoryCategoryItem]
	@CategoryID bigint,
	@ItemID bigint
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
			@ItemID
		)
