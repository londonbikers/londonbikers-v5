IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DoesUserExist')
	BEGIN
		PRINT 'Dropping DoesUserExist'
		DROP Procedure DoesUserExist
	END
GO

PRINT 'Creating Procedure DoesUserExist'
GO

CREATE Procedure DoesUserExist
	@UID uniqueidentifier
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

	SELECT
		f_username
		FROM
		apollo_users
		WHERE
		f_uid = @uid

GO

GRANT EXEC ON DoesUserExist TO PUBLIC
GO