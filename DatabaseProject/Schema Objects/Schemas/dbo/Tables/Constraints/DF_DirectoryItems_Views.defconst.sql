ALTER TABLE [dbo].[DirectoryItems]
    ADD CONSTRAINT [DF_DirectoryItems_Views] DEFAULT ((0)) FOR [Views];

