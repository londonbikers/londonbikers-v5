﻿ALTER TABLE [dbo].[PopularTags]
    ADD CONSTRAINT [PK_PopularTags] PRIMARY KEY CLUSTERED ([SectionID] ASC, [Tag] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

