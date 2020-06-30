ALTER TABLE [dbo].[DirectoryCategories]
    ADD CONSTRAINT [DF_DirectoryCategories_RequiresMembership] DEFAULT ((0)) FOR [RequiresMembership];

