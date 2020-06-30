CREATE Procedure [dbo].[GetChannelSections]
	@ChannelID int
AS

/******************************************************************************
**		Desc: Retrieves all the section data for those associated with a
**			  specific channel.
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
	ParentChannelID = @ChannelID
