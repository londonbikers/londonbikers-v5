IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AddUserRole')
	BEGIN
		PRINT 'Dropping Procedure AddUserRole'
		DROP Procedure AddUserRole
	END
GO

PRINT 'Creating Procedure AddUserRole'
GO

CREATE Procedure dbo.AddUserRole
	@UID uniqueidentifier,
	@RoleID int
AS

/******************************************************************************
**		Name: AddUserRole
**		Desc: Adds a role to a user account.
**
**		Auth: Jay Adair
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
INSERT INTO
	apollo_user_roles
	(
		f_user,
		f_role
	)
	VALUES
	(
		@UID,
		@RoleID
	)
GO

GRANT EXEC ON dbo.AddUserRole TO PUBLIC
GO