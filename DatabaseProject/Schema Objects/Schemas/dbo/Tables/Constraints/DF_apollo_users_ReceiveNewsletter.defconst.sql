ALTER TABLE [dbo].[apollo_users]
    ADD CONSTRAINT [DF_apollo_users_ReceiveNewsletter] DEFAULT ((1)) FOR [ReceiveNewsletter];

