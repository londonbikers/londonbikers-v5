ALTER TABLE [dbo].[apollo_gallery_category_gallery_relations]
    ADD CONSTRAINT [PK_apollo_gallery_category_gallery_relations_1] PRIMARY KEY CLUSTERED ([CategoryID] ASC, [GalleryID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

