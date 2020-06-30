ALTER TABLE [dbo].[DirectoryItems]
    ADD CONSTRAINT [DF_DirectoryItems_Updated] DEFAULT (getdate()) FOR [Updated];

