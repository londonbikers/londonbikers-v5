CREATE PROCEDURE [dbo].[GetPopularDocuments]
	@PeriodDays int = 30,
	@ArticlePeriodDays int = 90,
	@MaxResults int = 100
AS
	SELECT TOP (@MaxResults) id
		FROM apollo_content 
		WHERE f_status = 'Published' AND
		[Type] = 0 AND 
		f_publish_date BETWEEN DATEADD(d, -@PeriodDays, GETUTCDATE()) AND DATEADD(d, 1, GETUTCDATE())
		ORDER BY [Views] DESC
		
	SELECT TOP (@MaxResults) id
		FROM apollo_content 
		WHERE f_status = 'Published' AND
		[Type] = 1 AND 
		f_publish_date BETWEEN DATEADD(d, -@ArticlePeriodDays, GETUTCDATE()) AND DATEADD(d, 1, GETUTCDATE())
		ORDER BY [Views] DESC