
---------------------------------------------------------------------

CREATE Procedure [dbo].[GetSimpleContentList]
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
		ORDER BY f_publish_date
		
	SELECT
		ID,
		f_title AS [Title],
		f_creation_date as  [LastModified]
		FROM
		apollo_galleries
		WHERE
		f_status = 1
		ORDER BY 
		f_creation_date
		
	SELECT
		ID,
		Title,
		Updated as  [LastModified]
		FROM
		DirectoryItems
		WHERE
		[Status] = 1
		ORDER BY
		Updated
