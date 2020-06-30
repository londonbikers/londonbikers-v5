CREATE PROCEDURE [dbo].[GetTagPopularDocuments]
	@Tag varchar(255), 
	@PeriodDays int = 30,
	@MaxResults int = 100
AS
	SET @Tag = REPLACE(@Tag, '"', '""')
	SET @Tag = '"' + @Tag + '"'
	SELECT TOP (@MaxResults) id
		FROM apollo_content 
		WHERE f_status = 'Published' AND 
		CONTAINS(Tags, @Tag) AND 
		[Type] = 0 AND 
		f_publish_date BETWEEN DATEADD(d, -@PeriodDays, GETUTCDATE()) AND DATEADD(d, 1, GETUTCDATE())
		ORDER BY [Views] DESC
		
	SELECT TOP (@MaxResults) id
		FROM apollo_content 
		WHERE f_status = 'Published' AND 
		CONTAINS(Tags, @Tag) AND 
		[Type] = 1 AND 
		f_publish_date BETWEEN DATEADD(d, -@PeriodDays, GETUTCDATE()) AND DATEADD(d, 1, GETUTCDATE())
		ORDER BY [Views] DESC
GO