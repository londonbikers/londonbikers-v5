ALTER TABLE [dbo].[apollo_images]
    ADD CONSTRAINT [DF_apollo_images_Type] DEFAULT ((0)) FOR [Type];

