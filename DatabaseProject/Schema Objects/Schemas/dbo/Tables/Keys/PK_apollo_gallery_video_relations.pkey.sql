ALTER TABLE [dbo].[apollo_gallery_video_relations]
    ADD CONSTRAINT [PK_apollo_gallery_video_relations] PRIMARY KEY CLUSTERED ([f_gallery_uid] ASC, [f_video_uid] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

