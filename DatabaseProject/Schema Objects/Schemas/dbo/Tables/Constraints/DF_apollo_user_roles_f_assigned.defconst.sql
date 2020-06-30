ALTER TABLE [dbo].[apollo_user_roles]
    ADD CONSTRAINT [DF_apollo_user_roles_f_assigned] DEFAULT (getdate()) FOR [f_assigned];

