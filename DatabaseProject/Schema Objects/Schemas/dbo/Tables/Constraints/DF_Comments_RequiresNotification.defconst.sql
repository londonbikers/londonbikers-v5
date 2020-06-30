ALTER TABLE [dbo].[Comments]
    ADD CONSTRAINT [DF_Comments_RequiresNotification] DEFAULT ((1)) FOR [RequiresNotification];

