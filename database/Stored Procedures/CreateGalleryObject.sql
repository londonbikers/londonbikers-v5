IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CreateGalleryVideo')
	BEGIN
		PRINT 'Dropping Procedure CreateGalleryVideo'
		DROP Procedure CreateGalleryVideo
	END
GO

PRINT 'Creating Procedure CreateGalleryVideo'
GO

CREATE Procedure CreateGalleryVideo
	@uid uniqueidentifier,
	@name varchar(256),
	@comment text,
	@creationDate datetime,
	@captureDate datetime,
	@codec tinyint,
	@filename varchar(256),
	@thumbnail varchar(256)
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

	INSERT INTO apollo_gallery_videos
	(
		f_uid,
		f_name,
		f_comment,
		f_creation_date,
		f_capture_date,
		f_codec,
		f_filename,
		f_thumbnail_filename
	)
	VALUES
	(
		@uid,
		@name,
		@comment,
		@creationDate,
		@captureDate,
		@codec,
		@filename,
		@thumbnail
	)

GO

GRANT EXEC ON CreateGalleryVideo TO PUBLIC
GO