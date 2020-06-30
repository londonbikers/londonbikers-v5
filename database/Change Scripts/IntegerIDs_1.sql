---------------------------------------------------------------------
-- TABLE STRUCTURE CHANGES
---------------------------------------------------------------------

-- Comments Restructuring
delete from comments where owneruid not in (
	select f_uid as [UID] from apollo_content
	union all
	select [UID] from galleryimages
	union all
	select [UID] from directoryitems
)
UPDATE Comments SET OwnerID = c.ID FROM Comments cmt INNER JOIN apollo_content c ON c.f_uid = cmt.OwnerUID 
UPDATE Comments SET OwnerID = gi.ID FROM Comments cmt INNER JOIN galleryimages gi ON gi.Uid = cmt.OwnerUID 
UPDATE Comments SET OwnerID = di.ID FROM Comments cmt INNER JOIN directoryitems di ON di.UID = cmt.OwnerUID
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Comments
	DROP CONSTRAINT DF_Comments_Created
GO
ALTER TABLE dbo.Comments
	DROP CONSTRAINT DF_Comments_Status
GO
ALTER TABLE dbo.Comments
	DROP CONSTRAINT DF_Comments_ReportStatus
GO
CREATE TABLE dbo.Tmp_Comments
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	AuthorID uniqueidentifier NOT NULL,
	Created datetime NOT NULL,
	Comment ntext NOT NULL,
	Status tinyint NOT NULL,
	OwnerID bigint NOT NULL,
	OwnerType tinyint NOT NULL,
	ReportStatus tinyint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Comments ADD CONSTRAINT
	DF_Comments_Created DEFAULT (getdate()) FOR Created
GO
ALTER TABLE dbo.Tmp_Comments ADD CONSTRAINT
	DF_Comments_Status DEFAULT ((1)) FOR Status
GO
ALTER TABLE dbo.Tmp_Comments ADD CONSTRAINT
	DF_Comments_ReportStatus DEFAULT ((0)) FOR ReportStatus
GO
SET IDENTITY_INSERT dbo.Tmp_Comments ON
GO
IF EXISTS(SELECT * FROM dbo.Comments)
	 EXEC('INSERT INTO dbo.Tmp_Comments (ID, AuthorID, Created, Comment, Status, OwnerID, OwnerType, ReportStatus)
		SELECT ID, AuthorID, Created, Comment, Status, OwnerID, OwnerType, ReportStatus FROM dbo.Comments WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Comments OFF
GO
DROP TABLE dbo.Comments
GO
EXECUTE sp_rename N'dbo.Tmp_Comments', N'Comments', 'OBJECT' 
GO
ALTER TABLE dbo.Comments ADD CONSTRAINT
	PK_Comments PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

CREATE NONCLUSTERED INDEX [IDX_secondary] ON [dbo].[Comments] 
(
	[Created] ASC,
	[OwnerID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


-- Gallery-Category table restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_gallery_category_gallery_relations ADD
	CategoryID bigint NULL,
	GalleryID bigint NULL
GO
COMMIT

UPDATE
	apollo_gallery_category_gallery_relations
	SET
	CategoryID = c.ID,
	GalleryID = g.ID
	FROM
	apollo_gallery_category_gallery_relations r
	INNER JOIN apollo_gallery_categories c ON c.f_uid = r.f_category_uid
	INNER JOIN apollo_galleries g ON g.f_uid = r.f_gallery_uid
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_apollo_gallery_category_gallery_relations
	(
	CategoryID bigint NOT NULL,
	GalleryID bigint NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.apollo_gallery_category_gallery_relations)
	 EXEC('INSERT INTO dbo.Tmp_apollo_gallery_category_gallery_relations (CategoryID, GalleryID)
		SELECT CategoryID, GalleryID FROM dbo.apollo_gallery_category_gallery_relations WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.apollo_gallery_category_gallery_relations
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_gallery_category_gallery_relations', N'apollo_gallery_category_gallery_relations', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_gallery_category_gallery_relations ADD CONSTRAINT
	PK_apollo_gallery_category_gallery_relations_1 PRIMARY KEY CLUSTERED 
	(
	CategoryID,
	GalleryID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- GalleryImages Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GalleryImages ADD
	GalleryID bigint NULL
GO
COMMIT

UPDATE
	GalleryImages
	SET
	GalleryID = g.ID
	FROM
	GalleryImages gi
	INNER JOIN apollo_galleries g ON g.f_uid = gi.GalleryUID
GO
DELETE FROM GalleryImages WHERE GalleryID IS NULL
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GalleryImages
	DROP CONSTRAINT DF_apollo_gallery_images_f_views
GO
CREATE TABLE dbo.Tmp_GalleryImages
	(
	Uid uniqueidentifier NOT NULL,
	ID bigint NOT NULL IDENTITY (1, 1),
	Name varchar(256) NULL,
	Credit varchar(512) NULL,
	Comment text NULL,
	CaptureDate datetime NOT NULL,
	CreationDate datetime NOT NULL,
	BaseUrl varchar(256) NULL,
	Filename800 varchar(256) NULL,
	Filename1024 varchar(256) NULL,
	Filename1280 varchar(256) NULL,
	Filename1600 varchar(256) NULL,
	ThumbnailFilename varchar(256) NULL,
	CustomThumbnailFilename varchar(256) NULL,
	Views int NOT NULL,
	GalleryID bigint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_GalleryImages ADD CONSTRAINT
	DF_apollo_gallery_images_f_views DEFAULT ((0)) FOR Views
GO
SET IDENTITY_INSERT dbo.Tmp_GalleryImages ON
GO
IF EXISTS(SELECT * FROM dbo.GalleryImages)
	 EXEC('INSERT INTO dbo.Tmp_GalleryImages (Uid, ID, Name, Credit, Comment, CaptureDate, CreationDate, BaseUrl, Filename800, Filename1024, Filename1280, Filename1600, ThumbnailFilename, CustomThumbnailFilename, Views, GalleryID)
		SELECT Uid, ID, Name, Credit, Comment, CaptureDate, CreationDate, BaseUrl, Filename800, Filename1024, Filename1280, Filename1600, ThumbnailFilename, CustomThumbnailFilename, Views, GalleryID FROM dbo.GalleryImages WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_GalleryImages OFF
GO
DROP TABLE dbo.GalleryImages
GO
EXECUTE sp_rename N'dbo.Tmp_GalleryImages', N'GalleryImages', 'OBJECT' 
GO
ALTER TABLE dbo.GalleryImages ADD CONSTRAINT
	PK_GalleryImages PRIMARY KEY CLUSTERED 
	(
	Uid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GalleryImages
	DROP CONSTRAINT DF_apollo_gallery_images_f_views
GO
CREATE TABLE dbo.Tmp_GalleryImages
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	Uid uniqueidentifier NULL,
	Name varchar(256) NULL,
	Credit varchar(512) NULL,
	Comment text NULL,
	CaptureDate datetime NOT NULL,
	CreationDate datetime NOT NULL,
	BaseUrl varchar(256) NULL,
	Filename800 varchar(256) NULL,
	Filename1024 varchar(256) NULL,
	Filename1280 varchar(256) NULL,
	Filename1600 varchar(256) NULL,
	ThumbnailFilename varchar(256) NULL,
	CustomThumbnailFilename varchar(256) NULL,
	Views int NOT NULL,
	GalleryID bigint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_GalleryImages ADD CONSTRAINT
	DF_apollo_gallery_images_f_views DEFAULT ((0)) FOR Views
GO
SET IDENTITY_INSERT dbo.Tmp_GalleryImages ON
GO
IF EXISTS(SELECT * FROM dbo.GalleryImages)
	 EXEC('INSERT INTO dbo.Tmp_GalleryImages (ID, Uid, Name, Credit, Comment, CaptureDate, CreationDate, BaseUrl, Filename800, Filename1024, Filename1280, Filename1600, ThumbnailFilename, CustomThumbnailFilename, Views, GalleryID)
		SELECT ID, Uid, Name, Credit, Comment, CaptureDate, CreationDate, BaseUrl, Filename800, Filename1024, Filename1280, Filename1600, ThumbnailFilename, CustomThumbnailFilename, Views, GalleryID FROM dbo.GalleryImages WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_GalleryImages OFF
GO
DROP TABLE dbo.GalleryImages
GO
EXECUTE sp_rename N'dbo.Tmp_GalleryImages', N'GalleryImages', 'OBJECT' 
GO
ALTER TABLE dbo.GalleryImages ADD CONSTRAINT
	PK_GalleryImages PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- apollo_related_objects Restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_related_objects ADD
	ObjectAID bigint NULL,
	ObjectBID bigint NULL
GO
COMMIT

UPDATE
	apollo_related_objects
	SET
	ObjectAID = c.ID
	FROM
	apollo_related_objects ro
	INNER JOIN apollo_content c ON c.f_uid = ro.f_a_object_uid

UPDATE
	apollo_related_objects
	SET
	ObjectBID = c.ID
	FROM
	apollo_related_objects ro
	INNER JOIN apollo_content c ON c.f_uid = ro.f_b_object_uid
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_apollo_related_objects
	(
	ObjectAID bigint NOT NULL,
	ObjectBID bigint NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.apollo_related_objects)
	 EXEC('INSERT INTO dbo.Tmp_apollo_related_objects (ObjectAID, ObjectBID)
		SELECT ObjectAID, ObjectBID FROM dbo.apollo_related_objects WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.apollo_related_objects
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_related_objects', N'apollo_related_objects', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_related_objects ADD CONSTRAINT
	PK_apollo_related_objects_1 PRIMARY KEY CLUSTERED 
	(
	ObjectAID,
	ObjectBID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- Section Restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.Sections.DefaultDocumentID', N'Tmp_old_DefaultDocumentID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sections.Tmp_old_DefaultDocumentID', N'old_DefaultDocumentID', 'COLUMN' 
GO
ALTER TABLE dbo.Sections ADD
	DefaultDocumentID bigint NULL
GO
COMMIT

UPDATE
	Sections
	SET
	DefaultDocumentID = c.ID
	FROM
	Sections s
	INNER JOIN apollo_content c ON c.f_uid = s.old_DefaultDocumentID
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sections
	DROP COLUMN old_DefaultDocumentID
GO
COMMIT

-- DocumentMappings Restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DocumentMappings
	DROP CONSTRAINT DF_DocumentMappings_IsFeatured
GO
CREATE TABLE dbo.Tmp_DocumentMappings
	(
	old_DocumentID uniqueidentifier NOT NULL,
	DocumentID bigint NULL,
	ParentSectionID int NOT NULL,
	IsFeaturedDocument bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_DocumentMappings ADD CONSTRAINT
	DF_DocumentMappings_IsFeatured DEFAULT ((0)) FOR IsFeaturedDocument
GO
IF EXISTS(SELECT * FROM dbo.DocumentMappings)
	 EXEC('INSERT INTO dbo.Tmp_DocumentMappings (old_DocumentID, ParentSectionID, IsFeaturedDocument)
		SELECT DocumentID, ParentSectionID, IsFeaturedDocument FROM dbo.DocumentMappings WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.DocumentMappings
GO
EXECUTE sp_rename N'dbo.Tmp_DocumentMappings', N'DocumentMappings', 'OBJECT' 
GO
COMMIT

DELETE FROM documentmappings WHERE old_documentid NOT IN (SELECT f_uid FROM apollo_content)
GO

UPDATE
	DocumentMappings
	SET
	DocumentID = c.ID
	FROM
	DocumentMappings dm
	INNER JOIN apollo_content c ON c.f_uid = dm.old_DocumentID
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DocumentMappings
	DROP CONSTRAINT DF_DocumentMappings_IsFeatured
GO
CREATE TABLE dbo.Tmp_DocumentMappings
	(
	DocumentID bigint NOT NULL,
	ParentSectionID int NOT NULL,
	IsFeaturedDocument bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_DocumentMappings ADD CONSTRAINT
	DF_DocumentMappings_IsFeatured DEFAULT ((0)) FOR IsFeaturedDocument
GO
IF EXISTS(SELECT * FROM dbo.DocumentMappings)
	 EXEC('INSERT INTO dbo.Tmp_DocumentMappings (DocumentID, ParentSectionID, IsFeaturedDocument)
		SELECT DocumentID, ParentSectionID, IsFeaturedDocument FROM dbo.DocumentMappings WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.DocumentMappings
GO
EXECUTE sp_rename N'dbo.Tmp_DocumentMappings', N'DocumentMappings', 'OBJECT' 
GO
ALTER TABLE dbo.DocumentMappings ADD CONSTRAINT
	PK_DocumentMappings PRIMARY KEY CLUSTERED 
	(
	DocumentID,
	ParentSectionID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

CREATE NONCLUSTERED INDEX [IDX_Secondary] ON [dbo].[DocumentMappings] 
(
	[IsFeaturedDocument] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- DirectoryItems Restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DirectoryItems
	DROP CONSTRAINT DF_DirectoryItems_Created
GO
ALTER TABLE dbo.DirectoryItems
	DROP CONSTRAINT DF_DirectoryItems_Updated
GO
CREATE TABLE dbo.Tmp_DirectoryItems
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	UID uniqueidentifier NOT NULL,
	Title varchar(256) NOT NULL,
	Description text NOT NULL,
	TelephoneNumber varchar(20) NULL,
	Keywords text NULL,
	Links text NULL,
	Images text NULL,
	Rating bigint NOT NULL,
	NumberOfRatings int NOT NULL,
	Submiter uniqueidentifier NOT NULL,
	Status tinyint NOT NULL,
	Created datetime NOT NULL,
	Updated datetime NOT NULL,
	Longitude float(53) NULL,
	Latitude float(53) NULL,
	Postcode varchar(8) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_DirectoryItems ADD CONSTRAINT
	DF_DirectoryItems_Created DEFAULT (getdate()) FOR Created
GO
ALTER TABLE dbo.Tmp_DirectoryItems ADD CONSTRAINT
	DF_DirectoryItems_Updated DEFAULT (getdate()) FOR Updated
GO
SET IDENTITY_INSERT dbo.Tmp_DirectoryItems ON
GO
IF EXISTS(SELECT * FROM dbo.DirectoryItems)
	 EXEC('INSERT INTO dbo.Tmp_DirectoryItems (ID, UID, Title, Description, TelephoneNumber, Keywords, Links, Images, Rating, NumberOfRatings, Submiter, Status, Created, Updated, Longitude, Latitude, Postcode)
		SELECT ID, UID, Title, Description, TelephoneNumber, Keywords, Links, Images, Rating, NumberOfRatings, Submiter, Status, Created, Updated, Longitude, Latitude, Postcode FROM dbo.DirectoryItems WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_DirectoryItems OFF
GO
DROP TABLE dbo.DirectoryItems
GO
EXECUTE sp_rename N'dbo.Tmp_DirectoryItems', N'DirectoryItems', 'OBJECT' 
GO
ALTER TABLE dbo.DirectoryItems ADD CONSTRAINT
	PK_DirectoryItems_1 PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX DI_UID ON dbo.DirectoryItems
	(
	UID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DirectoryItems
	DROP CONSTRAINT DF_DirectoryItems_Created
GO
ALTER TABLE dbo.DirectoryItems
	DROP CONSTRAINT DF_DirectoryItems_Updated
GO
CREATE TABLE dbo.Tmp_DirectoryItems
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	UID uniqueidentifier NULL,
	Title varchar(256) NOT NULL,
	Description text NOT NULL,
	TelephoneNumber varchar(20) NULL,
	Keywords text NULL,
	Links text NULL,
	Images text NULL,
	Rating bigint NOT NULL,
	NumberOfRatings int NOT NULL,
	Submiter uniqueidentifier NOT NULL,
	Status tinyint NOT NULL,
	Created datetime NOT NULL,
	Updated datetime NOT NULL,
	Longitude float(53) NULL,
	Latitude float(53) NULL,
	Postcode varchar(8) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_DirectoryItems ADD CONSTRAINT
	DF_DirectoryItems_Created DEFAULT (getdate()) FOR Created
GO
ALTER TABLE dbo.Tmp_DirectoryItems ADD CONSTRAINT
	DF_DirectoryItems_Updated DEFAULT (getdate()) FOR Updated
GO
SET IDENTITY_INSERT dbo.Tmp_DirectoryItems ON
GO
IF EXISTS(SELECT * FROM dbo.DirectoryItems)
	 EXEC('INSERT INTO dbo.Tmp_DirectoryItems (ID, UID, Title, Description, TelephoneNumber, Keywords, Links, Images, Rating, NumberOfRatings, Submiter, Status, Created, Updated, Longitude, Latitude, Postcode)
		SELECT ID, UID, Title, Description, TelephoneNumber, Keywords, Links, Images, Rating, NumberOfRatings, Submiter, Status, Created, Updated, Longitude, Latitude, Postcode FROM dbo.DirectoryItems WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_DirectoryItems OFF
GO
DROP TABLE dbo.DirectoryItems
GO
EXECUTE sp_rename N'dbo.Tmp_DirectoryItems', N'DirectoryItems', 'OBJECT' 
GO
ALTER TABLE dbo.DirectoryItems ADD CONSTRAINT
	PK_DirectoryItems_1 PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX DI_UID ON dbo.DirectoryItems
	(
	UID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT


-- apollo_content Restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_content
	DROP CONSTRAINT DF_apollo_content_f_creation_date
GO
ALTER TABLE dbo.apollo_content
	DROP CONSTRAINT DF_apollo_document_Type
GO
CREATE TABLE dbo.Tmp_apollo_content
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	f_uid uniqueidentifier NULL,
	f_title varchar(512) NOT NULL,
	f_author uniqueidentifier NOT NULL,
	f_creation_date datetime NOT NULL,
	f_publish_date datetime NOT NULL,
	f_lead_statement varchar(512) NULL,
	f_abstract text NULL,
	f_body text NOT NULL,
	f_status varchar(20) NOT NULL,
	Type tinyint NOT NULL,
	Tags text NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_apollo_content ADD CONSTRAINT
	DF_apollo_content_f_creation_date DEFAULT (getdate()) FOR f_creation_date
GO
EXECUTE sp_bindefault N'dbo.nulldate', N'dbo.Tmp_apollo_content.f_publish_date'
GO
ALTER TABLE dbo.Tmp_apollo_content ADD CONSTRAINT
	DF_apollo_document_Type DEFAULT ((0)) FOR Type
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_content ON
GO
IF EXISTS(SELECT * FROM dbo.apollo_content)
	 EXEC('INSERT INTO dbo.Tmp_apollo_content (ID, f_uid, f_title, f_author, f_creation_date, f_publish_date, f_lead_statement, f_abstract, f_body, f_status, Type, Tags)
		SELECT ID, f_uid, f_title, f_author, f_creation_date, f_publish_date, f_lead_statement, f_abstract, f_body, f_status, Type, Tags FROM dbo.apollo_content WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_content OFF
GO
DROP TABLE dbo.apollo_content
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_content', N'apollo_content', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_content ADD CONSTRAINT
	PK_apollo_content PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IDX_Secondary ON dbo.apollo_content
	(
	ID,
	f_publish_date
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT

-- apollo_galleries Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_galleries
	DROP CONSTRAINT DF_apollo_galleries_f_views
GO
CREATE TABLE dbo.Tmp_apollo_galleries
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	f_uid uniqueidentifier NULL,
	f_title varchar(512) NOT NULL,
	f_description text NULL,
	f_creation_date datetime NOT NULL,
	f_type tinyint NOT NULL,
	f_is_public bit NOT NULL,
	f_status tinyint NOT NULL,
	f_views int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_apollo_galleries ADD CONSTRAINT
	DF_apollo_galleries_f_views DEFAULT ((0)) FOR f_views
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_galleries ON
GO
IF EXISTS(SELECT * FROM dbo.apollo_galleries)
	 EXEC('INSERT INTO dbo.Tmp_apollo_galleries (ID, f_uid, f_title, f_description, f_creation_date, f_type, f_is_public, f_status, f_views)
		SELECT ID, f_uid, f_title, f_description, f_creation_date, f_type, f_is_public, f_status, f_views FROM dbo.apollo_galleries WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_galleries OFF
GO
DROP TABLE dbo.apollo_galleries
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_galleries', N'apollo_galleries', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_galleries ADD CONSTRAINT
	PK_apollo_galleries PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- apollo-images Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.apollo_images
	DROP CONSTRAINT DF_apollo_images_f_created
GO
ALTER TABLE dbo.apollo_images
	DROP CONSTRAINT DF_apollo_images_Type
GO
CREATE TABLE dbo.Tmp_apollo_images
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	f_uid uniqueidentifier NULL,
	f_name varchar(255) NOT NULL,
	f_filename varchar(255) NOT NULL,
	f_width int NOT NULL,
	f_height int NOT NULL,
	f_created datetime NOT NULL,
	Type tinyint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_apollo_images ADD CONSTRAINT
	DF_apollo_images_f_created DEFAULT (getdate()) FOR f_created
GO
ALTER TABLE dbo.Tmp_apollo_images ADD CONSTRAINT
	DF_apollo_images_Type DEFAULT ((0)) FOR Type
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_images ON
GO
IF EXISTS(SELECT * FROM dbo.apollo_images)
	 EXEC('INSERT INTO dbo.Tmp_apollo_images (ID, f_uid, f_name, f_filename, f_width, f_height, f_created, Type)
		SELECT ID, f_uid, f_name, f_filename, f_width, f_height, f_created, Type FROM dbo.apollo_images WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_images OFF
GO
DROP TABLE dbo.apollo_images
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_images', N'apollo_images', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_images ADD CONSTRAINT
	PK_apollo_images PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- DirectoryCategories Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DirectoryCategories
	DROP CONSTRAINT DF_DirectoryCategories_RequiresMembership
GO
CREATE TABLE dbo.Tmp_DirectoryCategories
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	UID uniqueidentifier NULL,
	Name varchar(256) NOT NULL,
	Description text NULL,
	RequiresMembership bit NOT NULL,
	Keywords text NULL,
	ParentCategoryUID uniqueidentifier NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_DirectoryCategories ADD CONSTRAINT
	DF_DirectoryCategories_RequiresMembership DEFAULT ((0)) FOR RequiresMembership
GO
SET IDENTITY_INSERT dbo.Tmp_DirectoryCategories ON
GO
IF EXISTS(SELECT * FROM dbo.DirectoryCategories)
	 EXEC('INSERT INTO dbo.Tmp_DirectoryCategories (ID, UID, Name, Description, RequiresMembership, Keywords, ParentCategoryUID)
		SELECT ID, UID, Name, Description, RequiresMembership, Keywords, ParentCategoryUID FROM dbo.DirectoryCategories WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_DirectoryCategories OFF
GO
DROP TABLE dbo.DirectoryCategories
GO
EXECUTE sp_rename N'dbo.Tmp_DirectoryCategories', N'DirectoryCategories', 'OBJECT' 
GO
ALTER TABLE dbo.DirectoryCategories ADD CONSTRAINT
	PK_DirectoryCategories PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX SearchTerms ON dbo.DirectoryCategories
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT

-- apollo-gallery-categories restructuring
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_apollo_gallery_categories
	(
	ID bigint NOT NULL IDENTITY (1, 1),
	f_uid uniqueidentifier NULL,
	f_name varchar(100) NULL,
	f_owner uniqueidentifier NULL,
	f_description text NULL,
	f_type tinyint NOT NULL,
	f_active bit NOT NULL,
	ParentSiteID int NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_gallery_categories ON
GO
IF EXISTS(SELECT * FROM dbo.apollo_gallery_categories)
	 EXEC('INSERT INTO dbo.Tmp_apollo_gallery_categories (ID, f_uid, f_name, f_owner, f_description, f_type, f_active, ParentSiteID)
		SELECT ID, f_uid, f_name, f_owner, f_description, f_type, f_active, ParentSiteID FROM dbo.apollo_gallery_categories WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_apollo_gallery_categories OFF
GO
DROP TABLE dbo.apollo_gallery_categories
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_gallery_categories', N'apollo_gallery_categories', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_gallery_categories ADD CONSTRAINT
	PK_apollo_gallery_categories PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IDX_Secondary ON dbo.apollo_gallery_categories
	(
	ID,
	ParentSiteID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT

-- DirectoryCategories Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DirectoryCategories ADD
	ParentCategoryID bigint NULL
GO
COMMIT

UPDATE
	DirectoryCategories
	SET
	ParentCategoryID = (SELECT dc2.ID FROM DirectoryCategories AS dc2 WHERE dc2.UID = DirectoryCategories.ParentCategoryUID)
	WHERE
	ParentCategoryUID IS NOT NULL
	
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DirectoryCategories
	DROP COLUMN ParentCategoryUID
GO
COMMIT

-- DirectoryCategoryItems Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DirectoryCategoryItems ADD
	CategoryID bigint NULL,
	ItemID bigint NULL
GO
COMMIT

UPDATE DirectoryCategoryItems SET CategoryID = dc.ID FROM DirectoryCategoryItems dci INNER JOIN DirectoryCategories dc ON dc.UID = dci.CategoryUID
UPDATE DirectoryCategoryItems SET ItemID = di.ID FROM DirectoryCategoryItems dci INNER JOIN DirectoryItems di ON di.UID = dci.ItemUID

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_DirectoryCategoryItems
	(
	CategoryID bigint NOT NULL,
	ItemID bigint NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.DirectoryCategoryItems)
	 EXEC('INSERT INTO dbo.Tmp_DirectoryCategoryItems (CategoryID, ItemID)
		SELECT CategoryID, ItemID FROM dbo.DirectoryCategoryItems WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.DirectoryCategoryItems
GO
EXECUTE sp_rename N'dbo.Tmp_DirectoryCategoryItems', N'DirectoryCategoryItems', 'OBJECT' 
GO
ALTER TABLE dbo.DirectoryCategoryItems ADD CONSTRAINT
	PK_DirectoryCategoryItems_1 PRIMARY KEY CLUSTERED 
	(
	CategoryID,
	ItemID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- apollo-content-images Reconstruction
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_apollo_content_images
	(
	f_content_uid uniqueidentifier NOT NULL,
	f_image_uid uniqueidentifier NOT NULL,
	ContentID bigint NULL,
	ImageID bigint NULL,
	f_cover_image bit NOT NULL,
	IntroImage bit NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.apollo_content_images)
	 EXEC('INSERT INTO dbo.Tmp_apollo_content_images (f_content_uid, f_image_uid, f_cover_image, IntroImage)
		SELECT f_content_uid, f_image_uid, f_cover_image, IntroImage FROM dbo.apollo_content_images WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.apollo_content_images
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_content_images', N'apollo_content_images', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_content_images ADD CONSTRAINT
	PK_apollo_content_images PRIMARY KEY CLUSTERED 
	(
	f_content_uid,
	f_image_uid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

UPDATE apollo_content_images SET ContentID = c.ID FROM apollo_content_images aci INNER JOIN apollo_content c ON c.f_uid = aci.f_content_uid
UPDATE apollo_content_images SET ImageID = i.ID FROM apollo_content_images aci INNER JOIN apollo_images i ON i.f_uid = aci.f_image_uid
delete from apollo_content_images where f_content_uid not in (select f_uid from apollo_content)
delete from apollo_content_images where f_image_uid not in (select f_uid from apollo_images)

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_apollo_content_images
	(
	ContentID bigint NOT NULL,
	ImageID bigint NOT NULL,
	f_cover_image bit NOT NULL,
	IntroImage bit NOT NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.apollo_content_images)
	 EXEC('INSERT INTO dbo.Tmp_apollo_content_images (ContentID, ImageID, f_cover_image, IntroImage)
		SELECT ContentID, ImageID, f_cover_image, IntroImage FROM dbo.apollo_content_images WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.apollo_content_images
GO
EXECUTE sp_rename N'dbo.Tmp_apollo_content_images', N'apollo_content_images', 'OBJECT' 
GO
ALTER TABLE dbo.apollo_content_images ADD CONSTRAINT
	PK_apollo_content_images PRIMARY KEY CLUSTERED 
	(
	ContentID,
	ImageID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- Comments Notification Changes
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Comments ADD
	ReceiveNotification bit NOT NULL CONSTRAINT DF_Comments_ReceiveNotification DEFAULT 1,
	RequiresNotification bit NOT NULL CONSTRAINT DF_Comments_RequiresNotification DEFAULT 1
GO
COMMIT