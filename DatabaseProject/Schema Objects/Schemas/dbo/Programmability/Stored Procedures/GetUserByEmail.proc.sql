﻿
CREATE Procedure [dbo].[GetUserByEmail]
	@email varchar(512)
AS

/******************************************************************************
**		Name: GetUserByEmail
**		Desc: Retrieves a users details by their email address.
**
**		Auth: Jay Adair
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**		02.01.2006	Jay Adair			Added in the user roles query.
**		10.04.2011	Jay Adair			Removed the forum avatar join as it doesn't exist anymore.
**    
*******************************************************************************/

SET NOCOUNT ON
SELECT
	apollo_users.*
	FROM
	apollo_users
	WHERE
	f_email = @email
	
SELECT
	Roles.f_key as [ID],
	Roles.f_role as [Name]
	FROM
	apollo_roles Roles
	INNER JOIN
	apollo_user_roles UserRoles ON Roles.f_key = UserRoles.f_role
	WHERE
	UserRoles.f_user = (SELECT f_uid FROM apollo_users WHERE f_email= @email)
