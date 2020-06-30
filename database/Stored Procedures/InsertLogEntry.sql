IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertLogEntry')
	BEGIN
		PRINT 'Dropping Procedure InsertLogEntry'
		DROP Procedure InsertLogEntry
	END
GO

PRINT 'Creating Procedure InsertLogEntry'
GO

CREATE Procedure InsertLogEntry
	@LogTypeID int,
	@Message text,
	@StackTrace text = null,
	@Context text = null
AS

/******************************************************************************
**		File: 
**		Name: Stored_Procedure_Name
**		Desc: 
**
**		This template can be customized:
**              
**		Return values:
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**     ----------							-----------
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------		--------				-------------------------------------------
**    
*******************************************************************************/

INSERT INTO
	Logs
	(
		Type,
		Message,
		StackTrace,
		Context
	)
	VALUES
	(
		@LogTypeID,
		@Message,
		@StackTrace,
		@Context
	)

GO

GRANT EXEC ON InsertLogEntry TO PUBLIC
GO