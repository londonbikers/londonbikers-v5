IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'if_sp_ShowMiscellaneousTopics')
	BEGIN
		PRINT 'Dropping Procedure if_sp_ShowMiscellaneousTopics'
		DROP Procedure if_sp_ShowMiscellaneousTopics
	END
GO

PRINT 'Creating Procedure if_sp_ShowMiscellaneousTopics'
GO

CREATE Procedure dbo.if_sp_ShowMiscellaneousTopics (
	@Type varchar(50),
	@MemberGroupID int,
	@LastVisit DateTime
) AS

/******************************************************************************
**		Name: if_sp_ShowMiscellaneousTopics
**		Desc: 
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**		18.01.06	Jay Adair			Fixed 24hr case bug.
**    
*******************************************************************************/

DECLARE @Today datetime 
DECLARE @DateToday datetime
IF (@Type = 'active') BEGIN	
	-- gets active posts from past 7 days ordered by groupsortorder and forumsortorder
	SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	AND (InstantForum_Messages.NoOfReplies) > 0 AND (InstantForum_Messages.LastPost >= DATEADD(d,-14,GETDATE()))
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC,  InstantForum_Messages.PinnedPost DESC,
	InstantForum_Messages.LastPost DESC, InstantForum_Messages.NoOfReplies DESC, InstantForum_Messages.[Views] DESC	 
END
ELSE IF  (@Type = 'popular') BEGIN
	SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	AND (InstantForum_Messages.LastPost >= DATEADD(d,-14,GETDATE()))
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC, InstantForum_Messages.PinnedPost DESC,
	InstantForum_Messages.NoOfReplies DESC, InstantForum_Messages.[Views] DESC, InstantForum_Messages.LastPost DESC
END
ELSE IF  (@Type = 'misc12') BEGIN
	DECLARE @12HoursAgo smalldatetime
	SET @12HoursAgo = DATEADD(hh,-12,GETDATE())
	
	SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	AND (InstantForum_Messages.LastPost >= @12HoursAgo)
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC, InstantForum_Messages.PinnedPost DESC, InstantForum_Messages.LastPost DESC
END
ELSE IF  (@Type = 'misc24') BEGIN
	DECLARE @24HoursAgo smalldatetime
	SET @24HoursAgo = DATEADD(hh,-24,GETDATE())
	
	SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	AND (InstantForum_Messages.LastPost >= @24HoursAgo) 
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC, InstantForum_Messages.PinnedPost DESC, InstantForum_Messages.LastPost DESC
END
ELSE IF  (@Type = 'misc48') BEGIN
	
	SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	AND (InstantForum_Messages.LastPost >= DATEADD(d,-1,GETDATE())) 
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC, InstantForum_Messages.PinnedPost DESC, InstantForum_Messages.LastPost DESC
END
ELSE IF  (@Type = 'lastvisit') BEGIN
	
	SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	AND (InstantForum_Messages.LastPost >= @LastVisit) 
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC, InstantForum_Messages.LastPost DESC
END
ELSE BEGIN
SELECT TOP 100 InstantForum_Messages.ForumID, InstantForum_Messages.MessageID, InstantForum_Messages.UserID, InstantForum_Messages.LastPost, 
	InstantForum_Messages.LastUserID, InstantForum_Messages.NoOfReplies, InstantForum_Messages.PostedDate, InstantForum_Messages.Subject, InstantForum_Messages.Message,
	InstantForum_Messages.Views, InstantForum_Messages.PinnedPost, InstantForum_Messages.Announcement, 
	InstantForum_Messages.MessageIcon, InstantForum_Messages.MovedPost, InstantForum_ForumGroups.GroupID, InstantForum_ForumGroups.GroupName,
	InstantForum_ForumGroups.GroupOrder, InstantForum_Forums.ForumSortOrder, InstantForum_Forums.ForumName FROM InstantForum_Messages INNER JOIN
	InstantForum_ForumMemberGroups ON InstantForum_Messages.ForumID = InstantForum_ForumMemberGroups.ForumID INNER JOIN
	InstantForum_Forums ON InstantForum_ForumMemberGroups.ForumID = InstantForum_Forums.ForumID INNER JOIN
	InstantForum_ForumGroups ON InstantForum_Forums.GroupID = InstantForum_ForumGroups.GroupID
	WHERE (InstantForum_Messages.IsTopic = 1) AND (InstantForum_Messages.Approved = 1) AND (InstantForum_ForumMemberGroups.MemberGroupID = @MemberGroupID)
	ORDER BY InstantForum_ForumGroups.GroupOrder DESC, InstantForum_Forums.ForumSortOrder DESC, InstantForum_Messages.LastPost DESC
END
RETURN
GO

GRANT EXEC ON dbo.if_sp_ShowMiscellaneousTopics TO PUBLIC
GO