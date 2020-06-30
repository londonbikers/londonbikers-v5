IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetLatestLogEntries')
	BEGIN
		PRINT 'Dropping Procedure GetLatestLogEntries'
		DROP Procedure GetLatestLogEntries
	END
GO

PRINT 'Creating Procedure GetLatestLogEntries'
GO

CREATE Procedure dbo.GetLatestLogEntries
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
GO

GRANT EXEC ON dbo.GetLatestLogEntries TO PUBLIC
GO