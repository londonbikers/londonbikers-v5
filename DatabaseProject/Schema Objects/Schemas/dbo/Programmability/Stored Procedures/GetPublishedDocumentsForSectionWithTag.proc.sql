
-------------------------------------------------------------------

CREATE Procedure [dbo].[GetPublishedDocumentsForSectionWithTag]
	@SectionID int,
	@Tag varchar(255),
	@Limit int
AS
	SET @Tag = REPLACE(@Tag, '"', '""')
	SET @Tag = '"' + @Tag + '"'
	SELECT
		TOP (@Limit)
		c.ID
		FROM
		apollo_content c
		INNER JOIN DocumentMappings dm ON dm.DocumentID = c.ID
		WHERE
		dm.ParentSectionID = @SectionID AND
		CONTAINS(Tags, @Tag) AND
		c.f_status = 'Published'
		ORDER BY 
		c.f_publish_date DESC
