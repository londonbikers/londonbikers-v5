IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'IsDocumentDuplicateToday')
	BEGIN
		DROP Procedure IsDocumentDuplicateToday
	END
GO

CREATE Procedure IsDocumentDuplicateToday
(
	@Title VARCHAR(512)
)
AS
    SELECT COUNT(0) 
	    FROM apollo_content 
	    WHERE f_title = @Title AND 
	    YEAR(f_creation_date) = YEAR(getutcdate()) AND
	    MONTH(f_creation_date) = MONTH(getutcdate()) AND
	    DAY(f_creation_date) = DAY(getutcdate())
GO

GRANT EXEC ON IsDocumentDuplicateToday TO PUBLIC
GO