IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateDocument')
	BEGIN
		PRINT 'Dropping Procedure UpdateDocument'
		DROP Procedure UpdateDocument
	END
GO

PRINT 'Creating Procedure UpdateDocument'
GO

CREATE Procedure dbo.UpdateDocument
	@ID bigint,
	@Title varchar(512),
	@Author uniqueidentifier,
	@Type tinyint,
	@LeadStatement varchar(512),
	@Abstract text,
	@Body text,
	@Status varchar(20),
	@PublishedDate datetime,
	@Tags text
AS
	/******************************************************************************
	**		File: UpdateDocument.sql
	**		Name: UpdateDocument
	**
	**		Auth: Jay Adair
	**		Date: 20.08.05
	*******************************************************************************
	**		Change History
	*******************************************************************************
	**		Date:		Author:				Description:
	**		--------	--------			---------------------------------------
	**		04.06.06	Jay Adair			Changed the Type column to reflect the
	**										new data-type.
	**		17.12.06	Jay Adair			Added in the Tags field.
	**    
	*******************************************************************************/

	SET NOCOUNT ON
	DECLARE @ReturnID INT
	IF (@ID > 0)
	BEGIN
		UPDATE
			apollo_content
			SET
			f_title = @Title,
			f_author = @Author,
			f_lead_statement = @LeadStatement,
			f_abstract = @Abstract,
			f_body = @Body,
			f_status = @Status,
			f_publish_date = @PublishedDate,
			Type = @Type,
			Tags = @Tags
			WHERE
			ID = @ID
			SET @ReturnID = @ID
	END
	ELSE
	BEGIN
		INSERT INTO
			apollo_content
			(
				f_title,
				f_author,
				f_lead_statement,
				f_abstract,
				f_body,
				f_status,
				f_publish_date,
				Type,
				Tags
			)
			VALUES
			(
				@Title,
				@Author,
				@LeadStatement,
				@Abstract,
				@Body,
				@Status,
				@PublishedDate,
				@Type,
				@Tags
			)
		SET @ReturnID = @@Identity
	END
	SELECT @ReturnID
GO

GRANT EXEC ON dbo.UpdateDocument TO PUBLIC
GO