IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateGalleryCategory')
	BEGIN
		PRINT 'Dropping Procedure Stored_Procedure_Name'
		DROP Procedure UpdateGalleryCategory
	END
GO

PRINT 'Creating Procedure UpdateGalleryCategory'
GO

CREATE Procedure UpdateGalleryCategory
	@ID bigint,
	@Name varchar(100),
	@Description text,
	@OwnerUID uniqueidentifier,
	@Type tinyint,
	@Active bit,
	@ParentSiteID int
AS
	SET NOCOUNT ON
	DECLARE @ReturnID INT
	IF (@ID > 0)
	BEGIN
		UPDATE
			apollo_gallery_categories
			SET
			f_name = @Name,
			f_owner = @OwnerUID,
			f_description = @Description,
			f_type = @Type,
			f_active = @Active,
			ParentSiteID = @ParentSiteID
			WHERE
			ID = @ID
			SET @ReturnID = @ID
	END
	ELSE
	BEGIN
		INSERT INTO
			apollo_gallery_categories
			(
				f_name,
				f_owner,
				f_description,
				f_type,
				f_active,
				ParentSiteID
			)
			VALUES
			(
				@Name,
				@OwnerUID,
				@Description,
				@Type,
				@Active,
				@ParentSiteID
			)
			SET @ReturnID = @@Identity
	END
	SELECT @ReturnID
GO

GRANT EXEC ON UpdateGalleryCategory TO PUBLIC
GO