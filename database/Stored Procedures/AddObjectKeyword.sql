IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'AddObjectKeyword')
	BEGIN
		PRINT 'Dropping Procedure AddObjectKeyword'
		DROP Procedure AddObjectKeyword
	END
GO

PRINT 'Creating Procedure AddObjectKeyword'
GO

CREATE Procedure AddObjectKeyword
	@ObjectUID uniqueidentifier,
	@KeywordUID uniqueidentifier
AS

/******************************************************************************
**		File: AddObjectKeyword
**		Name: AddObjectKeyword.sql
**		Desc: 
**
**		Auth: Jay Adair
**		Date: 19.08.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
INSERT INTO
	apollo_content_keywords
	(
		f_object_uid,
		f_keyword_uid
	)
	VALUES
	(
		@ObjectUID,
		@KeywordUID
	)
GO

GRANT EXEC ON AddObjectKeyword TO PUBLIC
GO