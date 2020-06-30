IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetChannel')
	BEGIN
		PRINT 'Dropping Procedure GetChannel'
		DROP Procedure GetChannel
	END
GO

PRINT 'Creating Procedure GetChannel'
GO

CREATE Procedure dbo.GetChannel
	@ID int
AS

/******************************************************************************
**		Desc: Retrieves the data for a specific channel.
**
**		Auth: Jay Adair
**		Date: 20.06.06
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
	Channels
	WHERE
	ID = @ID
GO

GRANT EXEC ON dbo.GetChannel TO PUBLIC
GO