IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetUserByUsername')
	BEGIN
		PRINT 'Dropping Procedure GetUserByUsername'
		DROP Procedure GetUserByUsername
	END
GO

PRINT 'Creating Procedure GetUserByUsername'
GO

CREATE Procedure [dbo].[GetUserByUsername]
	@username varchar(100)
AS

/******************************************************************************
**		Name: GetUserByUsername
**		Desc: Retrieves a users details by their username.
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
	f_username = @username
	
SELECT
	Roles.f_key as [ID],
	Roles.f_role as [Name]
	FROM
	apollo_roles Roles
	INNER JOIN
	apollo_user_roles UserRoles ON Roles.f_key = UserRoles.f_role
	WHERE
	UserRoles.f_user = (SELECT f_uid FROM apollo_users WHERE f_username = @username)

GO

GRANT EXEC ON dbo.GetUserByUsername TO PUBLIC
GO