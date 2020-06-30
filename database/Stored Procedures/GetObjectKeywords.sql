IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetObjectKeywords')
	BEGIN
		PRINT 'Dropping Procedure GetObjectKeywords'
		DROP Procedure GetObjectKeywords
	END
GO

PRINT 'Creating Procedure GetObjectKeywords'
GO

CREATE Procedure GetObjectKeywords
	@ObjectUID uniqueidentifier
AS

/******************************************************************************
**		File: 
**		Name: Stored_Procedure_Name
**		Desc: 
**
**		This template can be customized:
**              
**		Return values:
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**		----------						-----------
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT
	k.f_uid as "UID",
	k.f_name as "Name",
	k.f_url as "Url",
	k.f_definition as "Definition",
	k.f_author as "Author"
	FROM
	apollo_content_keywords ck
	INNER JOIN apollo_keywords k ON k.f_uid = ck.f_keyword_uid
	WHERE
	ck.f_object_uid = @objectUID

GO

GRANT EXEC ON GetObjectKeywords TO PUBLIC
GO