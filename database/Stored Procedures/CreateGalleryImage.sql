IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CreateGalleryImage')
	BEGIN
		PRINT 'Dropping Procedure CreateGalleryImage'
		DROP Procedure CreateGalleryImage
	END
GO

PRINT 'Creating Procedure CreateGalleryImage'
GO

CREATE Procedure CreateGalleryImage
	@uid uniqueidentifier,
	@name varchar(256),
	@comment text,
	@credit varchar(512),
	@creationDate datetime,
	@captureDate datetime,
	@baseUrl varchar(256),
	@800 varchar(256),
	@1024 varchar(256),
	@1280 varchar(256),
	@1600 varchar(256),
	@thumbnail varchar(256),
	@customThumbnail varchar(256),
	@GalleryUID uniqueidentifier
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
**      ----------						-----------
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

INSERT INTO
	GalleryImages
	(
		Uid,
		[Name],
		Comment,
		Credit,
		CreationDate,
		CaptureDate,
		BaseUrl,
		Filename800,
		Filename1024,
		Filename1280,
		Filename1600,
		ThumbnailFilename,
		CustomThumbnailFilename,
		GalleryUID
	)
	VALUES
	(
		@uid,
		@name,
		@comment,
		@credit,
		@creationDate,
		@captureDate,
		@baseUrl,
		@800,
		@1024,
		@1280,
		@1600,
		@thumbnail,
		@customThumbnail,
		@GalleryUID
	)

GO

GRANT EXEC ON CreateGalleryImage TO PUBLIC
GO