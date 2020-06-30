CREATE Procedure [dbo].[GetLatestLogEntries]
	@Maximum int
AS

/******************************************************************************
**		Desc: Retrieves the details for the latest log entries.
**
**		Auth: Jay Adair
**		Date: 16.11.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	TOP (@Maximum)
	*
	FROM
	Logs
	ORDER BY
	LogID DESC
