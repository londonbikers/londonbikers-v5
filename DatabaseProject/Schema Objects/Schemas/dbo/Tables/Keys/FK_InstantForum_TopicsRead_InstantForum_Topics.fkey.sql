/*ALTER TABLE [dbo].[InstantForum_TopicsRead]
    ADD CONSTRAINT [FK_InstantForum_TopicsRead_InstantForum_Topics] FOREIGN KEY ([ReadTopicID]) REFERENCES [dbo].[InstantForum_Topics] ([PostID]) ON DELETE CASCADE ON UPDATE NO ACTION;*/

