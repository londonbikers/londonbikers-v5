/*ALTER TABLE [dbo].[InstantForum_TopicSubscriptions]
    ADD CONSTRAINT [FK_InstantForum_TopicSubscriptions_InstantForum_Topics] FOREIGN KEY ([TopicID]) REFERENCES [dbo].[InstantForum_Topics] ([PostID]) ON DELETE CASCADE ON UPDATE NO ACTION;*/

