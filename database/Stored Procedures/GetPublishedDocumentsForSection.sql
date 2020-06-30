IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetPublishedDocumentsForSection')
	BEGIN
		PRINT 'Dropping Procedure GetPublishedDocumentsForSection'
		DROP Procedure GetPublishedDocumentsForSection
	END
GO

PRINT 'Creating Procedure GetPublishedDocumentsForSection'
GO

CREATE Procedure [dbo].[GetPublishedDocumentsForSection]
	@SectionID int,
	@Limit int
AS
	SELECT
		TOP (@Limit)
		c.ID
		FROM
		apollo_content c
		INNER JOIN DocumentMappings dm ON dm.DocumentID = c.ID
		WHERE
		dm.ParentSectionID = @SectionID AND
		c.f_status = 'Published'
		ORDER BY 
		c.f_publish_date DESC
GO

GRANT EXEC ON dbo.GetPublishedDocumentsForSection TO PUBLIC
GO