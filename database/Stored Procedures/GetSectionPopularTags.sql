IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSectionPopularTags')
	BEGIN
		DROP Procedure GetSectionPopularTags
	END
GO

CREATE Procedure dbo.GetSectionPopularTags
(
	@SectionID int
)
AS
	SELECT
		Tag,
		Occurrences
		FROM
		PopularTags
		WHERE
		SectionID = @SectionID
		ORDER BY
		Tag
GO

GRANT EXEC ON GetSectionPopularTags TO PUBLIC
GO