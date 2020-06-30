IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetBlacklistedReferrers')
	BEGIN
		PRINT 'Dropping Procedure GetBlacklistedReferrers'
		DROP Procedure GetBlacklistedReferrers
	END
GO

PRINT 'Creating Procedure GetBlacklistedReferrers'
GO

CREATE Procedure [dbo].GetBlacklistedReferrers
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
GO

GRANT EXEC ON [dbo].GetBlacklistedReferrers TO PUBLIC
GO