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
		londonbikers_v5_forums.dbo.InstantForum_Topics t
		INNER JOIN londonbikers_v5_forums.dbo.InstantASP_Users u ON u.UserID = t.UserID
		WHERE 
		t.DateStamp BETWEEN dateadd(m, -1, getdate()) AND getdate() AND
		u.PrimaryRoleID NOT IN (9,12) -- No Moderators, Administrators
		GROUP BY
		t.UserID,
		u.Username
		ORDER BY
		COUNT(0) DESC
