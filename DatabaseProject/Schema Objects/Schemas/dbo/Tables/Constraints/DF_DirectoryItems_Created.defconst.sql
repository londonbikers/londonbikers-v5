ALTER TABLE [dbo].[DirectoryItems]
    ADD CONSTRAINT [DF_DirectoryItems_Created] DEFAULT (getdate()) FOR [Created];

