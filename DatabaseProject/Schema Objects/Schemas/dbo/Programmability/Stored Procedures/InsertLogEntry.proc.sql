CREATE Procedure [dbo].[InsertLogEntry]
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
