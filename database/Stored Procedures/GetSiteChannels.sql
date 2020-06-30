IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSiteChannels')
	BEGIN
		PRINT 'Dropping Procedure GetSiteChannels'
		DROP Procedure GetSiteChannels
	END
GO

PRINT 'Creating Procedure GetSiteChannels'
GO

CREATE Procedure dbo.GetSiteChannels
	@SiteID int
AS

/******************************************************************************
**		Desc: Collects all the channel data for a particular site.
**
**		Auth: Jay Adair
**		Date: 11.06.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	ID
	FROM
	Channels
	WHERE
	SiteID = @SiteID
GO

GRANT EXEC ON dbo.GetSiteChannels TO PUBLIC
GO