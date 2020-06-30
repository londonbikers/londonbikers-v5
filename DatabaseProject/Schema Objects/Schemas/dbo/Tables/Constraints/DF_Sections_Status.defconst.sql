ALTER TABLE [dbo].[Sections]
    ADD CONSTRAINT [DF_Sections_Status] DEFAULT ((0)) FOR [Status];

