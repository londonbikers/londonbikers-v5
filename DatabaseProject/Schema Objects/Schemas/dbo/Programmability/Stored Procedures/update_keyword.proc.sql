CREATE PROCEDURE [dbo].[update_keyword]
	(
		@UID uniqueidentifier,
		@Name varchar(256),
		@Url varchar(512),
		@Definition text,
		@Author uniqueidentifier
	)
AS
	UPDATE
	
			apollo_keywords
			
	SET
	
		f_uid = @UID,
		f_name = @Name,
		f_url = @Url,
		f_definition = @Definition,
		f_author = @Author
		
	WHERE
	
		f_uid = @UID
	SET NOCOUNT ON
	RETURN
