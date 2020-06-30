IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDirectoryItem')
	BEGIN
		PRINT 'Dropping Procedure GetDirectoryItem'
		DROP Procedure GetDirectoryItem
	END
GO

PRINT 'Creating Procedure GetDirectoryItem'
GO

CREATE Procedure dbo.GetDirectoryItem
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		Title,
		[Description],
		TelephoneNumber,
		Created,
		Updated,
		Submiter,
		[Status],
		Rating,
		NumberOfRatings,
		Images,
		Links,
		Keywords,
		Postcode,
		Longitude,
		Latitude
		FROM
		DirectoryItems
		WHERE
		ID = @ID
GO

GRANT EXEC ON dbo.GetDirectoryItem TO PUBLIC
GO