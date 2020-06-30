IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteBlacklistedReferrer')
	BEGIN
		PRINT 'Dropping Procedure DeleteBlacklistedReferrer'
		DROP Procedure DeleteBlacklistedReferrer
	END
GO

PRINT 'Creating Procedure DeleteBlacklistedReferrer'
GO

CREATE Procedure [dbo].DeleteBlacklistedReferrer
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
GO

GRANT EXEC ON [dbo].DeleteBlacklistedReferrer TO PUBLIC
GO