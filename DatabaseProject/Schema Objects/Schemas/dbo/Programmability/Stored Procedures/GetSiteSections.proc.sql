CREATE Procedure [dbo].[GetSiteSections]
	@SiteID int
AS

/******************************************************************************
**		Desc: Retrieves all the section data for those associated with a
**			  specific site.
**
**		Auth: Jay Adair
**		Date: 13.06.06
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
	Sections
	WHERE
	ParentSiteID = @SiteID AND
	ParentChannelID is null
