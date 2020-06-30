/****** Object:  Table [dbo].[Sections]    Script Date: 12/28/2006 18:38:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sections](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShortDescription] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UrlIdentifier] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContentTypeID] [tinyint] NOT NULL,
	[ParentChannelID] [int] NULL,
	[ParentSiteID] [int] NULL,
	[FavouriteTags] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DefaultDocumentID] [uniqueidentifier] NULL,
	[Status] [tinyint] NOT NULL CONSTRAINT [DF_Sections_Status]  DEFAULT ((0)),
 CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF