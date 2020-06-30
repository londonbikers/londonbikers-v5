 ---------------------------------------------------------------------
-- STORED PROCEDURE CHANGES
---------------------------------------------------------------------


ALTER Procedure [dbo].[GetEditorialDocument]
	@ID BigInt
AS
	SET NOCOUNT ON
	SELECT
		ID,
		f_title AS [Title],
		f_author AS [Author],
		f_creation_date AS [Created],
		f_publish_date AS [Published],
		f_lead_statement AS [LeadStatement],
		f_abstract AS [Abstract],
		f_body AS [Body],
		f_status AS [Status],
		Type,
		Tags
		FROM
		apollo_content
		WHERE
		ID = @ID
GO

---------------------------------------------------------------------

ALTER Procedure [dbo].[GetSimpleContentList]
AS
	SELECT
		ID,
		f_title AS [Title],
		[Type],
		f_publish_date AS [LastModified]
		FROM
		apollo_content
		WHERE
		f_status = 'Published' AND
		[Type] < 2
		
	SELECT
		ID,
		f_title AS [Title],
		f_creation_date as  [LastModified]
		FROM
		apollo_galleries
		WHERE
		f_status = 1
		
	SELECT
		ID,
		Title,
		Updated as  [LastModified]
		FROM
		DirectoryItems
		WHERE
		[Status] = 1
GO

---------------------------------------------------------------------

ALTER Procedure [dbo].[GetGalleryCategory]
	@ID bigint
AS
	SELECT 
		ID,
		f_name AS [Name],
		f_description AS [Description],
		f_active AS [Active],
		f_owner AS [Owner],
		f_type AS [Type],
		ParentSiteID
		FROM
		apollo_gallery_categories
		WHERE
		ID = @ID
GO

---------------------------------------------------------------------

ALTER Procedure [dbo].[GetGalleryObject]
	@ID BigInt
AS
	-- GALLERY DATA
	SELECT
		ID,
		f_title as [Title],
		f_creation_date as [CreationDate],
		f_description as [Description],
		f_type as [Type],
		f_status as [Status],
		f_is_public as [IsPublic]
		FROM
		apollo_galleries
		WHERE
		ID = @ID
		
	-- GALLERY CATEGORIES
	SELECT
		CategoryID
		FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID
		
	-- GALLERY IMAGES
	SELECT
		ID,
		BaseUrl,
		CreationDate,
		CaptureDate,
		[Name],
		Comment,
		Credit,
		[Views],
		GalleryID,
		Filename800,
		Filename1024,
		Filename1600,
		ThumbnailFilename
		FROM
		GalleryImages
		WHERE
		GalleryID = @ID
		ORDER BY
		CreationDate
GO

---------------------------------------------------------------------

ALTER Procedure [dbo].[GetEditorialRelatedObjects]
	@ObjectID bigint
AS
	SET NOCOUNT ON
	SELECT
		ObjectBID as [ID]
		FROM
		apollo_related_objects
		WHERE
		ObjectAID = @ObjectID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetSectionFeaturedDocuments]
	@SectionID int,
	@MaxResults int
AS
	SELECT
		TOP (@MaxResults)
		DocumentMappings.DocumentID
		FROM
		DocumentMappings 
		INNER JOIN apollo_content c ON c.ID = DocumentMappings.DocumentID
		WHERE
		DocumentMappings.IsFeaturedDocument = 1 AND 
		DocumentMappings.ParentSectionID = @SectionID
		ORDER BY 
		c.f_publish_date DESC
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetComments]
(
	@OwnerID bigint
)
AS
	SELECT
		ID,
		AuthorID,
		Created,
		Comment,
		[Status],
		OwnerType,
		ReportStatus
		FROM
		Comments
		WHERE
		OwnerID = @OwnerID
		ORDER BY
		Created ASC
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryItem]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		Title,
		Description,
		TelephoneNumber,
		Created,
		Updated,
		Submiter,
		[Status],
		Rating,
		NumberOfRatings,
		Images,
		Links,
		Keywords,
		Postcode,
		Longitude,
		Latitude
		FROM
		DirectoryItems
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateDirectoryCategory]
	@ID bigint,
	@Name varchar(256),
	@Description text,
	@RequiresMembership bit,
	@Keywords text,
	@ParentCategoryID bigint
AS
	SET NOCOUNT ON
	IF (@ID = 0)
	BEGIN
		INSERT INTO
			DirectoryCategories
			(
				[Name],
				[Description],
				RequiresMembership,
				Keywords,
				ParentCategoryID
			)
			VALUES
			(
				@Name,
				@Description,
				@RequiresMembership,
				@Keywords,
				@ParentCategoryID
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			DirectoryCategories
			SET
			[Name] = @Name,
			[Description] = @Description,
			RequiresMembership = @RequiresMembership,
			Keywords = @Keywords,
			ParentCategoryID = @ParentCategoryID
			WHERE
			ID = @ID
		SELECT @ID
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryCategory]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		[Name],
		[Description],
		RequiresMembership,
		Keywords,
		ParentCategoryID
		FROM
		DirectoryCategories
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[DeleteDirectoryItem]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
		
	DELETE FROM
		DirectoryItems
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[DeleteDirectoryCategory]
	@ID bigint
AS
	SET NOCOUNT ON

	-- remove any subcategory associations.
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = null
		WHERE
		ParentCategoryID = @ID

	DELETE FROM
		DirectoryCategoryItems
		WHERE
		CategoryID = @ID
		
	DELETE FROM 
		DirectoryCategories
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryCategoryItems]
	@CategoryID bigint
AS
	SELECT
		ID
		FROM
		DirectoryItems DI
		INNER JOIN DirectoryCategoryItems DCI ON DI.ID = DCI.ItemID
		WHERE
		DCI.CategoryID = @CategoryID
		ORDER BY
		Rating DESC,
		Title ASC
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryCategorySubCategories]
	@CategoryID bigint
AS
	SET NOCOUNT ON
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID = @CategoryID
		ORDER BY
		[Name]
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetCategoryParents]
	@ID bigint
AS
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[LatestPublicGalleries]
AS
	SELECT
		TOP 50
		ID
		FROM
		apollo_galleries
		WHERE
		f_is_public = 1 AND
		f_status = 1
		ORDER BY
		f_creation_date DESC
GO

-------------------------------------------------------------------

ALTER PROCEDURE [dbo].[GetGalleryCategories]
(
		@ID bigint
)
AS
	SET NOCOUNT ON
	SELECT
		CategoryID
		FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[ClearGalleryExhibitRelationships]
	@ID bigint
AS
	SET NOCOUNT ON
	UPDATE
		GalleryImages
		SET
		GalleryID = NULL
		WHERE
		GalleryID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateGalleryImage]
	@ID bigint,
	@Name varchar(256),
	@Comment text,
	@Credit varchar(512),
	@CreationDate datetime,
	@CaptureDate datetime,
	@BaseUrl varchar(256),
	@800 varchar(256),
	@1024 varchar(256),
	@1600 varchar(256),
	@Thumbnail varchar(256),
	@GalleryID bigint
AS
	IF (@ID > 0)
	BEGIN
		UPDATE
			GalleryImages
			SET
			[Name] = @Name,
			Comment = @Comment,
			Credit = @Credit,
			CaptureDate = @CaptureDate,
			BaseUrl = @BaseUrl,
			Filename800 = @800,
			Filename1024 = @1024,
			Filename1600 = @1600,
			ThumbnailFilename = @Thumbnail,
			GalleryID = @GalleryID
			WHERE
			ID = @ID
		SELECT @ID
	END
	ELSE
	BEGIN
		INSERT INTO
			GalleryImages
			(
				[Name],
				Comment,
				Credit,
				CreationDate,
				CaptureDate,
				BaseUrl,
				Filename800,
				Filename1024,
				Filename1600,
				ThumbnailFilename,
				GalleryID
			)
			VALUES
			(
				@Name,
				@Comment,
				@Credit,
				@CreationDate,
				@CaptureDate,
				@BaseUrl,
				@800,
				@1024,
				@1600,
				@Thumbnail,
				@GalleryID
			)
		SELECT SCOPE_IDENTITY()
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetAllGalleryCategoryGalleries]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		cgr.GalleryID as [ID]
		FROM
		apollo_gallery_category_gallery_relations cgr
		INNER JOIN apollo_galleries g on g.ID = cgr.GalleryID
		WHERE
		cgr.CategoryID = @ID
		ORDER BY
		g.f_creation_date desc
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetGalleryCategoryGalleries]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		r.GalleryID as [ID]
		FROM
		apollo_gallery_category_gallery_relations r 
		INNER JOIN apollo_galleries g ON g.ID = r.GalleryID
		WHERE
		r.CategoryID = @ID AND
		g.f_status = 1
		ORDER BY
		g.f_creation_date desc
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryItemCategories]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		CategoryID
		FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
GO

-------------------------------------------------------------------

ALTER PROCEDURE [dbo].[FlushContentImages]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_content_images
		WHERE
		ContentID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetContentImages]
	@ContentID bigint
AS
	SET NOCOUNT ON
	SELECT
		ci.ImageID as "ID",
		ci.f_cover_image as "Cover",
		ci.IntroImage as "Intro"
		FROM
		apollo_content_images ci
		INNER JOIN apollo_images i ON i.ID = ci.ImageID
		WHERE
		ci.ContentID = @ContentID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDocumentSections]
	@DocumentID bigint
AS
	SELECT
		ParentSectionID
		FROM
		DocumentMappings
		WHERE
		DocumentID = @DocumentID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[FlushEditorialRelatedObjects]
	@ObjectID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_related_objects
		WHERE
		ObjectAID = @ObjectID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[InsertEditorialRelatedObject]
	@ObjectID bigint,
	@RelatedObjectID bigint
AS
	SET NOCOUNT ON
	INSERT INTO
		apollo_related_objects
		(
			ObjectAID,
			ObjectBID
		)
		VALUES
		(
			@ObjectID,
			@RelatedObjectID
		)
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetGalleryImage]
	@ID as bigint
AS
	SELECT
		ID,
		BaseUrl,
		CreationDate,
		CaptureDate,
		[Name],
		Comment,
		Credit,
		Filename800,
		Filename1024,
		Filename1600,
		ThumbnailFilename,
		[Views],
		GalleryID
		FROM
		GalleryImages
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetPublishedDocumentsForSection]
	@SectionID int,
	@Limit int
AS
	SELECT
		TOP (@Limit)
		c.ID
		FROM
		apollo_content c
		INNER JOIN DocumentMappings dm ON dm.DocumentID = c.ID
		WHERE
		dm.ParentSectionID = @SectionID AND
		c.f_status = 'Published'
		ORDER BY 
		c.f_publish_date DESC
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetEditorialImage]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		ID,
		f_name AS [Name],
		f_filename AS [Filename],
		f_width AS [width],
		f_height AS [Height],
		f_created AS [Created],
		[Type]
		FROM
		apollo_images
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[CreateGalleryCategoryGalleryRelation]
	@ID bigint,
	@GalleryID bigint
AS
	SET NOCOUNT ON
	INSERT INTO
		apollo_gallery_category_gallery_relations
		(
			CategoryID,
			GalleryID
		)
		VALUES
		(
			@ID,
			@GalleryID
		)
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[DeleteGallery]
	@ID bigint
AS
	SET NOCOUNT ON

	DELETE FROM
		GalleryImages
		WHERE
		GalleryID = @ID
			
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		GalleryID = @ID

	DELETE FROM
		apollo_galleries
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetLatestGalleriesForSite]
	@ParentID int
AS
	SELECT
		g.ID
		FROM
		apollo_galleries g
		INNER JOIN apollo_gallery_category_gallery_relations cgr ON cgr.GalleryID = g.ID
		INNER JOIN apollo_gallery_categories gc ON cgr.CategoryID = gc.ID
		WHERE
		g.f_status = 1 AND
		gc.ParentSiteID = @ParentID
		GROUP BY
		g.ID, 
		g.f_creation_date
		ORDER BY
		g.f_creation_date DESC
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetPublishedDocumentsForSectionWithTag]
	@SectionID int,
	@Tag varchar(255),
	@Limit int
AS
	SET @Tag = '"' + @Tag + '"'
	SELECT
		TOP (@Limit)
		c.ID
		FROM
		apollo_content c
		INNER JOIN DocumentMappings dm ON dm.DocumentID = c.ID
		WHERE
		dm.ParentSectionID = @SectionID AND
		CONTAINS(Tags, @Tag) AND
		c.f_status = 'Published'
		ORDER BY 
		c.f_publish_date DESC
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetSiteGalleryCategories]
	@SiteID int
AS
	SELECT
		ID
		FROM
		apollo_gallery_categories
		WHERE
		f_type = 0 AND
		f_active = 1 AND
		ParentSiteID = @SiteID
		ORDER BY
		f_name
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetRootDirectoryCategories]
AS
	SET NOCOUNT ON
	SELECT
		ID
		FROM
		DirectoryCategories
		WHERE
		ParentCategoryID IS NULL
		ORDER BY
		[Name]
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateGalleryCategory]
	@ID bigint,
	@Name varchar(100),
	@Description text,
	@OwnerUID uniqueidentifier,
	@Type tinyint,
	@Active bit,
	@ParentSiteID int
AS
	SET NOCOUNT ON
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
		SELECT @ID
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
		SELECT SCOPE_IDENTITY()
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[ClearGalleryCategoryGalleries]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		CategoryID = @ID
GO

-------------------------------------------------------------------

ALTER PROCEDURE [dbo].[DeleteGalleryCategory]
	@ID bigint
AS
	DELETE FROM
		apollo_gallery_categories
		WHERE
		ID = @ID
		
	-- this should surfice, Apollo will ensure all relations underneath are severed before hand.
		
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		CategoryID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetGalleryImageParentID]
(
	@ImageID bigint
)
AS
	SELECT
		GalleryID
		FROM
		GalleryImages
		WHERE
		ID = @ImageID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetEditorialDocumentStatus]
	@ID bigint
AS
	SELECT
		f_status
		FROM
		apollo_content
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateDocument]
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
			[Type] = @Type,
			Tags = @Tags
			WHERE
			ID = @ID
		SELECT @ID
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
				[Type],
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
		SELECT SCOPE_IDENTITY()
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[ClearDocumentMappings]
	@DocumentID bigint
AS
	DELETE FROM
		DocumentMappings
		WHERE
		DocumentID = @DocumentID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[AddDocumentMapping]
	@DocumentID bigint,
	@SectionID int
AS
	INSERT INTO
		DocumentMappings
		(
			DocumentID,
			ParentSectionID
		)
		VALUES
		(
			@DocumentID,
			@SectionID
		)
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateContentImage]
	@ID bigint,
	@Name varchar(255),
	@Filename varchar(255),
	@Width int,
	@Height int,
	@Type tinyint
AS
	SET NOCOUNT ON
	IF (@ID < 1)
	BEGIN
		INSERT INTO
			apollo_images
			(
				f_name,
				f_filename,
				f_width,
				f_height,
				[Type]
			)
			VALUES
			(
				@Name,
				@Filename,
				@Width,
				@Height,
				@Type
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			apollo_images
			SET
			f_name = @Name,
			f_filename = @Filename,
			f_width = @Width,
			f_height = @Height,
			[Type] = @Type
			WHERE
			ID = @ID
		SELECT @ID
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[DeleteContentImage]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		apollo_content_images
		WHERE
		ImageID = @ID

	DELETE FROM
		apollo_images
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateDirectoryItem]
	@ID bigint,
	@Title varchar(256),
	@Description text,
	@TelephoneNumber varchar(20),
	@Keywords text,
	@Links text,
	@Images text,
	@Rating decimal,
	@NumberOfRatings int,
	@Submiter uniqueidentifier,
	@Status tinyint,
	@Postcode varchar(8),
	@Longitude float,
	@Latitude float
AS
	SET NOCOUNT ON
	IF (@ID < 1)
	BEGIN
		INSERT INTO
			DirectoryItems
			(
				Title,
				[Description],
				TelephoneNumber,
				Keywords,
				Links,
				Images,
				Rating,
				NumberOfRatings,
				Submiter,
				[Status],
				Postcode,
				Longitude,
				Latitude
			)
			VALUES
			(
				@Title,
				@Description,
				@TelephoneNumber,
				@Keywords,
				@Links,
				@Images,
				@Rating,
				@NumberOfRatings,
				@Submiter,
				@Status,
				@Postcode,
				@Longitude,
				@Latitude
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			DirectoryItems
			SET
			Title = @Title,
			[Description] = @Description,
			TelephoneNumber = @TelephoneNumber,
			Keywords = @Keywords,
			Links = @Links,
			Images = @Images,
			Rating = @Rating,
			NumberOfRatings = @NumberOfRatings,
			Submiter = @Submiter,
			[Status] = @Status,
			Updated = getdate(),
			Postcode = @Postcode,
			Longitude = @Longitude,
			Latitude = @Latitude
			WHERE
			ID = @ID
		SELECT @ID
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[FlushDirectoryItemCategories]
	@ID BIGINT
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		ItemID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[CreateDirectoryItemCategoryRelation]
	@ID BIGINT,
	@CategoryID BIGINT
AS
	SET NOCOUNT ON
	INSERT INTO
		DirectoryCategoryItems
		(
			CategoryID,
			ItemID
		)
		VALUES
		(
			@CategoryID,
			@ID
		)
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[FlushDirectoryCategoryItems]
	@ID bigint
AS
	SET NOCOUNT ON
	DELETE FROM
		DirectoryCategoryItems
		WHERE
		CategoryID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[InsertDirectoryCategoryItem]
	@CategoryID bigint,
	@ItemID bigint
AS
	SET NOCOUNT ON
	INSERT INTO
		DirectoryCategoryItems
		(
			CategoryID,
			ItemID
		)
		VALUES
		(
			@CategoryID,
			@ItemID
		)
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[FlushCategorySubCategories]
	@CategoryID bigint
AS
	SET NOCOUNT ON
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = null
		WHERE
		ParentCategoryID = @CategoryID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[InsertDirectorySubCategory]
	@CategoryID bigint,
	@SubCategoryID bigint
AS
	SET NOCOUNT ON
	UPDATE
		DirectoryCategories
		SET
		ParentCategoryID = @CategoryID
		WHERE
		ID = @SubCategoryID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateGalleryObject]
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
		SELECT @ID
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
		SELECT SCOPE_IDENTITY()
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateComment]
(
	@ID bigint,
	@AuthorID uniqueidentifier,
	@Created datetime,
	@Comment ntext,
	@Status tinyint,
	@OwnerID bigint,
	@OwnerType tinyint,
	@ReportStatus tinyint
)
AS
	-- is this a new, or existing Comment?
	IF @ID = 0
	BEGIN
		INSERT INTO
			Comments
			(
				AuthorID,
				Created,
				Comment,
				[Status],
				OwnerID,
				OwnerType
			)
			VALUES
			(
				@AuthorID,
				@Created,
				@Comment,
				@Status,
				@OwnerID,
				@OwnerType
			)
			
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			Comments
			SET
			AuthorID = @AuthorID,
			Created = @Created,
			Comment = @Comment,
			[Status] = @Status,
			OwnerID = @OwnerID,
			OwnerType = @OwnerType,
			ReportStatus = @ReportStatus
			WHERE
			ID = @ID
			SELECT @ID
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateUser]
	@UID uniqueidentifier,
	@Firstname varchar(100),
	@Lastname varchar(100),
	@Password varchar(50),
	@Email varchar(512),
	@Username varchar(100),
	@Status tinyint,
	@ForumUserID int
AS	
	SET NOCOUNT ON
	IF (SELECT COUNT(0) FROM apollo_users WHERE f_uid = @UID) > 0
	BEGIN
		-- UPDATE AN EXISTING USER
		UPDATE
		apollo_users
		SET
		f_firstname = @Firstname,
		f_lastname = @Lastname,
		f_password = @Password,
		f_email = @Email,
		f_username = @Username,
		[Status] = @Status,
		ForumUserID = @ForumUserID
		WHERE
		f_uid = @UID
	END
	ELSE
		-- CREATE A NEW USER
		INSERT INTO apollo_users
		(
			f_uid,
			f_firstname,
			f_lastname,
			f_password,
			f_email,
			f_username,
			f_created,
			[Status],
			ForumUserID
		)
		VALUES
		(
			@UID,
			@Firstname,
			@Lastname,
			@Password,
			@Email,
			@Username,
			getdate(),
			@Status,
			@ForumUserID
		)
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[UpdateComment]
(
	@ID bigint,
	@AuthorID uniqueidentifier,
	@Created datetime,
	@Comment ntext,
	@Status tinyint,
	@OwnerID bigint,
	@OwnerType tinyint,
	@ReportStatus tinyint
)
AS
	-- is this a new, or existing Comment?
	IF @ID = 0
	BEGIN
		INSERT INTO
			Comments
			(
				AuthorID,
				Created,
				Comment,
				[Status],
				OwnerID,
				OwnerType
			)
			VALUES
			(
				@AuthorID,
				@Created,
				@Comment,
				@Status,
				@OwnerID,
				@OwnerType
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			Comments
			SET
			AuthorID = @AuthorID,
			Created = @Created,
			Comment = @Comment,
			[Status] = @Status,
			OwnerID = @OwnerID,
			OwnerType = @OwnerType,
			ReportStatus = @ReportStatus
			WHERE
			ID = @ID
		SELECT @ID
	END
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[IncrementImageViewCount]
	@ID bigint
AS
	SET NOCOUNT ON
	UPDATE
		GalleryImages
		SET
		[Views] = [Views] + 1
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetGoogleNewsContentList]
AS
	SELECT
		TOP 1000
		ID,
		[Type],
		f_publish_date AS [LastModified],
		Tags
		FROM
		apollo_content
		WHERE
		f_status = 'Published' AND
		[Type] < 2
		ORDER BY
		f_publish_date DESC		
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryItem]
	@ID bigint
AS
	SET NOCOUNT ON
	SELECT
		Title,
		[Description],
		TelephoneNumber,
		Created,
		Updated,
		Submiter,
		[Status],
		Rating,
		NumberOfRatings,
		Images,
		Links,
		Keywords,
		Postcode,
		Longitude,
		Latitude
		FROM
		DirectoryItems
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetDirectoryCategory]
	@ID bigint
AS
SET NOCOUNT ON
	SELECT
		[Name],
		[Description],
		RequiresMembership,
		Keywords,
		ParentCategoryID
		FROM
		DirectoryCategories
		WHERE
		ID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[GetContentImageAssociationsCount]
	@ID bigint
AS
	SELECT 
		COUNT(0)
		FROM
		apollo_content_images
		WHERE
		ImageID = @ID
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[DeleteOrphanedImages]
AS
	SET NOCOUNT ON
	DELETE FROM
		GalleryImages
		WHERE
		GalleryID IS NULL
GO

-------------------------------------------------------------------

ALTER Procedure [dbo].[AddSectionFeaturedDocument]
	@SectionID int,
	@DocumentID bigint
AS
	UPDATE
		DocumentMappings
		SET
		IsFeaturedDocument = 1
		WHERE
		ParentSectionID = @SectionID AND
		DocumentID = @DocumentID
GO

-----------------------------------------------------------------

ALTER PROCEDURE [dbo].[AddContentImage]
	@content bigint,
	@image bigint,
	@coverImage bit,
	@IntroImage bit
AS
	SET NOCOUNT ON
	INSERT INTO
		apollo_content_images
		(
			ContentID,
			ImageID,
			f_cover_image,
			IntroImage
		)
		VALUES
		(
			@content,
			@image,
			@coverImage,
			@IntroImage
		)
GO

-----------------------------------------------------------------

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ConvertLegacyID')
	BEGIN
		DROP Procedure ConvertLegacyID
	END
GO

CREATE Procedure dbo.ConvertLegacyID
(
	@DomainObjectType varchar(50),
	@UID uniqueidentifier
)
AS
	IF (@DomainObjectType = 'Document')
	BEGIN
		SELECT ID FROM apollo_content WHERE f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'DocumentImage')
	BEGIN
		SELECT i.ID, m.ContentID FROM apollo_images i INNER JOIN apollo_content_images m on m.ImageID = i.ID WHERE i.f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'Gallery' OR @DomainObjectType = 'GallerySlideshow')
	BEGIN
		SELECT ID FROM apollo_galleries WHERE f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'GalleryImage')
	BEGIN
		SELECT ID FROM GalleryImages WHERE [Uid] = @UID
	END
	ELSE IF (@DomainObjectType = 'GalleryCategory')
	BEGIN
		SELECT ID FROM apollo_gallery_categories WHERE f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'DirectoryCategory')
	BEGIN
		SELECT ID FROM DirectoryCategories WHERE [UID] = @UID
	END
	ELSE IF (@DomainObjectType = 'DirectoryItem')
	BEGIN
		SELECT ID FROM DirectoryItems WHERE [UID] = @UID
	END
GO

GRANT EXEC ON ConvertLegacyID TO PUBLIC
GO

-----------------------------------------------------------------

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetTopForumPostersForLastMonth')
	BEGIN
		DROP Procedure GetTopForumPostersForLastMonth
	END
GO

CREATE Procedure dbo.GetTopForumPostersForLastMonth
(
	@MaxResults int
)
AS
	SELECT 
		TOP (@MaxResults)
		u.Username,
		t.UserID,
		COUNT(0) AS TotalPosts
		FROM 
		InstantForum_Topics t
		INNER JOIN InstantASP_Users u ON u.UserID = t.UserID
		WHERE 
		t.DateStamp BETWEEN dateadd(m, -1, getdate()) AND getdate() AND
		u.PrimaryRoleID NOT IN (9,12) -- No Moderators, Administrators
		GROUP BY
		t.UserID,
		u.Username
		ORDER BY
		COUNT(0) DESC
GO

GRANT EXEC ON GetTopForumPostersForLastMonth TO PUBLIC
GO

-----------------------------------------------------------------

ALTER Procedure [dbo].[GetUserStats]
AS
	SELECT
		(SELECT COUNT(0) FROM apollo_users) AS [TotalUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE [Status] = 1) AS [ActiveUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE [Status] = 2) AS [SuspendedUsers],
		(SELECT COUNT(0) FROM apollo_users WHERE [Status] = 0) AS [DeletedUsers]
GO

-----------------------------------------------------------------

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetMonthlyCommunityStats')
	BEGIN
		DROP Procedure GetMonthlyCommunityStats
	END
GO

CREATE Procedure dbo.GetMonthlyCommunityStats
AS
	SELECT
		(select count(0) from apollo_users where f_created between dateadd(m, -1, getdate()) and getdate()) AS NewUsers,
		(select count(0) from InstantForum_Topics where DateStamp between dateadd(m, -1, getdate()) and getdate()) AS NewPosts,
		(select count(0) from InstantForum_PrivateMessages where DateStamp between dateadd(m, -1, getdate()) and getdate()) AS NewPMs
GO

GRANT EXEC ON GetMonthlyCommunityStats TO PUBLIC
GO

-----------------------------------------------------------------

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateComment')
	BEGIN
		DROP Procedure UpdateComment
	END
GO

CREATE Procedure dbo.UpdateComment
(
	@ID bigint,
	@AuthorID uniqueidentifier,
	@Created datetime,
	@Comment ntext,
	@Status tinyint,
	@OwnerID bigint,
	@OwnerType tinyint,
	@ReportStatus tinyint,
	@ReceiveNotifications bit,
	@RequiresNotification bit
)
AS
	-- is this a new, or existing Comment?
	IF @ID = 0
	BEGIN
		INSERT INTO
			Comments
			(
				AuthorID,
				Created,
				Comment,
				[Status],
				OwnerID,
				OwnerType,
				ReceiveNotification
			)
			VALUES
			(
				@AuthorID,
				@Created,
				@Comment,
				@Status,
				@OwnerID,
				@OwnerType,
				@ReceiveNotifications
			)
		SELECT SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE
			Comments
			SET
			AuthorID = @AuthorID,
			Created = @Created,
			Comment = @Comment,
			[Status] = @Status,
			OwnerID = @OwnerID,
			OwnerType = @OwnerType,
			ReportStatus = @ReportStatus,
			ReceiveNotification = @ReceiveNotifications,
			RequiresNotification = @RequiresNotification
			WHERE
			ID = @ID
		SELECT @ID
	END
GO

GRANT EXEC ON UpdateComment TO PUBLIC
GO

-----------------------------------------------------------------

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetComments')
	BEGIN
		DROP Procedure GetComments
	END
GO

CREATE Procedure dbo.GetComments
(
	@OwnerID bigint
)
AS
	SELECT
		*
		FROM
		Comments
		WHERE
		OwnerID = @OwnerID
		ORDER BY
		Created ASC
GO

GRANT EXEC ON GetComments TO PUBLIC
GO

-----------------------------------------------------------------

 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGoogleNewsContentList')
	BEGIN
		PRINT 'Dropping Procedure GetGoogleNewsContentList'
		DROP Procedure GetGoogleNewsContentList
	END
GO

PRINT 'Creating Procedure GetGoogleNewsContentList'
GO

CREATE Procedure dbo.GetGoogleNewsContentList
AS
	SELECT
		TOP 1000
		ID,
		f_title as [Title],
		[Type],
		f_publish_date AS [LastModified],
		Tags
		FROM
		apollo_content
		WHERE
		f_status = 'Published' AND
		[Type] < 2
		ORDER BY
		f_publish_date DESC		
GO

GRANT EXEC ON dbo.GetGoogleNewsContentList TO PUBLIC
GO

-----------------------------------------------------------------