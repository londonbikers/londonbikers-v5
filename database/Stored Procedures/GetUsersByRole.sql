IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetUsersByRole')
	BEGIN
		PRINT 'Dropping Procedure GetUsersByRole'
		DROP Procedure GetUsersByRole
	END
GO

PRINT 'Creating Procedure GetUsersByRole'
GO

CREATE Procedure GetUsersByRole
	@Role varchar(200)
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

SELECT
	apollo_user_roles.f_user as "UID"
	FROM
	apollo_user_roles
	INNER JOIN apollo_roles ON apollo_roles.f_key = apollo_user_roles.f_role
	WHERE
	apollo_roles.f_role = @Role
GO

GRANT EXEC ON GetUsersByRole TO PUBLIC
GO