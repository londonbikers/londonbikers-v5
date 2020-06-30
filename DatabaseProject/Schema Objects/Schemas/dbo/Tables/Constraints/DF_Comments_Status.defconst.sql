ALTER TABLE [dbo].[Comments]
    ADD CONSTRAINT [DF_Comments_Status] DEFAULT ((1)) FOR [Status];

