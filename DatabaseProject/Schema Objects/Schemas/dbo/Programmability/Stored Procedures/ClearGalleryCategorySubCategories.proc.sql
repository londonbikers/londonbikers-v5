
CREATE Procedure [dbo].[ClearGalleryCategorySubCategories]
	@UID uniqueidentifier
AS

/******************************************************************************
**		Name: ClearGalleryCategorySubCategories
**
**		Auth: Jay Adair
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
UPDATE
	apollo_gallery_categories
	SET
	f_owner = '{00000000-0000-0000-0000-000000000000}'
	WHERE
	f_owner = @UID
