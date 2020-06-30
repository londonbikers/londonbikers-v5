IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateComment')
	BEGIN
		DROP Procedure UpdateComment
	END
GO

CREATE Procedure dbo.UpdateComment
(
	@ID bigint,
	@AuthorID uniqueidentifier,
	@Created datetime,
	@Comment ntext,
	@Status tinyint,
	@OwnerID bigint,
	@OwnerType tinyint,
	@ReportStatus tinyint,
	@ReceiveNotifications bit,
	@RequiresNotification bit
)
AS
	-- is this a new, or existing Comment?
	IF @ID = 0
	BEGIN
		INSERT INTO
			Comments
			(
				AuthorID,
				Created,
				Comment,
				[Status],
				OwnerID,
				OwnerType,
				ReceiveNotification
			)
			VALUES
			(
				@AuthorID,
				@Created,
				@Comment,
				@Status,
				@OwnerID,
				@OwnerType,
				@ReceiveNotifications
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			Comments
			SET
			AuthorID = @AuthorID,
			Created = @Created,
			Comment = @Comment,
			[Status] = @Status,
			OwnerID = @OwnerID,
			OwnerType = @OwnerType,
			ReportStatus = @ReportStatus,
			ReceiveNotification = @ReceiveNotifications,
			RequiresNotification = @RequiresNotification
			WHERE
			ID = @ID
		SELECT @ID
	END
GO

GRANT EXEC ON UpdateComment TO PUBLIC
GO