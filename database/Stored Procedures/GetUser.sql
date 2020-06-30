IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetUser')
	BEGIN
		PRINT 'Dropping Procedure GetUser'
		DROP Procedure GetUser
	END
GO

PRINT 'Creating Procedure GetUser'
GO

CREATE Procedure [dbo].[GetUser]
	@UID uniqueidentifier
AS

/******************************************************************************
**		Name: GetUser
**		Desc: Retrieves a users details.
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
	f_uid = @UID
	
SELECT
	Roles.f_key as [ID],
	Roles.f_role as [Name]
	FROM
	apollo_roles Roles
	INNER JOIN
	apollo_user_roles UserRoles ON Roles.f_key = UserRoles.f_role
	WHERE
	UserRoles.f_user = @UID
GO

GRANT EXEC ON dbo.GetUser TO PUBLIC
GO