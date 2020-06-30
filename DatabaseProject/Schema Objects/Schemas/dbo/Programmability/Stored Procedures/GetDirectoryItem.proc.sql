
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
		Latitude,
		Views
		FROM
		DirectoryItems
		WHERE
		ID = @ID
