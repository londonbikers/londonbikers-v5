CREATE Procedure [dbo].[RemoveUserRole]
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
