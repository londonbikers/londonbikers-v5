ALTER TABLE [dbo].[apollo_images]
    ADD CONSTRAINT [DF_apollo_images_f_created] DEFAULT (getdate()) FOR [f_created];

