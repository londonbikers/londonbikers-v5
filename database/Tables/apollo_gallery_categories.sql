/****** Object:  Table [dbo].[apollo_gallery_categories]    Script Date: 09/07/2006 00:41:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[apollo_gallery_categories](
	[f_uid] [uniqueidentifier] NOT NULL,
	[f_name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[f_owner] [uniqueidentifier] NULL,
	[f_description] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[f_parent] [uniqueidentifier] NULL,
	[f_type] [tinyint] NOT NULL,
	[f_active] [bit] NOT NULL,
	[ParentSiteID] [int] NULL,
 CONSTRAINT [PK_apollo_gallery_categories] PRIMARY KEY CLUSTERED 
(
	[f_uid] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF 