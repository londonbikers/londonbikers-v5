IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetUserByForumID')
	BEGIN
		PRINT 'Dropping Procedure GetUserByForumID'
		DROP Procedure GetUserByForumID
	END
GO

PRINT 'Creating Procedure GetUserByForumID'
GO

CREATE Procedure [dbo].[GetUserByForumID]
	@ForumUserID int
AS

/******************************************************************************
**		Desc: Retrieves the details for a specific User object by a forumID associated with their account.
**
**		Auth: Jay Adair
**		Date: 17.12.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
SELECT
	apollo_users.*,
	fu.AvatarURL
	FROM
	apollo_users
	RIGHT OUTER JOIN InstantForum_Users fu ON fu.UserID = apollo_users.ForumUserID
	WHERE ForumUserID = @ForumUserID
	
SELECT
	Roles.f_key as [ID],
	Roles.f_role as [Name]
	FROM
	apollo_roles Roles
	INNER JOIN
	apollo_user_roles UserRoles ON Roles.f_key = UserRoles.f_role
	WHERE
	UserRoles.f_user = (SELECT f_uid FROM apollo_users WHERE ForumUserID = @ForumUserID)

GO

GRANT EXEC ON GetUserByForumID TO PUBLIC
GO