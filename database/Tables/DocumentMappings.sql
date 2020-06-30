/****** Object:  Table [dbo].[DocumentMappings]    Script Date: 02/12/2007 20:21:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentMappings](
	[DocumentID] [uniqueidentifier] NOT NULL,
	[ParentSectionID] [int] NOT NULL,
	[IsFeaturedDocument] [bit] NOT NULL CONSTRAINT [DF_DocumentMappings_IsFeatured]  DEFAULT ((0))
) ON [PRIMARY]
