
-------------------------------------------------------------------

CREATE Procedure [dbo].[UpdateUser]
	@UID uniqueidentifier,
	@Firstname varchar(100),
	@Lastname varchar(100),
	@Password varchar(50),
	@Email varchar(512),
	@Username varchar(100),
	@Status tinyint,
	@ForumUserID int
AS	
	SET NOCOUNT ON
	IF (SELECT COUNT(0) FROM apollo_users WHERE f_uid = @UID) > 0
	BEGIN
		-- UPDATE AN EXISTING USER
		UPDATE
		apollo_users
		SET
		f_firstname = @Firstname,
		f_lastname = @Lastname,
		f_password = @Password,
		f_email = @Email,
		f_username = @Username,
		[Status] = @Status,
		ForumUserID = @ForumUserID
		WHERE
		f_uid = @UID
	END
	ELSE
		-- CREATE A NEW USER
		INSERT INTO apollo_users
		(
			f_uid,
			f_firstname,
			f_lastname,
			f_password,
			f_email,
			f_username,
			f_created,
			[Status],
			ForumUserID
		)
		VALUES
		(
			@UID,
			@Firstname,
			@Lastname,
			@Password,
			@Email,
			@Username,
			getdate(),
			@Status,
			@ForumUserID
		)
