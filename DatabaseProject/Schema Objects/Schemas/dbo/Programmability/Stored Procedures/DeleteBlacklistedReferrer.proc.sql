CREATE Procedure [dbo].[DeleteBlacklistedReferrer]
	@Url varchar(255)
AS

/******************************************************************************
**		Name: DeleteBlacklistedReferrer
**		Desc: Deletes a specific referrer from the database.
**
**		Auth: Jay Adair
**		Date: 16.11.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
DELETE FROM
	BlacklistedReferrers
	WHERE
	Url = @Url
