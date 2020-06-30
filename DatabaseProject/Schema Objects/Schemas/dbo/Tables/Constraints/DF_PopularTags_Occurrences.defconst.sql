ALTER TABLE [dbo].[PopularTags]
    ADD CONSTRAINT [DF_PopularTags_Occurrences] DEFAULT ((1)) FOR [Occurrences];

