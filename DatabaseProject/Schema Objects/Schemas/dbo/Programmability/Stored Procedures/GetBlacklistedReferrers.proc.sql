CREATE Procedure [dbo].[GetBlacklistedReferrers]
AS

/******************************************************************************
**		Name: GetBlacklistedReferrers
**		Desc: Collects a list of all the blacklisted referrers.
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
SELECT
	Url
	FROM
	BlacklistedReferrers
