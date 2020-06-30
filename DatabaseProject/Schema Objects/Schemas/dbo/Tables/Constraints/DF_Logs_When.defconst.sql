ALTER TABLE [dbo].[Logs]
    ADD CONSTRAINT [DF_Logs_When] DEFAULT (getdate()) FOR [When];

