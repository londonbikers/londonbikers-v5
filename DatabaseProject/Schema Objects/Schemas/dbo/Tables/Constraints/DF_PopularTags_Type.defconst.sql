ALTER TABLE [dbo].[PopularTags]
    ADD CONSTRAINT [DF_PopularTags_Type] DEFAULT ((1)) FOR [Type];

