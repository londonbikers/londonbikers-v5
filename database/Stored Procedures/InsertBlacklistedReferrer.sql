IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertBlacklistedReferrer')
	BEGIN
		PRINT 'Dropping Procedure InsertBlacklistedReferrer'
		DROP Procedure InsertBlacklistedReferrer
	END
GO

PRINT 'Creating Procedure InsertBlacklistedReferrer'
GO

CREATE Procedure [dbo].InsertBlacklistedReferrer
	@Url varchar(255)
AS

/******************************************************************************
**		Name: InsertBlacklistedReferrer
**		Desc: Persists a new referrer to the database.
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
INSERT INTO
	BlacklistedReferrers
	(
		Url
	)
	VALUES
	(
		@Url
	)
GO

GRANT EXEC ON [dbo].InsertBlacklistedReferrer TO PUBLIC
GO