IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'GalleryImages')
	BEGIN
		PRINT 'Dropping Table GalleryImages'
		DROP Table GalleryImages
	END
GO

/******************************************************************************
**		File: GalleryImages.sql 
**		Name: GalleryImages
**		Desc: 
**
**		This template can be customized:
**              
**
**		Auth: Jay Adair
**		Date: 07.05.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			-------------------------------------------
**    
*******************************************************************************/

PRINT 'Creating Table GalleryImages'
GO

CREATE TABLE [GalleryImages] (
	[Uid] [uniqueidentifier] NOT NULL ,
	[Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Credit] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Comment] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[CaptureDate] [datetime] NOT NULL ,
	[CreationDate] [datetime] NOT NULL ,
	[BaseUrl] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Filename800] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Filename1024] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Filename1280] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Filename1600] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ThumbnailFilename] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[CustomThumbnailFilename] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Views] [int] NOT NULL CONSTRAINT [DF_apollo_gallery_images_f_views] DEFAULT (0),
	[GalleryUID] [uniqueidentifier] NULL ,
	CONSTRAINT [PK_apollo_gallery_images] PRIMARY KEY  CLUSTERED 
	(
		[Uid]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GRANT SELECT ON GalleryImages TO PUBLIC
GO