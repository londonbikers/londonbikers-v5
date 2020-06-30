ALTER TABLE [dbo].[Comments]
    ADD CONSTRAINT [DF_Comments_ReportStatus] DEFAULT ((0)) FOR [ReportStatus];

