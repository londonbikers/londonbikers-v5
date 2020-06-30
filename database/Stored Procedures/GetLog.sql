IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetLog')
	BEGIN
		PRINT 'Dropping Procedure GetLog'
		DROP Procedure GetLog
	END

GO

PRINT 'Creating Procedure GetLog'
GO
CREATE Procedure dbo.GetLog
	@ID int
AS

/******************************************************************************
**		Desc: Retrieves the details of a specific log.
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
	*
	FROM
	Logs
	WHERE
	LogID = @ID
GO

GRANT EXEC ON dbo.GetLog TO PUBLIC
GO
