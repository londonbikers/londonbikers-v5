
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
