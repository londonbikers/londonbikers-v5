-- Fix any inconsistencies
UPDATE apollo_content SET [Type] = 1 WHERE f_content_type = 'Article'

-- news
INSERT INTO
	DocumentMappings (DocumentID, ParentSectionID)
	SELECT
		f_uid,
		1
		FROM
		apollo_content
		WHERE
		f_content_type = 'News'
	
-- articles	
INSERT INTO
	DocumentMappings (DocumentID, ParentSectionID)
	SELECT
		f_uid,
		2
		FROM
		apollo_content
		WHERE
		f_content_type = 'Article'