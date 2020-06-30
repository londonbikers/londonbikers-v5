
-------------------------------------------------------------------

CREATE Procedure [dbo].[FlushDirectoryCategoryItems]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		CategoryID = @ID
