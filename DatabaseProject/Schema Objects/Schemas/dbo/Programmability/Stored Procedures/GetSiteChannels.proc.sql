CREATE Procedure [dbo].[GetSiteChannels]
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
