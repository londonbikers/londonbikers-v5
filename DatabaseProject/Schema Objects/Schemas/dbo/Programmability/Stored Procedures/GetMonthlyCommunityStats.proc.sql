CREATE Procedure dbo.GetMonthlyCommunityStats
AS
	SELECT
		(select count(0) from apollo_users where f_created between dateadd(m, -1, getdate()) and getdate()) AS NewUsers,
		(select count(0) from londonbikers_v5_forums.dbo.InstantForum_Topics where DateStamp between dateadd(m, -1, getdate()) and getdate()) AS NewPosts,
		(select count(0) from londonbikers_v5_forums.dbo.InstantForum_PrivateMessages where DateStamp between dateadd(m, -1, getdate()) and getdate()) AS NewPMs