 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGoogleNewsContentList')
	BEGIN
		PRINT 'Dropping Procedure GetGoogleNewsContentList'
		DROP Procedure GetGoogleNewsContentList
	END
GO

PRINT 'Creating Procedure GetGoogleNewsContentList'
GO

CREATE Procedure dbo.GetGoogleNewsContentList
AS
	SELECT
		TOP 1000
		ID,
		f_title as [Title],
		[Type],
		f_publish_date AS [LastModified],
		CASE 
			WHEN Tags LIKE '%motogp%' THEN 'Racing (Sports),' + CAST(Tags AS VARCHAR(MAX))
			WHEN Tags LIKE '%bsb%' THEN 'Racing (Sports),' + CAST(Tags AS VARCHAR(MAX))
			WHEN Tags LIKE '%wsb%' THEN 'Racing (Sports),' + CAST(Tags AS VARCHAR(MAX))
			WHEN Tags LIKE '%mx%' THEN 'Racing (Sports),' + CAST(Tags AS VARCHAR(MAX))
			WHEN Tags LIKE '%motorcycles%' THEN 'Automotive (Lifestyle),' + CAST(Tags AS VARCHAR(MAX))
			WHEN Tags LIKE '%products%' THEN 'Automotive (Lifestyle),' + CAST(Tags AS VARCHAR(MAX))
			WHEN Tags LIKE '%london%' THEN 'Local,' + CAST(Tags AS VARCHAR(MAX))
			ELSE Tags
		END AS [Tags]
		FROM
		apollo_content
		WHERE
		f_status = 'Published' AND
		[Type] < 2 AND 
		f_publish_date > DATEADD(d, -3, GETDATE())		
		ORDER BY
		f_publish_date DESC
GO

GRANT EXEC ON dbo.GetGoogleNewsContentList TO PUBLIC
GO