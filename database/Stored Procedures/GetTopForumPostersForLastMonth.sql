IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetTopForumPostersForLastMonth')
	BEGIN
		DROP Procedure GetTopForumPostersForLastMonth
	END
GO

CREATE Procedure dbo.GetTopForumPostersForLastMonth
(
	@MaxResults int
)
AS
	SELECT 
		TOP (@MaxResults)
		u.Username,
		t.UserID,
		COUNT(0) AS TotalPosts
		FROM 
		InstantForum_Topics t
		INNER JOIN InstantASP_Users u ON u.UserID = t.UserID
		WHERE 
		t.DateStamp BETWEEN dateadd(m, -1, getdate()) AND getdate() AND
		u.PrimaryRoleID NOT IN (9,12) -- No Moderators, Administrators
		GROUP BY
		t.UserID,
		u.Username
		ORDER BY
		COUNT(0) DESC
GO

GRANT EXEC ON GetTopForumPostersForLastMonth TO PUBLIC
GO