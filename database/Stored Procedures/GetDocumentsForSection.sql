IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetPublishedDocumentsForSection')
	BEGIN
		PRINT 'Dropping Procedure GetPublishedDocumentsForSection'
		DROP Procedure GetPublishedDocumentsForSection
	END
GO

PRINT 'Creating Procedure GetPublishedDocumentsForSection'
GO

CREATE Procedure dbo.GetPublishedDocumentsForSection
	@SectionID int
AS

/******************************************************************************
**		Desc: Retrieves published documents for a given Section.
**
**		Auth: Jay Adair
**		Date: 09.07.06
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	c.f_uid AS [UID]
	FROM
	apollo_content c
	INNER JOIN DocumentMappings dm ON dm.DocumentID = c.f_uid
	WHERE
	dm.SectionID = @SectionID AND
	c.f_status = 'Published'
GO

GRANT EXEC ON dbo.GetPublishedDocumentsForSection TO PUBLIC
GO