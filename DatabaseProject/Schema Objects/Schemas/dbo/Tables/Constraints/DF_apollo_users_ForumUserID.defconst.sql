ALTER TABLE [dbo].[apollo_users]
    ADD CONSTRAINT [DF_apollo_users_ForumUserID] DEFAULT ((0)) FOR [ForumUserID];

