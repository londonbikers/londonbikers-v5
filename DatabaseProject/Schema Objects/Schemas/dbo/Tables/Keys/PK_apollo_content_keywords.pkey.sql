ALTER TABLE [dbo].[apollo_content_keywords]
    ADD CONSTRAINT [PK_apollo_content_keywords] PRIMARY KEY CLUSTERED ([f_object_uid] ASC, [f_keyword_uid] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

