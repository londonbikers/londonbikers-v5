CREATE PROCEDURE [dbo].[GetLatestCommentedDocumentsForTag]
	@Tag varchar(255), 
	@PeriodDays int = 30,
	@MaxResults int = 100
AS
	SET @Tag = REPLACE(@Tag, '"', '""')
	SET @Tag = '"' + @Tag + '"'
	;WITH Documents_CTE (id, document_published, comment_created)
		AS
		(
			SELECT TOP (100) ac.id, ac.f_publish_date, c.Created
				FROM apollo_content ac
				INNER JOIN Comments c ON c.OwnerID = ac.id
				WHERE f_status = 'Published' AND 
				c.OwnerType = 0 AND 
				CONTAINS(Tags, @Tag) --AND 
				--f_publish_date BETWEEN DATEADD(d, -9999, GETUTCDATE()) AND GETUTCDATE()
				ORDER BY c.Created DESC
		)
		
	SELECT id
		FROM
		(
			SELECT TOP 100 id, 
			comment_created, 
			row_number() over(partition by id order by comment_created desc) as col
			FROM Documents_CTE
			ORDER BY comment_created desc
		) t
		WHERE col = 1
		ORDER BY comment_created DESC