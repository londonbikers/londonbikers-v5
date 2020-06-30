SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AuthorID] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Comments_Created]  DEFAULT (getdate()),
	[Comment] [ntext] NOT NULL,
	[Status] [tinyint] NOT NULL CONSTRAINT [DF_Comments_Status]  DEFAULT ((1)),
	[OwnerID] [bigint] NULL,
	[OwnerUID] [uniqueidentifier] NULL,
	[OwnerType] [tinyint] NOT NULL,
	[ReportStatus] [tinyint] NOT NULL CONSTRAINT [DF_Comments_ReportStatus]  DEFAULT ((0)),
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
