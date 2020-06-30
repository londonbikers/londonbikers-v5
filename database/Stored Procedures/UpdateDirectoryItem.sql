IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateDirectoryItem')
	BEGIN
		PRINT 'Dropping Procedure UpdateDirectoryItem'
		DROP Procedure UpdateDirectoryItem
	END
GO

PRINT 'Creating Procedure UpdateDirectoryItem'
GO

CREATE Procedure dbo.UpdateDirectoryItem
	@ID bigint,
	@Title varchar(256),
	@Description text,
	@TelephoneNumber varchar(20),
	@Keywords text,
	@Links text,
	@Images text,
	@Rating decimal,
	@NumberOfRatings int,
	@Submiter uniqueidentifier,
	@Status tinyint,
	@Postcode varchar(8),
	@Longitude float,
	@Latitude float
AS
	SET NOCOUNT ON
	DECLARE @ReturnID BIGINT
	IF (@ID < 1)
	BEGIN
		INSERT INTO
			DirectoryItems
			(
				Title,
				Description,
				TelephoneNumber,
				Keywords,
				Links,
				Images,
				Rating,
				NumberOfRatings,
				Submiter,
				Status,
				Postcode,
				Longitude,
				Latitude
			)
			VALUES
			(
				@Title,
				@Description,
				@TelephoneNumber,
				@Keywords,
				@Links,
				@Images,
				@Rating,
				@NumberOfRatings,
				@Submiter,
				@Status,
				@Postcode,
				@Longitude,
				@Latitude
			)
		SET @ReturnID = @@Identity
	END
	ELSE
	BEGIN
		UPDATE
			DirectoryItems
			SET
			Title = @Title,
			Description = @Description,
			TelephoneNumber = @TelephoneNumber,
			Keywords = @Keywords,
			Links = @Links,
			Images = @Images,
			Rating = @Rating,
			NumberOfRatings = @NumberOfRatings,
			Submiter = @Submiter,
			Status = @Status,
			Updated = getdate(),
			Postcode = @Postcode,
			Longitude = @Longitude,
			Latitude = @Latitude
			WHERE
			ID = @ID
		SET @ReturnID = @ID
	END
	SELECT @ReturnID
GO

GRANT EXEC ON dbo.UpdateDirectoryItem TO PUBLIC
GO