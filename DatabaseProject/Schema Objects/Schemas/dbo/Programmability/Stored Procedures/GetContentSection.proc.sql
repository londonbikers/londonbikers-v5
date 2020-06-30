CREATE Procedure [dbo].[GetContentSection]
	@SectionID int
AS

/******************************************************************************
**		Desc: Retrieves the detail information on a particular Section.
**
**		Auth: Jay Adair
**		Date: 21.08.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	*
	FROM
	Sections
	WHERE
	ID = @SectionID
