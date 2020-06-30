ALTER TABLE [dbo].[apollo_site_text]
    ADD CONSTRAINT [DF_apollo_sitetext_LanguageID] DEFAULT ((1)) FOR [LanguageID];

