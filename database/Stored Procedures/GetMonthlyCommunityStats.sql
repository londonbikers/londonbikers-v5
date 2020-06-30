IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetMonthlyCommunityStats')
	BEGIN
		DROP Procedure GetMonthlyCommunityStats
	END
GO

CREATE Procedure dbo.GetMonthlyCommunityStats
AS
	SELECT
		(select count(0) from apollo_users where f_created between dateadd(m, -1, getdate()) and getdate()) AS NewUsers,
		(select count(0) from InstantForum_Topics where DateStamp between dateadd(m, -1, getdate()) and getdate()) AS NewPosts,
		(select count(0) from InstantForum_PrivateMessages where DateStamp between dateadd(m, -1, getdate()) and getdate()) AS NewPMs
GO

GRANT EXEC ON GetMonthlyCommunityStats TO PUBLIC
GO