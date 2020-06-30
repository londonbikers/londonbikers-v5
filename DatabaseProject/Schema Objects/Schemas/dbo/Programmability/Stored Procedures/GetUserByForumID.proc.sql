


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
**		10.04.2011	Jay Adair			Removed the forum avatar join as it doesn't exist anymore.
**    
*******************************************************************************/

SET NOCOUNT ON
SELECT
	apollo_users.*
	FROM
	apollo_users
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
