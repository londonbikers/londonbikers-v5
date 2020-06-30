ALTER TABLE [dbo].[apollo_content]
    ADD CONSTRAINT [DF_apollo_document_Type] DEFAULT ((0)) FOR [Type];

