ALTER TABLE [dbo].[GalleryImages]
    ADD CONSTRAINT [DF_apollo_gallery_images_f_views] DEFAULT ((0)) FOR [Views];

