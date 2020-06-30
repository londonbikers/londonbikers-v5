 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetUserByEmail')
	BEGIN
		PRINT 'Dropping Procedure GetUserByEmail'
		DROP Procedure GetUserByEmail
	END
GO

PRINT 'Creating Procedure GetUserByEmail'
GO

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
**    
*******************************************************************************/

SET NOCOUNT ON
SELECT
	apollo_users.*,
	fu.AvatarURL
	FROM
	apollo_users
	RIGHT OUTER JOIN InstantForum_Users fu ON fu.UserID = apollo_users.ForumUserID
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

GO

GRANT EXEC ON dbo.GetUserByEmail TO PUBLIC
GO