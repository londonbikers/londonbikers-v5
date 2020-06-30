IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetUserStats')
	BEGIN
		PRINT 'Dropping Procedure GetUserStats'
		DROP Procedure GetUserStats
	END
GO

PRINT 'Creating Procedure GetUserStats'
GO

CREATE Procedure dbo.GetUserStats
AS
	SELECT
		(SELECT COUNT(0) FROM apollo_users) AS [TotalUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE Status = 1) AS [ActiveUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE Status = 2) AS [SuspendedUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE Status = 0) AS [DeletedUsers]
GO

GRANT EXEC ON GetUserStats TO PUBLIC
GO