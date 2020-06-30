
-----------------------------------------------------------------

CREATE Procedure [dbo].[GetUserStats]
AS
	SELECT
		(SELECT COUNT(0) FROM apollo_users) AS [TotalUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE [Status] = 1) AS [ActiveUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE [Status] = 2) AS [SuspendedUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE [Status] = 0) AS [DeletedUsers]
