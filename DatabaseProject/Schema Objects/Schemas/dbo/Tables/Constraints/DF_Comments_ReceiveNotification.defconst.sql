ALTER TABLE [dbo].[Comments]
    ADD CONSTRAINT [DF_Comments_ReceiveNotification] DEFAULT ((1)) FOR [ReceiveNotification];

