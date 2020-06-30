/*
-------------------------------------------------------------

CREATE TRIGGER [dbo].[SyncMemberAccounts] 
ON [dbo].[InstantASP_Users] 
AFTER UPDATE 
AS 
        -- if the username has been changed, keep the Apollo record in sync. 
		SET NOCOUNT ON

		IF (SELECT COUNT(0) FROM inserted) = 1
		BEGIN
			IF (SELECT Username FROM inserted) <> (SELECT Username FROM deleted)
			BEGIN 
					UPDATE 
							apollo_users 
							SET 
							f_username = (SELECT Username FROM inserted)
							WHERE 
							ForumUserID = (SELECT UserId FROM inserted)
			END

			IF (SELECT EmailAddress FROM inserted) <> (SELECT EmailAddress FROM deleted)
			BEGIN 
					UPDATE 
							apollo_users 
							SET 
							f_email = (SELECT EmailAddress FROM inserted)
							WHERE 
							ForumUserID = (SELECT UserId FROM inserted)
			END
		END*/
