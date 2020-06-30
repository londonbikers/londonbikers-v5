CREATE Procedure [dbo].[GetUserRoles]
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
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
		Roles.f_key as [ID],
		Roles.f_role as [Name]
FROM
		apollo_roles Roles
		INNER JOIN
		apollo_user_roles UserRoles ON Roles.f_key = UserRoles.f_role
WHERE
		UserRoles.f_user = @UID
