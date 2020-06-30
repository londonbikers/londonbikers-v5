IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateGalleryObject')
	BEGIN
		PRINT 'Dropping Procedure UpdateGalleryObject'
		DROP Procedure UpdateGalleryObject
	END
GO

PRINT 'Creating Procedure UpdateGalleryObject'
GO

CREATE Procedure dbo.UpdateGalleryObject
	@ID bigint,
	@Title varchar(512),
	@CreationDate datetime,
	@Description text,
	@Type tinyint,
	@Status tinyint,
	@IsPublic bit
AS
	/******************************************************************************
	**		File: UpdateGalleryObject.sql
	**		Name: UpdateGalleryObject
	**
	**		Auth: Jay Adair
	**		Date: 21.08.05
	*******************************************************************************
	**		Change History
	*******************************************************************************
	**		Date:		Author:				Description:
	**		--------	--------			---------------------------------------
	**		21.08.05	Jay Adair			Extended to perform update and insert operations.
	**		13.09.05	Jay Adair			Added the CreationDate column to the update.
	**    
	*******************************************************************************/

	SET NOCOUNT ON
	DECLARE @ReturnID BIGINT
	IF (@ID > 0)
	BEGIN
		UPDATE
			apollo_galleries
			SET
			f_title = @Title,
			f_description = @Description,
			f_creation_date = @CreationDate,
			f_type = @Type,
			f_status = @Status,
			f_is_public = @IsPublic
			WHERE
			ID = @ID
		SET @ReturnID = @ID
	END
	ELSE
	BEGIN
		INSERT INTO apollo_galleries
		(
			f_title,
			f_creation_date,
			f_description,
			f_type,
			f_status,
			f_is_public
		)
		VALUES
		(
			@Title,
			@CreationDate,
			@Description,
			@Type,
			@Status,
			@IsPublic
		)
		SET @ReturnID = @@Identity
	END
	SELECT @ReturnID
GO

GRANT EXEC ON UpdateGalleryObject TO PUBLIC
GO