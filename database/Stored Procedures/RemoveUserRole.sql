IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'RemoveUserRole')
	BEGIN
		PRINT 'Dropping Procedure RemoveUserRole'
		DROP Procedure RemoveUserRole
	END
GO

PRINT 'Creating Procedure RemoveUserRole'
GO

CREATE Procedure RemoveUserRole
	@UID uniqueidentifier,
	@RoleID int
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
**		----------						-----------
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

DELETE FROM
	apollo_user_roles
	WHERE
	f_user = @UID
	AND
	f_role = @RoleID

GO

GRANT EXEC ON RemoveUserRole TO PUBLIC
GO