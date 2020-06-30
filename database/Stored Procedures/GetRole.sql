IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetRole')
	BEGIN
		PRINT 'Dropping Procedure GetRole'
		DROP Procedure GetRole
	END
GO

PRINT 'Creating Procedure GetRole'
GO

CREATE Procedure GetRole
	@RoleName varchar(200)
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
	f_key as [ID],
	f_role as [Name]
	FROM
	apollo_roles
	WHERE
	f_role = @RoleName

GO

GRANT EXEC ON GetRole TO PUBLIC
GO