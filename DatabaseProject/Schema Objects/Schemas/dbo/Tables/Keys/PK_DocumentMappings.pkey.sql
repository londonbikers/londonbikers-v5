﻿ALTER TABLE [dbo].[DocumentMappings]
    ADD CONSTRAINT [PK_DocumentMappings] PRIMARY KEY CLUSTERED ([DocumentID] ASC, [ParentSectionID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

