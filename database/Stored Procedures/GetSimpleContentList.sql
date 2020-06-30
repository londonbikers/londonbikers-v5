IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSimpleContentList')
	BEGIN
		PRINT 'Dropping Procedure GetSimpleContentList'
		DROP Procedure GetSimpleContentList
	END
GO

PRINT 'Creating Procedure GetSimpleContentList'
GO

CREATE Procedure dbo.GetSimpleContentList
AS
	SELECT
		ID,
		f_title AS [Title],
		[Type],
		f_publish_date AS [LastModified]
		FROM
		apollo_content
		WHERE
		f_status = 'Published' AND
		[Type] < 2
		
	SELECT
		ID,
		f_title AS [Title],
		f_creation_date as [LastModified]
		FROM
		apollo_galleries
		WHERE
		f_status = 1
		
	SELECT
		ID,
		Title,
		Updated as [LastModified]
		FROM
		DirectoryItems
		WHERE
		Status = 1
GO

GRANT EXEC ON dbo.GetSimpleContentList TO PUBLIC
GO