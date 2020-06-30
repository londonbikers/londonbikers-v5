ALTER TABLE [dbo].[apollo_content]
    ADD CONSTRAINT [DF_apollo_content_f_creation_date] DEFAULT (getdate()) FOR [f_creation_date];

