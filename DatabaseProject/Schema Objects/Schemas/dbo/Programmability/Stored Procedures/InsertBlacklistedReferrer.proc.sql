CREATE Procedure [dbo].[InsertBlacklistedReferrer]
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
