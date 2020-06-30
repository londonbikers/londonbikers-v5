ALTER TABLE [dbo].[DocumentMappings]
    ADD CONSTRAINT [DF_DocumentMappings_IsFeatured] DEFAULT ((0)) FOR [IsFeaturedDocument];

